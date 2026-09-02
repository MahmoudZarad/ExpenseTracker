# 💸 ExpenseTracker

<div align="center">

### Personal Finance Management Platform

A full-stack personal finance application for managing income, expenses, categories, budgets, and financial insights.

Built with **Angular**, **ASP.NET Core**, **Entity Framework Core**, **SQL Server**, and **Docker**.

<br>

🌐 **Live Application**  
https://expensetracker.hoodaabdo665.workers.dev

📚 **API / Swagger**  
https://expensetracker3.runasp.net/swagger/index.html

</div>

---

## ✨ Features

- 🔐 User Registration & Login
- 🎟️ JWT Authentication & Authorization
- Abuse protection (rate limiting) & Hashing
- 📊 Financial Dashboard
- 💳 Income & Expense Transactions
- 🗂️ Custom Categories
- 💰 Budget Management
- ⚙️ User Settings & Preferences
- 👤 User-specific financial data
- 📈 Financial charts and statistics
- 🐳 Full Docker Compose environment
- 🗄️ SQL Server database with EF Core migrations

---

## 🛠️ Technologies

### Frontend

- Angular
- TypeScript
- HTML
- CSS
- JavaScript 
- Tailwind CSS
- ECharts

### Backend

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- BCrypt
- AutoMapper
- MediatR
- FluentValidation
- MailKit
- Hangfire
- Swagger / OpenAPI

### Architecture & Design

- Clean Architecture
- CQRS with MediatR
- Repository Pattern
- Unit of Work
- DTOs
- Dependency Injection
- Middleware
- Feature-based organization

### DevOps & Deployment

- Docker
- Docker Compose
- Nginx
- Git & GitHub
- Cloudflare Workers
---

## 🏗️ Architecture

The backend follows a Clean Architecture approach:

```text
ExpenseTracker
│
├── ExpenseTracker.Api
├── ExpenseTracker.Application
├── ExpenseTracker.Domain
├── ExpenseTracker.Infrastructure
│
└── expense-tracker
```

### Backend Layers

```text
API
 ↓
Application
 ↓
Domain
 ↑
Infrastructure
 ↓
SQL Server
```

- **Domain** → Core entities and business concepts
- **Application** → Application logic, features, DTOs, and interfaces
- **Infrastructure** → EF Core, repositories, Unit of Work, and database access
- **API** → Controllers, authentication, middleware, Swagger, and HTTP configuration

---

# 🐳 Docker

The project includes a complete Docker Compose setup containing:

```text
┌─────────────────────┐
│ Angular + Nginx     │
│      Frontend       │
│       :4200         │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│   ASP.NET Core API  │
│       :8080         │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│     SQL Server      │
│       :1433         │
└─────────────────────┘
```

### Run with Docker

From the project root:

```bash
docker compose up --build
```

Or run in the background:

```bash
docker compose up -d --build
```

Check the containers:

```bash
docker compose ps
```

Stop the application:

```bash
docker compose down
```

### Local Docker URLs

Frontend:

```text
http://localhost:4200
```

API / Swagger:

```text
http://localhost:8080/swagger
```

---

# 🚀 Local Development

## Prerequisites

Make sure you have:

- .NET SDK
- Node.js
- Angular CLI
- SQL Server
- Git

## Backend

Navigate to the API project and run:

```bash
dotnet restore
dotnet run
```

## Frontend

Navigate to the Angular project:

```bash
npm install
ng serve
```

The frontend will be available at:

```text
http://localhost:4200
```

---

## ⚙️ Configuration

The application uses ASP.NET Core configuration and environment variables.

Sensitive values such as:

- Database connection strings
- JWT signing keys
- Passwords
- Other secrets

should not be committed to the repository.

For Docker, environment variables can override application configuration.

Example:

```yaml
environment:
  ConnectionStrings__DefaultConnection: "YOUR_CONNECTION_STRING"
```

---

## 🌍 Deployment

The project is deployed as separate frontend and backend applications.

### Frontend

Cloudflare Workers:

https://expensetracker.hoodaabdo665.workers.dev

### Backend

ASP.NET hosting:

https://expensetracker3.runasp.net/swagger/index.html

Docker configuration is also included for running the complete application locally as a multi-container environment.

---

## 📚 API Documentation

Interactive API documentation is available through Swagger:

https://expensetracker3.runasp.net/swagger/index.html

---

## 👨‍💻 Author

**Mahmoud Abdo**

Faculty of Computers and Information science & Full-Stack Developer

---

<div align="center">

⭐ If you find this project useful, consider giving it a star.

</div>
