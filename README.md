# IntraLink

> **Project Description**
> Intralink is a corporate social network that lets you manage an organization
> employee directory, create personal pages, department and division pages,
> project team pages, publish information on those pages, create events (calendar
> entries), and invite employees to those events (for organizing meetings).

## 🛠 Technology Stack

*   **Backend**: .NET 8 (ASP.NET Core Web API)
*   **Database**: PostgreSQL
*   **ORM**: Entity Framework Core
*   **Frontend**: Vue.js

## 📂 Project Structure

*   `backend/Api` — Main Web API project.
*   `backend/Data` — Class library with the DB context and migrations.
*   `docs/` — Project documentation and technical specification.

## 🚀 Setup and Run (Developers)

### Prerequisites
1.  Install the **.NET 8 SDK**.
2.  Install **PostgreSQL**.
3.  Enable access to the `dotnet ef` tool (if missing):
    ```powershell
    dotnet tool install --global dotnet-ef
    ```

### 1. Configure the Database
Do not store passwords in code. Use **User Secrets** to set the connection string.

Open a terminal in `backend/Api` and run:
```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=IntraLinkDb;Username=postgres;Password=YOUR_PASSWORD"
```
*(Replace `YOUR_PASSWORD` with your local postgres user password.)*\

### 2. Create Migrations
To create the database and tables, run this from the project root(cd backend/Api):
```powershell
dotnet ef database update --project backend/Data/Data.csproj --startup-project backend/Api/Api.csproj
```

### 2.2 Work with Migration
1. Create new migration
from folder cd backend/Api:
dotnet ef migrations add <MigrationName> --project ..\Data --startup-project .
for example:
dotnet ef migrations add AddEmailToUser --project ..\Data --startup-project .

2. Apply migration to db
dotnet ef database update --project ..\Data --startup-project .

3. roll back for previous version of migration
dotnet ef database update <PreviousMigrationName> --project ..\Data --startup-project .

### 3. Run Backend
```powershell
dotnet run --project backend/Api/Api.csproj
```
The API will be available at `http://localhost:5038/swagger` (the port may differ; check the terminal output).
