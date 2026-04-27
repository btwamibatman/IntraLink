# IntraLink

> **Project Description**
> Intralink is a corporate social network that lets you manage an organization
> employee directory, create personal pages, department and division pages,
> project team pages, publish information on those pages, create events (calendar
> entries), and invite employees to those events (for organizing meetings).

## 🛠 Technology Stack

*   **server **: .NET 8 (ASP.NET Core Web API)
*   **Database**: PostgreSQL
*   **ORM**: Entity Framework Core
*   **client**: Vue.js

## 📂 Project Structure

*   `server /Api` — Main Web API project.
*   `server /Data` — Class library with the DB context and migrations.
    'client/src' - client part of project
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

Open a terminal in `server /Api` and run:
```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=IntraLinkDb;Username=postgres;Password=YOUR_PASSWORD"
```
*(Replace `YOUR_PASSWORD` with your local postgres user password.)*\

### 2. Create Migrations
To create the database and tables, run this from the project root(cd server /Api):
```powershell
dotnet ef database update --project server /Data/Data.csproj --startup-project server /Api/Api.csproj
```

### 2.2 Work with Migration
1. Create new migration
from folder cd server /Api:
dotnet ef migrations add <MigrationName> --project ..\Data --startup-project .
for example:
dotnet ef migrations add AddEmailToUser --project ..\Data --startup-project .

2. Apply migration to db
dotnet ef database update --project ..\Data --startup-project .

3. roll back for previous version of migration
dotnet ef database update <PreviousMigrationName> --project ..\Data --startup-project .

### 3. Run server 
```powershell
dotnet run --project server /Api/Api.csproj
```
## dotnet watch run --project server /Api/Api.csproj - Run of the API with the AUTORELOAD.

The API will be available at `http://localhost:5038/swagger` (the port may differ; check the terminal output).

### 3. Run client
cd "c:\Users\Hamza\OneDrive\Desktop\Notes of Hamza\Programming\Projects\IntraLink\client\intralink-client"

npm run dev

