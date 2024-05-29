# User Management API

This project is a User Management API built using ASP.NET Core Web API and follows Clean Architecture principles. It provides functionality for user registration, login, profile management, and role-based authorization.

## Project Structure

The project is organized into the following layers:

- **Domain**: Contains the core entities and business rules of the application.
- **Application**: Contains the application logic, services, DTOs, and interfaces.
- **Infrastructure**: Contains the implementation details, such as repositories, database context, and external services.
- **API**: Contains the API controllers and handles HTTP requests and responses.

### Overview of the Project Structure

- TeraAuthApi.Domain: Contains the domain entities and enums.
- TeraAuthApi.Application: Contains the application services, DTOs, interfaces, and mappings.
- TeraAuthApi.Infrastructure: Contains the repository implementations, database context, and email service.
- TeraAuthApi.Api: Contains the API controllers, middleware, and configuration.



## Prerequisites

Before running the application, ensure that you have the following installed:

- .NET 8 SDK
- SQL Server (or any other supported database provider)

## Getting Started

To get started with the User Management API, follow these steps:

1. Clone the repository:
   ```sh
   git clone https://github.com/gunchar16/TeraAuthApi.git
   ```
2. Navigate to the TeraAuthApi.Api directory:
   ```sh
   cd TeraAuthApi.Api
   ```
3. Update the database connection string in the `appsettings.json` and `appsettings.Development.json` file:
   ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YourServerName;Database=YourDatabaseName;User Id=YourUsername;Password=YourPassword;"
    }
   ```
   Replace `YourServerName`, `YourDatabaseName`, `YourUsername`, and `YourPassword` with your actual database connection details. (NOTE: Currently it is using `Trusted_Connection=true;` and `TrustServerCertificate=True;` for Microsofot Authorization, you can use it too.)
4. Open the Package Manager Console in Visual Studio and navigate to the `TeraAuthApi.Infrastructure` project.
5. Run the following command to create the database and apply the migrations:
   ```sh
   Update-Database
   ```
   This will create the necessary tables and seed the database with initial data.
6. Start the application:
   ```sh
   dotnet run --project TeraAuthApi.Api
   ```

# API Endpoints
The following API endpoints are available:

- `POST /api/auth/register`: Register a new user.
- `POST /api/auth/login`: Authenticate a user and obtain a JWT token.
- `POST /api/auth/refresh-token`: Refresh an expired JWT token using a refresh token.
- `GET /api/user/profile`: Get the authenticated user's profile information.
- `PUT /api/user/profile`: Update the authenticated user's profile information.
- `POST /api/user/change-password`: Change the authenticated user's password.
- `POST /api/user/reset-password`: Reset a user's password by providing their email.
- `GET /api/admin/users`: Get all users (Admin only).
- `GET /api/admin/users/{id}`: Get a specific user's profile (Admin only).
- `PUT /api/admin/users/{id}`: Update a specific user's profile (Admin only).
- `DELETE /api/admin/users/{id}`: Delete a specific user (Admin only).
- `DELETE /api/role`: Get the list of all the roles (Admin only).
- `DELETE /api/role/assign`: Assign the role to a user (Admin only).


# Database Configuration
The application uses Entity Framework Core Code First approach for database management. The database schema is automatically created and updated based on the domain entities defined in the `TeraAuthApi.Domain` project. The database connection string is specified in the `appsettings.json` and `appsettings.Development.json` file. Make sure to update the connection string with your actual database connection details.

Before running the application, ensure that the database is created. You can create the database using SQL Server Management Studio (SSMS) or any other database management tool. The database should be named `TeraAuth` or the name specified in your connection string.

Example SQL script to create the database:

```sql
CREATE DATABASE TeraAuth;
```

To apply the database migrations and create the necessary tables, run the following command in the Package Manager Console:
```sh
Update-Database
```

This command will apply the migrations to create the required tables and seed the initial data. 

**The initial data will have admin user with these credentials: Username - admin, Password- admin**

# Technologies Used
- .NET 8, C# 12, ASP.NET Core Web API
- Entity Framework Core (Code First)
- SQL Server
- Swagger (Swashbuckle.AspNetCore)
- AutoMapper
- FluentValidation
- Serilog
