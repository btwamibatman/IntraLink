# IntraLink

> **Описание проекта**
> Intralink - корпоративная социальную сеть, с возможностью
ведения списка сотрудников организации, создания личных страниц, страниц отделов
и департаментов, страниц проектных команд, публикации информации на созданных
страницах, создания мероприятий (событий в календаре) и приглашения на эти
мероприятия сотрудников (для организации совещаний)


## 🛠 Технологический стек

*   **Backend**: .NET 8 (ASP.NET Core Web API)
*   **Database**: PostgreSQL
*   **ORM**: Entity Framework Core
*   **Frontend**: Vue.js

## 📂 Структура проекта

*   `backend/Api` — Основной проект WEB API.
*   `backend/Data` — Библиотека классов с контекстом БД и миграциями.
*   `docs/` — Документация и ТЗ проекта.

## 🚀 Установка и запуск (для разработчиков)

### Предварительные требования
1.  Установите **.NET 8 SDK**.
2.  Установите **PostgreSQL**.
3.  Настройте доступ к команде `dotnet ef` (если нет):
    ```powershell
    dotnet tool install --global dotnet-ef
    ```

### 1. Настройка базы данных
Мы не храним пароли в коде. Используйте **User Secrets** для настройки подключения.

Откройте терминал в папке `backend/Api` и выполните:
```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=IntraLinkDb;Username=postgres;Password=ВАШ_ПАРОЛЬ"
```
*(Замените `ВАШ_ПАРОЛЬ` на пароль от пользователя postgres на вашем компьютере).*

### 2. Применение миграций
Чтобы создать базу данных и таблицы, выполните команду из корня проекта:
```powershell
dotnet ef database update --project backend/Data/Data.csproj --startup-project backend/Api/Api.csproj
```

### 3. Запуск Backend
```powershell
dotnet run --project backend/Api/Api.csproj
```
API будет доступно по адресу: `http://localhost:5038/swagger` (порт может отличаться, смотрите вывод в терминале).
