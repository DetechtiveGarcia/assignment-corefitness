# CoreFitness

CoreFitness is an ASP.NET Core MVC gym portal built using Clean Architecture and DDD principles.

## Features

- User authentication with ASP.NET Identity
- Profile management (update info, upload profile image)
- Membership system (choose and view memberships)
- Role-based access (Admin / Member)

## Tech Stack

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- ASP.NET Identity

## Architecture

The solution follows Clean Architecture:

- **Presentation** – MVC (Controllers + Views)
- **Application** – Business logic & services
- **Domain** – Entities & core rules
- **Infrastructure** – Database & Identity

## Getting Started

1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run migrations:
   ```bash
   dotnet ef database update
