# IntraLink

> **Project Description**
> IntraLink is a corporate social network that lets you manage an organization
> employee directory, create personal pages, department and division pages,
> project team pages, publish information on those pages, create events
> (calendar entries), and invite employees to those events.

## Technology Stack

* **server**: .NET 8 (ASP.NET Core Web API)
* **Database**: PostgreSQL
* **ORM**: Entity Framework Core
* **client**: Vue.js

## Project Structure

* `server/Api` - Main Web API project.
* `server/Data` - Class library with the DB context and migrations.
* `client/intralink-frontend` - Client application.
* `docs/` - Project documentation and technical specification.

## Setup and Run

### Prerequisites

1. Install the .NET 8 SDK.
2. Install PostgreSQL.
3. Enable access to the `dotnet ef` tool if it is missing:

```powershell
dotnet tool install --global dotnet-ef
```

### Configure the Database

Do not store passwords in code. Use User Secrets to set the connection string.

Open a terminal in `server/Api` and run:

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=IntraLinkDb;Username=postgres;Password=YOUR_PASSWORD"
```

Replace `YOUR_PASSWORD` with your local postgres user password.

### Work with Migrations

Run migration commands from the project root.

Create a new migration:

```powershell
dotnet ef migrations add <MigrationName> --project server/Data/Data.csproj --startup-project server/Api/Api.csproj
```

Apply migrations to the database:

```powershell
dotnet ef database update --project server/Data/Data.csproj --startup-project server/Api/Api.csproj
```

Roll back to a previous migration:

```powershell
dotnet ef database update <PreviousMigrationName> --project server/Data/Data.csproj --startup-project server/Api/Api.csproj
```

### Run Server

```powershell
dotnet run --project server/Api/Api.csproj
```

Run the API with auto reload:

```powershell
dotnet watch run --project server/Api/Api.csproj
```

The API will be available at `http://localhost:5038/swagger`. The port may differ; check the terminal output.

### Run Client

```powershell
cd client/intralink-frontend
npm run dev
```
