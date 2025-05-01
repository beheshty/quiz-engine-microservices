# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY ["QuizEngineMicroservices.sln", "./"]
COPY ["src/QuestionService/QuestionService.API/QuestionService.API.csproj", "src/QuestionService/QuestionService.API/"]
COPY ["src/QuestionService/QuestionService.Application/QuestionService.Application.csproj", "src/QuestionService/QuestionService.Application/"]
COPY ["src/QuestionService/QuestionService.Domain/QuestionService.Domain.csproj", "src/QuestionService/QuestionService.Domain/"]
COPY ["src/QuestionService/QuestionService.Infrastructure/QuestionService.Infrastructure.csproj", "src/QuestionService/QuestionService.Infrastructure/"]
COPY ["src/Shared/Shared.Domain/Shared.Domain.csproj", "src/Shared/Shared.Domain/"]

# Restore dependencies
RUN dotnet restore "src/QuestionService/QuestionService.API/QuestionService.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/src/QuestionService/QuestionService.API"
RUN dotnet build "QuestionService.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "QuestionService.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Add health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost/health || exit 1

# Expose the port the app runs on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "QuestionService.API.dll"] 