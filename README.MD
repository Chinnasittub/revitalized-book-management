# Book Management API

Welcome to the **Book Management API**! 
This project is a Technical Assessment Question from Revitalized solutions, 
built with .NET 8.0 and powered by Entity Framework Core. It also includes JWT-based authentication for secure access.

---

## Features

- **User Management**: Add, update, delete, and retrieve users.
- **Category Management**: Add, update, delete, and retrieve categories.
- **Book Management**: Add, update, delete, and retrieve books.
- **JWT Authentication**: Secure your API with JSON Web Tokens.
- **Swagger Integration**: Explore and test the API using Swagger UI.
- **Database Support**: Works with SQL Server and SQLite.

---

## 🛠️ Tech Stack

- **.NET 8.0**: The latest and greatest from Microsoft.
- **Entity Framework Core**: For database interactions.
- **JWT Authentication**: Secure your endpoints.
- **Swagger**: API documentation and testing.

---

## 📂 Project Structure
- BookManagementPrj/ 
- ├── Controllers/ # API controllers 
- ├── Data/ # Database context
- ├── Models/ # models 
- ├── Services/ # Business logic and services 
- ├── Migrations/ # EF Core migrations 
- ├── Program.cs # Entry point of the application 
- ├── appsettings.json # Configuration settings 
- ├── BookManagemenAPI.postman_collection.json # Collection in Postman for testing API
- └── README.md # You're reading this!

## Getting Started

To get started with this project, follow these steps:

1. **Clone the repository**:
   git clone <repository-url>
   cd BookManagementPrj

2. **Install dependencies**:
   Make sure you have the .NET SDK installed. Then run:
   dotnet restore

3. **Run the application**:
   Start the application with:
   dotnet run

4. **Access the app**:
   Open your browser and go to `http://localhost:5196/` or your setting port to see the app in action!

## Testing the API
I’ve made testing easy by providing a Postman collection that you can use to test the API endpoints.

1. Steps to Test
Import the Postman Collection:

2. Open Postman.
Click on Import and upload the exported Postman collection file (`BookManagemenAPI.postman_collection.json`) provided with this project.

3. Set Up the Environment:
Go to Environments in Postman.
Create a new environment named BookManagementEnv.
Add a variable named bookManagementUrl and set its value to your application’s base URL, e.g., `http://localhost:5196`.
Run the Tests:

4. Select the BookManagementEnv environment in Postman.
Open the imported collection and run the requests to test the API endpoints.