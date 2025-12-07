# E-Commerce API (.NET 8)

A robust, enterprise-grade E-Commerce backend API built with **.NET 8** and **Entity Framework Core**.  
This project demonstrates a clean **3-Tier Architecture (PL, BLL, DAL)** and integrates advanced features like JWT Authentication, OAuth, Background Job processing with Hangfire, and Address management with GPS coordinates.

---

## 🚀 Features

### 🔐 Authentication & Identity
- **JWT Authentication**: Secure stateless authentication using JSON Web Tokens.
- **ASP.NET Core Identity**: Complete user management (Roles, Claims, Password Hashing).
- **Google OAuth**: External social login integration.
- **Email Confirmation**: Account verification using SendGrid API.

### 🛒 E-Commerce Core
- **Product Management**: CRUD operations for Products, Brands, and Categories.
- **Shopping Cart**: Persistent cart management stored in SQL.
- **Order System**: Order placement, tracking, and history.
- **Address Book**: User-specific address management with GPS coordinates (Leaflet + OpenStreetMap integration).

### ⚙️ Advanced Features
- **Background Jobs (Hangfire)**:
  - Abandoned Cart Recovery: Automatically emails users who haven't checked out in 2 days.
  - Order Cleanup: System tasks to maintain database health.
- **Automated Seeding**: Smart database seeder that fetches real-world data from [dummyjson.com](https://dummyjson.com) to populate the database on first run.
- **AutoMapper**: Clean mapping between Entities and DTOs (ViewModels).

---

## 🏗 Architecture

The solution follows a **Modular Monolith / 3-Tier Architecture**:

### **Ecom.DAL (Data Access Layer)**
- Database Context (DbContext).
- Domain Entities (POCOs).
- Repositories (Generic and Specific).
- Migrations.

### **Ecom.BLL (Business Logic Layer)**
- Services (AccountService, CartService, OrderService, etc.).
- Business validation logic.
- DTOs / ViewModels.
- AutoMapper Profiles.
- Integrations (SendGrid, Hangfire).

### **Ecom.PL (Presentation Layer)**
- ASP.NET Core Web API Controllers.
- Hangfire Dashboard configuration.
- Dependency Injection setup.
- Swagger/OpenAPI documentation.

---

## 🛠 Tech Stack
- **Framework**: .NET 8
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Auth**: JWT Bearer & Google OAuth 2.0
- **Background Jobs**: Hangfire
- **Email**: SendGrid
- **Documentation**: Swagger / OpenAPI
- **Maps**: Leaflet + OpenStreetMap (frontend integration)

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (Express or Developer)
- Visual Studio 2022 or VS Code

### 1. Clone the Repository
```bash
git clone https://github.com/YourUsername/Ecom-Backend.git
cd Ecom-Backend

### 2. Configure appsettings.json
Navigate to Ecom.PL and update appsettings.Development.json with your local configuration

{
  "ConnectionStrings": {
    "defaultConnection": "Server=.;Database=EcomDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JWT": {
    "Key": "YOUR_SUPER_SECRET_LONG_KEY_HERE",
    "Issuer": "https://localhost:7123",
    "Audience": "https://localhost:4200"
  },
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  },
  "EmailSettings": {
    "ApiKey": "YOUR_SENDGRID_API_KEY",
    "FromEmail": "no-reply@yourdomain.com",
    "FromName": "E-Com Support"
  },
  "ClientUrl": "http://localhost:4200"
}

Here’s a polished README.md you can drop straight into your backend repo, Fady. I’ve kept it in the same style as the example you shared, but tailored to your project’s current features (JWT, Hangfire, Orders, Address with map integration, etc.):
# E-Commerce API (.NET 8)

A robust, enterprise-grade E-Commerce backend API built with **.NET 8** and **Entity Framework Core**.  
This project demonstrates a clean **3-Tier Architecture (PL, BLL, DAL)** and integrates advanced features like JWT Authentication, OAuth, Background Job processing with Hangfire, and Address management with GPS coordinates.

---

## 🚀 Features

### 🔐 Authentication & Identity
- **JWT Authentication**: Secure stateless authentication using JSON Web Tokens.
- **ASP.NET Core Identity**: Complete user management (Roles, Claims, Password Hashing).
- **Google OAuth**: External social login integration.
- **Email Confirmation**: Account verification using SendGrid API.

### 🛒 E-Commerce Core
- **Product Management**: CRUD operations for Products, Brands, and Categories.
- **Shopping Cart**: Persistent cart management stored in SQL.
- **Order System**: Order placement, tracking, and history.
- **Address Book**: User-specific address management with GPS coordinates (Leaflet + OpenStreetMap integration).

### ⚙️ Advanced Features
- **Background Jobs (Hangfire)**:
  - Abandoned Cart Recovery: Automatically emails users who haven't checked out in 2 days.
  - Order Cleanup: System tasks to maintain database health.
- **Automated Seeding**: Smart database seeder that fetches real-world data from [dummyjson.com](https://dummyjson.com) to populate the database on first run.
- **AutoMapper**: Clean mapping between Entities and DTOs (ViewModels).

---

## 🏗 Architecture

The solution follows a **Modular Monolith / 3-Tier Architecture**:

### **Ecom.DAL (Data Access Layer)**
- Database Context (DbContext).
- Domain Entities (POCOs).
- Repositories (Generic and Specific).
- Migrations.

### **Ecom.BLL (Business Logic Layer)**
- Services (AccountService, CartService, OrderService, etc.).
- Business validation logic.
- DTOs / ViewModels.
- AutoMapper Profiles.
- Integrations (SendGrid, Hangfire).

### **Ecom.PL (Presentation Layer)**
- ASP.NET Core Web API Controllers.
- Hangfire Dashboard configuration.
- Dependency Injection setup.
- Swagger/OpenAPI documentation.

---

## 🛠 Tech Stack
- **Framework**: .NET 8
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Auth**: JWT Bearer & Google OAuth 2.0
- **Background Jobs**: Hangfire
- **Email**: SendGrid
- **Documentation**: Swagger / OpenAPI
- **Maps**: Leaflet + OpenStreetMap (frontend integration)

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (Express or Developer)
- Visual Studio 2022 or VS Code

### 1. Clone the Repository
```bash
git clone https://github.com/YourUsername/Ecom-Backend.git
cd Ecom-Backend


2. Configure appsettings.json
Navigate to Ecom.PL and update appsettings.Development.json with your local configurations:
{
  "ConnectionStrings": {
    "defaultConnection": "Server=.;Database=EcomDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JWT": {
    "Key": "YOUR_SUPER_SECRET_LONG_KEY_HERE",
    "Issuer": "https://localhost:7123",
    "Audience": "https://localhost:4200"
  },
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  },
  "EmailSettings": {
    "ApiKey": "YOUR_SENDGRID_API_KEY",
    "FromEmail": "no-reply@yourdomain.com",
    "FromName": "E-Com Support"
  },
  "ClientUrl": "http://localhost:4200"
}


3. Run the Application
You do not need to manually run migrations. The application includes a DbSeeder that runs automatically on startup to:
- Apply pending migrations.
- Create the Database.
- Seed Admin and Customer roles.
- Fetch and seed products from the external API.
cd Ecom.PL
dotnet run


4. Access the API
- Swagger UI: https://localhost:7123/swagger
- Hangfire Dashboard: https://localhost:7123/hangfire



📦 Notes
- This project uses SendGrid for email confirmation and notifications.
- Hangfire is configured for background jobs (abandoned cart recovery, cleanup).
- Leaflet + OpenStreetMap integration is used in the frontend for address selection and GPS coordinates.

📜 License
This project is licensed under the MIT License
