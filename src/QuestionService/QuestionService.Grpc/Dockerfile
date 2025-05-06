# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/QuestionService/QuestionService.Grpc/QuestionService.Grpc.csproj", "src/QuestionService/QuestionService.Grpc/"]
COPY ["src/QuestionService/QuestionService.Application/QuestionService.Application.csproj", "src/QuestionService/QuestionService.Application/"]
COPY ["src/QuestionService/QuestionService.Domain/QuestionService.Domain.csproj", "src/QuestionService/QuestionService.Domain/"]
COPY ["src/QuestionService/QuestionService.Infrastructure/QuestionService.Infrastructure.csproj", "src/QuestionService/QuestionService.Infrastructure/"]
COPY ["src/Shared/Shared.Domain/Shared.Domain.csproj", "src/Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Question.Grpc.Proto/Shared.Question.Grpc.Proto.csproj", "src/Shared/Shared.Question.Grpc.Proto/"]
RUN dotnet restore "src/QuestionService/QuestionService.Grpc/QuestionService.Grpc.csproj"
COPY . .
WORKDIR "/src/src/QuestionService/QuestionService.Grpc"
RUN dotnet build "./QuestionService.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuestionService.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuestionService.Grpc.dll"]