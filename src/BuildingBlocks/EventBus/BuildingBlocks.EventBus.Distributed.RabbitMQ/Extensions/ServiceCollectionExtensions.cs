using BuildingBlocks.EventBus.Abstraction.Distributed;
using BuildingBlocks.EventBus.Distributed.RabbitMQ.Configuration;
using MassTransit;
using MassTransit.Internals;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.EventBus.Distributed.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqMassTransitEventBus(
                        this IServiceCollection services,
                        Assembly assembly,
                        Action<RabbitMqOptions> configureOptions)
        {

            var options = new RabbitMqOptions();
            configureOptions?.Invoke(options);

            services.AddScoped(typeof(RabbitMqEventConsumer<>));
            services.AddScoped<IDistributedEventBus, RabbitMqEventBus>();

            services.AddMassTransit(cfg =>
            {
                IEnumerable<Type> messageTypesWithAttributes = GetMessageTypes(assembly);

                foreach (var messageType in messageTypesWithAttributes)
                {
                    var consumerType = typeof(RabbitMqEventConsumer<>).MakeGenericType(messageType);
                    cfg.AddConsumer(consumerType);
                }

                cfg.UsingRabbitMq((context, busCfg) =>
                {
                    busCfg.Host(new Uri(options.HostUrl), h =>
                    {
                        if (!string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.Password))
                        {
                            h.Username(options.Username);
                            h.Password(options.Password);
                        }
                    });

                    busCfg.ConfigureWithAttributes(cfg, context, messageTypesWithAttributes);
                });
            });

            return services;
        }
        private static void ConfigureWithAttributes(this IRabbitMqBusFactoryConfigurator rfc, IBusRegistrationConfigurator brc,
                                                   IRegistrationContext context, IEnumerable<Type> messageTypesWithAttributes)
        {
            foreach (var messageType in messageTypesWithAttributes)
            {
                var attribute = messageType.GetCustomAttribute<DistributedMessageAttribute>()!;

                var consumerType = typeof(RabbitMqEventConsumer<>).MakeGenericType(messageType);
                if (consumerType == null)
                    continue;

                rfc.ReceiveEndpoint(attribute.Destination, e =>
                {
                    e.Bind(attribute.Subscription, x =>
                    {
                        x.ExchangeType = attribute.DistributionStrategy.ToLower();
                        x.RoutingKey = messageType.FullName;
                    });

                    e.ConfigureConsumer(context, consumerType);
                });
            }
        }

        private static IEnumerable<Type> GetMessageTypes(Assembly assembly)
        {
            var handlerInterfaceType = typeof(IDistributedEventHandler<>);

            return assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                    .Select(i => i.GetGenericArguments()[0]))
                .Where(messageType => messageType.HasAttribute<DistributedMessageAttribute>())
                .Distinct();
        }

    }
}
