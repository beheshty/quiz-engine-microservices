# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/QuizService/QuizService.API/QuizService.API.csproj", "QuizService.API/"]
COPY ["src/QuizService/QuizService.Application/QuizService.Application.csproj", "QuizService.Application/"]
COPY ["src/QuizService/QuizService.Domain/QuizService.Domain.csproj", "QuizService.Domain/"]
COPY ["src/QuizService/QuizService.Infrastructure/QuizService.Infrastructure.csproj", "QuizService.Infrastructure/"]
RUN dotnet restore "QuizService.API/QuizService.API.csproj"
COPY src/QuizService/ .
WORKDIR "/src/QuizService.API"
RUN dotnet build "QuizService.API.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "QuizService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "QuizService.API.dll"]