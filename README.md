# Task Management API

A comprehensive task management system built with ASP.NET Core, featuring user authentication, role-based permissions, task categorization, comments, reactions, and audit logging.

## 🚀 Features

- **User Management**: Registration, login, and user profiles
- **Task Management**: Create, read, update, delete tasks with priorities and statuses
- **Categories**: Organize tasks into categories
- **Comments & Reactions**: Collaborate on tasks with comments and emoji reactions
- **Role-Based Access Control**: Permissions system with roles and granular access control
- **JWT Authentication**: Secure token-based authentication with refresh tokens
- **Rate Limiting**: Built-in rate limiting to prevent abuse
- **Audit Logging**: Track all system activities
- **Swagger Documentation**: Interactive API documentation
- **Docker Support**: Containerized deployment

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 10.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **Validation**: FluentValidation
- **Documentation**: Swagger/OpenAPI
- **Rate Limiting**: ASP.NET Core Rate Limiting
- **Containerization**: Docker

## 📋 Prerequisites

- .NET 10.0 SDK
- SQL Server (LocalDB or full instance)
- Docker (optional, for containerized deployment)

## 🔧 Installation & Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd TaskManagement
```

### 2. Database Setup

The application uses SQL Server. Update the connection string in `TaskManagement.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskmanagementDB;User Id=sa;Password=sa123456;TrustServerCertificate=True;"
  }
}
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Run Database Migrations

```bash
cd TaskManagement.API
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

The API will be available at:

- HTTP: http://localhost:5037
- HTTPS: https://localhost:7055
- Swagger UI: https://localhost:7055/swagger

## 🐳 Docker Deployment

### Build and Run with Docker

```bash
docker build -t taskmanagement-api .
docker run -p 8080:80 -p 8081:443 taskmanagement-api
```

### Using Docker Compose

```bash
docker-compose up --build
```

## 📚 API Endpoints

### Authentication

- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh access token

### Tasks

- `GET /api/tasks` - Get all tasks for current user
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task
- `GET /api/tasks/paged` - Get paginated tasks

### Categories

- `GET /api/categories` - Get all categories
- `POST /api/categories` - Create category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

### Comments

- `GET /api/comments/task/{taskId}` - Get comments for task
- `POST /api/comments` - Add comment
- `PUT /api/comments/{id}` - Update comment
- `DELETE /api/comments/{id}` - Delete comment

### Reactions

- `GET /api/reactions/task/{taskId}` - Get reactions for task
- `POST /api/reactions` - Add reaction
- `DELETE /api/reactions/{id}` - Remove reaction

### Users

- `GET /api/users` - Get all users (admin only)
- `GET /api/users/{id}` - Get user by ID
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Roles

- `GET /api/roles` - Get all roles
- `POST /api/roles` - Create role
- `PUT /api/roles/{id}` - Update role
- `DELETE /api/roles/{id}` - Delete role

## 🔐 Authentication

The API uses JWT (JSON Web Tokens) for authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

## 📊 Rate Limiting

The API implements different rate limiting policies:

- **Auth**: 5 requests per minute (login/register/refresh)
- **Normal**: 30 requests per minute (CRUD operations)
- **Read**: 100 requests per minute (read operations)

## 🏗️ Project Structure

```
TaskManagement/
├── TaskManagement.API/           # Web API project
│   ├── Controllers/              # API controllers
│   ├── Middleware/               # Custom middleware
│   ├── Properties/               # Launch settings
│   └── appsettings.json          # Configuration
├── TaskManagement.Application/   # Application layer
│   ├── DTOs/                     # Data transfer objects
│   ├── Interfaces/               # Service interfaces
│   ├── Services/                 # Business logic
│   ├── Validators/               # Input validation
│   └── Security/                 # Security services
├── TaskManagement.Domain/        # Domain entities
│   └── Entities/                 # Domain models
├── TaskManagement.Infrastructure/ # Infrastructure layer
│   ├── Configurations/           # Entity configurations
│   ├── Data/                     # Database context & seeding
│   └── Migrations/               # EF migrations
└── TaskManagement.ConsoleTest/   # Console test project
```

## 🧪 Testing

Run the console test project to verify basic functionality:

```bash
cd TaskManagement.ConsoleTest
dotnet run
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📞 Support

For support, email support@taskmanagement.com or create an issue in the repository.</content>
<parameter name="filePath">a:\TaskManagement\README.md
