# BudgetPlanner8

BudgetPlanner8 is a WPF-based desktop application for budget planning, built with .NET and Entity Framework Core using SQL Server as the database. The project demonstrates a clean separation between presentation, business logic, and data access layers, following MVVM principles for a structured and scalable architecture.

This application serves as a practical example of how to build a modern desktop solution with persistent storage, database migrations, and a professional layered structure.

---

## Overview

BudgetPlanner8 allows users to:

- Create and manage budget entries
- Register income and expenses
- Categorize transactions
- Persist data using SQL Server via Entity Framework Core
- Work within a structured MVVM architecture
- Extend and build upon a clean, maintainable codebase

The project is divided into separate layers to ensure clarity, scalability, and maintainability.

---

## Tech Stack

- .NET (WPF)
- Entity Framework Core
- SQL Server
- MVVM architecture
- C#

---

## Project Structure

BudgetPlanner8/ ├── BudgetPlanner8.DAL/      # Data Access Layer (DbContext, entities, migrations) ├── BudgetPlanner8.WPF/      # Presentation Layer (Views, ViewModels) ├── BudgetPlanner8.slnx      # Solution file └── README.md

### BudgetPlanner8.DAL
Contains:
- Entity models
- DbContext
- Database configuration
- EF Core migrations

### BudgetPlanner8.WPF
Contains:
- Views (XAML)
- ViewModels
- UI logic following MVVM

---

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET SDK (version specified in the project file)
- SQL Server (LocalDB, Express, or full version)
- Entity Framework Core CLI tools (optional but recommended)

### Installation

1. Clone the repository

git clone https://github.com/antonlidstroem/BudgetPlanner8.git cd BudgetPlanner8

2. Configure the connection string

Open the configuration file (e.g., appsettings.json or the DbContext configuration) and update it with your SQL Server connection string.

3. Apply database migrations

If the project uses EF migrations:

dotnet ef database update --project .\BudgetPlanner8.DAL\

4. Run the application

Open the solution in Visual Studio and set `BudgetPlanner8.WPF` as the startup project. Build and run the application.

---

## Architecture and Design Principles

The project follows these principles:

- Separation of Concerns
- MVVM (Model-View-ViewModel)
- Clear separation between UI and data access
- Entity Framework as ORM
- Scalable and maintainable structure

This makes the project suitable as:

- A learning project
- A reference implementation for WPF + EF Core
- A foundation for further product development

---

## Future Improvements

Possible enhancements:

- Implement a repository pattern on top of EF
- Add validation using Data Annotations or FluentValidation
- Create unit tests for business logic
- Implement export functionality (PDF/CSV)
- Add charts and data visualization
- Set up CI/CD with GitHub Actions

---

## Contributing

Contributions, improvements, and pull requests are welcome.  
Feel free to open an issue if you find a bug or have suggestions.

---

## License

This project is intended for educational and demonstration purposes.  
Add a specific license if the project will be used in production or distributed publicly.