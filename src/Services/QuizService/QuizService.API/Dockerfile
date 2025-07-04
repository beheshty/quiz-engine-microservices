# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/QuizService/QuizService.API/QuizService.API.csproj", "src/QuizService/QuizService.API/"]
COPY ["src/Services/QuizService/QuizService.Application/QuizService.Application.csproj", "src/QuizService/QuizService.Application/"]
COPY ["src/Services/QuizService/QuizService.Domain/QuizService.Domain.csproj", "src/QuizService/QuizService.Domain/"]
COPY ["src/BuildingBlocks/Domain/BuildingBlocks.Domain/BuildingBlocks.Domain.csproj", "src/BuildingBlocks/BuilidngBlocks.Domain/"]
COPY ["src/Services/QuizService/QuizService.Infrastructure/QuizService.Infrastructure.csproj", "src/QuizService/QuizService.Infrastructure/"]
RUN dotnet restore "./src/Services/QuizService/QuizService.API/QuizService.API.csproj"
COPY . .
WORKDIR "/src/src/QuizService/QuizService.API"
RUN dotnet build "./QuizService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuizService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizService.API.dll"]