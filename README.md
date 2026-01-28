# 🛒 DemoECommerce - Microservices E-Commerce Platform

A modern, scalable e-commerce platform built with **.NET 8** using **microservices architecture** and **API Gateway pattern**. This project demonstrates clean architecture principles, JWT authentication, rate limiting, resilience patterns, and inter-service communication.

## 📋 Table of Contents

- [About](#about)
- [Architecture](#architecture)
- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Features](#features)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Security](#security)
- [Contributing](#contributing)

## 📖 About

DemoECommerce is a demonstration project showcasing how to build a production-ready e-commerce application using microservices architecture. The system is designed with separation of concerns, scalability, and maintainability in mind.

### Key Highlights

- **Microservices Architecture**: Independent, loosely-coupled services
- **API Gateway Pattern**: Centralized entry point using Ocelot
- **Clean Architecture**: Domain-driven design with clear separation of layers
- **Security First**: JWT authentication, rate limiting, and request signature validation
- **Resilience**: Retry policies and circuit breakers using Polly
- **Response Caching**: Improved performance with file-based caching
- **Structured Logging**: Comprehensive logging with Serilog

## 🏗️ Architecture

### System Overview

```
┌─────────────┐
│   Clients   │
│ (Postman,   │
│  Browser)   │
└──────┬──────┘
       │ HTTPS
       ▼
┌─────────────────────────────────────┐
│      API Gateway (Port 5003)        │
│  ┌─────────────────────────────┐   │
│  │  Ocelot Gateway             │   │
│  │  - Rate Limiting            │   │
│  │  - Request Routing          │   │
│  │  - Response Caching         │   │
│  │  - JWT Validation           │   │
│  │  - Request Signature        │   │
│  └─────────────────────────────┘   │
└──────┬──────────┬──────────┬────────┘
       │ HTTP     │ HTTP     │ HTTP
       ▼          ▼          ▼
┌─────────┐ ┌─────────┐ ┌─────────┐
│  Auth   │ │Product  │ │ Order   │
│   API   │ │  API    │ │  API    │
│ (5000)  │ │ (5001)  │ │ (5002)  │
└────┬────┘ └────┬────┘ └────┬────┘
     │           │           │
     ▼           ▼           ▼
┌─────────────────────────────────┐
│      SQL Server LocalDB         │
│        (ECommerceDb)            │
└─────────────────────────────────┘
```

### Microservices

#### 1. **Authentication API** (Port 5000)
- User registration and login
- JWT token generation
- User profile management
- Role-based access control

#### 2. **Product API** (Port 5001)
- Product catalog management
- CRUD operations on products
- Admin-only product modifications
- Public product browsing

#### 3. **Order API** (Port 5002)
- Order creation and management
- Order history retrieval
- Integration with Product and Auth APIs
- Client-specific order queries

#### 4. **API Gateway** (Port 5003)
- Single entry point for all clients
- Request routing to backend services
- Rate limiting per IP address
- Response caching
- JWT authentication
- Request signature validation

### Clean Architecture Layers

Each microservice follows **Clean Architecture** principles:

```
┌─────────────────────────────────────┐
│     Presentation Layer              │
│  (Controllers, Program.cs)          │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│     Application Layer               │
│  (DTOs, Interfaces, Services)       │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│     Infrastructure Layer            │
│  (DbContext, Repositories, DI)      │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│     Domain Layer                    │
│  (Entities, Business Logic)         │
└─────────────────────────────────────┘
```

## 🛠️ Technologies

### Core Technologies
- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API development
- **Entity Framework Core 8.0** - ORM for database operations
- **SQL Server LocalDB** - Database engine

### API Gateway & Middleware
- **Ocelot** - API Gateway for routing and orchestration
- **CacheManager** - Response caching

### Security
- **JWT Bearer Authentication** - Token-based authentication
- **BCrypt.Net** - Password hashing
- **Custom Middleware** - Request signature validation

### Resilience & Reliability
- **Polly** - Retry policies and circuit breakers
- **HttpClient Factory** - Managed HTTP connections

### Logging & Monitoring
- **Serilog** - Structured logging
- **Custom Exception Logging** - Error tracking

### Development Tools
- **Swagger/OpenAPI** - API documentation
- **Visual Studio 2022** - IDE

## 📁 Project Structure

```
DemoECommerce/
├── DemoECommerce.AuthenticationApiSolution/
│   ├── AuthenticationApi.Domain/          # Entities (AppUser)
│   ├── AuthenticationApi.Application/     # DTOs, Interfaces
│   ├── AuthenticationApi.Infrastructure/  # DbContext, Repositories, DI
│   └── AuthenticationApi.Presentation/    # Controllers, Program.cs
│
├── DemoECommerce.ProductApiSolution/
│   ├── ProductApi.Domain/                 # Entities (Product)
│   ├── ProductApi.Application/            # DTOs, Interfaces
│   ├── ProductApi.Infrastructure/         # DbContext, Repositories, DI
│   └── ProductApi.Presentation/           # Controllers, Program.cs
│
├── DemoECommerce.OrderApiSolution/
│   ├── OrderApi.Domain/                   # Entities (Order)
│   ├── OrderApi.Application/              # DTOs, Interfaces, Services
│   ├── OrderApi.Infrastructure/           # DbContext, Repositories, DI
│   └── OrderApi.Presentation/             # Controllers, Program.cs
│
├── DemoECommerce.ApiGatewaySolution/
│   └── ApiGateway.Presentation/
│       ├── Middleware/                    # Custom middleware
│       ├── ocelot.json                    # Gateway configuration
│       └── Program.cs
│
└── DemoECommerce.SharedLibrarySolution/
    └── ECommerce.SharedLibrary/           # Shared utilities, DI, logging
```

## ✨ Features

### 🔐 Authentication & Authorization
- User registration with email validation
- Secure login with JWT token generation
- Role-based authorization (Admin, User)
- Token expiration and validation
- Password hashing with BCrypt

### 🛍️ Product Management
- Browse all products (public)
- View product details
- Create, update, delete products (Admin only)
- Product inventory tracking

### 📦 Order Management
- Create new orders
- View all orders (authenticated users)
- Get order by ID
- View orders by client ID
- Get detailed order information (product + user details)
- Update and delete orders

### 🚪 API Gateway Features
- **Request Routing**: Intelligent routing to backend services
- **Rate Limiting**: IP-based rate limiting (configurable per route)
- **Response Caching**: File-based caching for GET requests
- **Request Signature**: Validates requests came through gateway
- **JWT Validation**: Centralized authentication
- **CORS Support**: Cross-origin request handling
- **HTTPS Enforcement**: Secure communication

### 🔄 Resilience Patterns
- **Retry Strategy**: Automatic retry on transient failures
- **Circuit Breaker**: Prevents cascading failures
- **Timeout Policies**: Request timeout management
- **Exponential Backoff**: Smart retry delays

### 📝 Logging & Monitoring
- Structured logging with Serilog
- Exception logging to file and console
- Request/response logging
- Performance monitoring

## 🚀 Getting Started

### Prerequisites

- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** or **VS Code**
- **SQL Server LocalDB** (included with Visual Studio)
- **Postman** (for API testing) - [Download](https://www.postman.com/downloads/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/DemoECommerce.git
   cd DemoECommerce
   ```

2. **Update Database Connection Strings**
   
   Verify connection strings in `appsettings.json` for each API:
   ```json
   "ConnectionStrings": {
     "eCommerceConnection": "Server=(localdb)\\MSSQLLocalDB; Database=ECommerceDb; Trusted_Connection=true; TrustServerCertificate=true;"
   }
   ```

3. **Run Database Migrations**

   **Authentication API:**
   ```bash
   cd DemoECommerce.AuthenticationApiSolution\AuthenticationApi.Infrastructure
   dotnet ef database update
   ```

   **Product API:**
   ```bash
   cd DemoECommerce.ProductApiSolution\ProductApi.Infrastructure
   dotnet ef database update
   ```

   **Order API:**
   ```bash
   cd DemoECommerce.OrderApiSolution\OrderApi.Infrastructure
   dotnet ef database update
   ```

4. **Configure JWT Settings**

   Ensure all APIs and Gateway have matching JWT configuration in `appsettings.json`:
   ```json
   "Authentication": {
     "Key": "your-super-secret-key-at-least-32-characters-long",
     "Issuer": "http://localhost:5000",
     "Audience": "http://localhost:5000"
   }
   ```

5. **Run the Services**

   Open **4 terminal windows** and run each service:

   **Terminal 1 - Authentication API:**
   ```bash
   cd DemoECommerce.AuthenticationApiSolution\AuthenticationApi.Presentation
   dotnet run
   ```
   ✅ Running on `http://localhost:5000`

   **Terminal 2 - Product API:**
   ```bash
   cd DemoECommerce.ProductApiSolution\ProductApi.Presentation
   dotnet run
   ```
   ✅ Running on `http://localhost:5001`

   **Terminal 3 - Order API:**
   ```bash
   cd DemoECommerce.OrderApiSolution\OrderApi.Presentation
   dotnet run
   ```
   ✅ Running on `http://localhost:5002`

   **Terminal 4 - API Gateway:**
   ```bash
   cd DemoECommerce.ApiGatewaySolution\ApiGateway.Presentation
   dotnet run
   ```
   ✅ Running on `https://localhost:5003`

6. **Verify Setup**

   Access Swagger documentation:
   - Authentication API: `http://localhost:5000/swagger`
   - Product API: `http://localhost:5001/swagger`
   - Order API: `http://localhost:5002/swagger`

   **Note:** All client requests should go through the API Gateway at `https://localhost:5003`

## 📍 API Endpoints

### 🔐 Authentication API (via Gateway)

#### Register User
```http
POST https://localhost:5003/api/authentication/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "SecurePass123!",
  "telephoneNumber": "+1234567890",
  "address": "123 Main St, City",
  "role": "User"
}
```

#### Login
```http
POST https://localhost:5003/api/authentication/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "SecurePass123!"
}
```

**Response:**
```json
{
  "flag": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Get User by ID
```http
GET https://localhost:5003/api/authentication/{userId}
Authorization: Bearer {token}
```

### 🛍️ Product API (via Gateway)

#### Get All Products (Public)
```http
GET https://localhost:5003/api/products
```

#### Get Product by ID (Public)
```http
GET https://localhost:5003/api/products/{id}
```

#### Create Product (Admin Only)
```http
POST https://localhost:5003/api/products
Authorization: Bearer {admin-token}
Content-Type: application/json

{
  "name": "Laptop",
  "price": 999.99,
  "quantity": 50
}
```

#### Update Product (Admin Only)
```http
PUT https://localhost:5003/api/products
Authorization: Bearer {admin-token}
Content-Type: application/json

{
  "id": 1,
  "name": "Gaming Laptop",
  "price": 1299.99,
  "quantity": 30
}
```

#### Delete Product (Admin Only)
```http
DELETE https://localhost:5003/api/products
Authorization: Bearer {admin-token}
Content-Type: application/json

{
  "id": 1
}
```

### 📦 Order API (via Gateway)

#### Get All Orders (Authenticated)
```http
GET https://localhost:5003/api/orders
Authorization: Bearer {token}
```

#### Get Order by ID (Authenticated)
```http
GET https://localhost:5003/api/orders/{id}
Authorization: Bearer {token}
```

#### Get Orders by Client ID (Authenticated)
```http
GET https://localhost:5003/api/orders/client/{clientId}
Authorization: Bearer {token}
```

#### Get Order Details (Authenticated)
```http
GET https://localhost:5003/api/orders/details/{orderId}
Authorization: Bearer {token}
```

#### Create Order (Authenticated)
```http
POST https://localhost:5003/api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "productId": 1,
  "clientId": 123,
  "quantity": 2,
  "orderDate": "2026-01-28T10:00:00"
}
```

#### Update Order (Authenticated)
```http
PUT https://localhost:5003/api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": 1,
  "productId": 1,
  "clientId": 123,
  "quantity": 5,
  "orderDate": "2026-01-28T10:00:00"
}
```

#### Delete Order (Authenticated)
```http
DELETE https://localhost:5003/api/orders/{id}
Authorization: Bearer {token}
```

## ⚙️ Configuration

### API Gateway (ocelot.json)

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/authentication/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5000
        }
      ],
      "UpstreamPathTemplate": "/api/authentication/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST" ],
      "RateLimitOptions": {
        "EnableRateLimiting": true,
        "Period": "1m",
        "Limit": 20
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:5003"
  }
}
```

### Rate Limiting

**Authentication API**: 20 requests per minute per IP
**Order API (GET)**: 100 requests per minute per IP
**Order API (POST/PUT/DELETE)**: 50 requests per minute per IP
**Product API**: No rate limiting (cached responses)

### Caching

**Product API (GET)**: 60 seconds cache TTL
**Order API (GET)**: 60 seconds cache TTL

## 🔒 Security

### Security Features

1. **JWT Authentication**
   - Token-based authentication
   - Configurable expiration
   - Role-based authorization

2. **Password Security**
   - BCrypt hashing
   - Salt generation

3. **Request Validation**
   - API Gateway signature validation
   - Model validation with Data Annotations

4. **Rate Limiting**
   - IP-based rate limiting
   - Configurable per route

5. **HTTPS Enforcement**
   - API Gateway uses HTTPS
   - SSL termination at gateway

6. **CORS Configuration**
   - Configurable CORS policies
   - Whitelisting support

### Security Best Practices

#### For Production Deployment:

1. **Network Isolation**
   - Keep backend APIs (5000, 5001, 5002) on private network
   - Only expose API Gateway (5003) publicly
   - Use firewall rules to block direct access to backend APIs

2. **Enhanced Authentication**
   - Use HMAC signatures instead of simple headers
   - Implement refresh tokens
   - Add token revocation

3. **Secrets Management**
   - Store JWT keys in environment variables
   - Use Azure Key Vault or similar
   - Never commit secrets to source control

4. **SSL/TLS**
   - Use valid SSL certificates
   - Enable HTTPS on all services
   - Enforce TLS 1.2+

## 🧪 Testing with Postman

### Step-by-Step Testing Guide

1. **Register a User**
   ```
   POST https://localhost:5003/api/authentication/register
   ```

2. **Login and Get Token**
   ```
   POST https://localhost:5003/api/authentication/login
   ```
   Copy the `token` from response

3. **Test Authenticated Endpoints**
   - Go to **Authorization** tab in Postman
   - Select **Bearer Token**
   - Paste your token
   - Make requests to protected endpoints

### Sample Postman Collection

Import this collection to get started quickly:

```json
{
  "info": {
    "name": "DemoECommerce API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Authentication",
      "item": [
        {
          "name": "Register",
          "request": {
            "method": "POST",
            "url": "https://localhost:5003/api/authentication/register",
            "body": {
              "mode": "raw",
              "raw": "{\n  \"name\": \"Test User\",\n  \"email\": \"test@example.com\",\n  \"password\": \"Test@123\",\n  \"telephoneNumber\": \"+1234567890\",\n  \"address\": \"123 Test St\",\n  \"role\": \"User\"\n}"
            }
          }
        }
      ]
    }
  ]
}
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Your Name**
- GitHub: [@yourusername](https://github.com/yourusername)

## 🙏 Acknowledgments

- .NET Team for the excellent framework
- Ocelot team for the API Gateway
- Polly team for resilience patterns
- Clean Architecture principles by Robert C. Martin

## 📞 Support

If you have any questions or issues, please open an issue on GitHub or contact the maintainer.

---

**⭐ If you find this project helpful, please give it a star!**
