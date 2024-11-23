## Expense Tracker App 📊
### Overview
The Expense Tracker App is a user-friendly application designed to help users track their daily transactions, manage budgets, and maintain financial discipline. Users can add transactions, categorize expenses, set monthly budgets, and receive email alerts when spending exceeds the predefined limits.

### Features 🌟  
| Feature              | Description                                      |
|----------------------|--------------------------------------------------|
| Add Transactions     | Log daily expenses with categories and details. |
| Set Budgets          | Define monthly spending limits per category.    |
| Email Notifications  | Receive alerts when exceeding budget limits.    |

### Getting Started  
Follow these steps to set up the app locally.

### Prerequisites  
- .NET 6.0+  
- SQL Server  
- SMTP credentials  

### Restore Dependencies
Restore NuGet packages required by the application:

dotnet restore

### Configure the Database
Set up the database connection string in appsettings.json:

"ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionStringHere"
}