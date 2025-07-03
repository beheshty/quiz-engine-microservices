# 🧠 Quiz Engine Microservices

## Overview ✨

This repository hosts a robust and scalable **Quiz Engine Microservices** system, designed to provide a comprehensive platform for managing quizzes, questions, and user interactions. Built with a focus on clean architecture and Domain-Driven Design (DDD) principles, this system leverages a diverse set of modern technologies to ensure high performance, maintainability, and extensibility. All services are containerized with Docker, making deployment and local development straightforward. 


## Features 🌟

*   **Modular Microservices Architecture**: Each core functionality is encapsulated within its own service, promoting independent development and deployment. 
*   **Comprehensive Quiz Management**: Full lifecycle management of quizzes and questions. 
*   **Real-time Notifications**: Inform users about quiz progress and updates via email. 
*   **Secure Identity Management**: Robust user authentication and authorization across all services. 
*   **Scalable Messaging**: Efficient inter-service communication using RabbitMQ.
*   **Containerized Deployment**: Easy setup and execution with Docker. 
*   **Clean Architecture & DDD**: Ensures maintainability, testability, and a clear separation of concerns. 

## Services 🛠️

This system comprises the following microservices:

### 1. Question Service ❓

*   **Purpose**: Manages the creation, retrieval, updating, and deletion of quiz questions and their associated answers.
*   **Key Technologies**:
    *   **gRPC**
    *   **MongoDB**

### 2. Quiz Service 🧠

*   **Purpose**: Handles the management of quizzes, including quiz creation, administration, and the core logic for answering quiz questions.
*   **Key Technologies**:
    *   **CQRS**
    *   **PostgreSQL**
    *   **gRPC**
    *   **RabbitMQ**

### 3. Notification Service 🔔

*   **Purpose**: Informs users about various stages of the quiz process, currently supporting email notifications.
*   **Key Technologies**:
    *   **RabbitMQ**
    *   **MailKit**
    *   **gRPC**

### 4. User Service 👤

*   **Purpose**: Provides identity management for all services, handling user registration, authentication, and authorization.
*   **Key Technologies**:
    *   **OpenIddict**
    *   **SQL Server**

### 5. API Gateway 🚪

*   **Purpose**: Acts as a single entry point for external clients, routing requests to the appropriate microservices and handling cross-cutting concerns like authentication and rate limiting.
*   **Key Technologies**:
    *   **YARP (Yet Another Reverse Proxy)**


## Technologies Used 💻

This project leverages a variety of technologies to build a robust and scalable microservices architecture:

*   **.NET Core**: The primary framework for building all microservices. 
*   **Docker**: For containerization of all services, ensuring easy deployment and environment consistency. 
*   **gRPC**: For high-performance inter-service communication. 
*   **RabbitMQ**: Asynchronous messaging for inter-service communication. 
*   **MongoDB**: NoSQL database for flexible data storage (Question Service). 
*   **PostgreSQL**: Relational database for structured data storage (Quiz Service). 
*   **SQL Server**: Relational database for identity management (User Service). 
*   **OpenIddict**: OpenID Connect server for secure authentication and authorization. 
*   **YARP**: High-performance reverse proxy for API Gateway. 
*   **Clean Architecture**: Architectural pattern for maintainability and testability. 
*   **Domain-Driven Design (DDD)**: Software development approach focusing on core domain logic. 
*   **Entity Framework Core**: An object-relational mapper (ORM) that enables .NET developers to work with a database using .NET objects. 


## Getting Started

To get a local copy up and running, follow these simple steps.

### Prerequisites

Ensure you have the following installed:

*   Docker and Docker Compose
*   .NET SDK (if you plan to build or modify the services)

### Installation and Running

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/beheshty/quiz-engine-microservices.git
    cd quiz-engine-microservices
    ```

2.  **Configure Environment Variables**:
    The `auth.env` file contains environment variables for authentication. Ensure it's correctly configured for your local environment.

3.  **Run with Docker Compose**:
    Navigate to the root of the cloned repository and run:
    ```bash
    docker-compose up --build
    ```
    This command will build the Docker images for all services and start them up, along with their respective databases and RabbitMQ.

4.  **Access the API Gateway**:
    Once all services are up and running, the API Gateway will be accessible at a defined port (e.g., `http://localhost:8000` or as configured in `docker-compose.yml`). You can then interact with the microservices through this gateway.

 ## 🤝 Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any bugs or feature requests.
