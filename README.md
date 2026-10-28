# To-Do application

Angular frontend and .NET 10 REST API with EF Core and SQLite. The backend uses controllers, services, interfaces and repositories. Each account has its own tasks and categories.

## Run locally

Start the API in one terminal:

```powershell
cd "Back/TODO_API/TODO_API"
dotnet run --launch-profile http
```

Start Angular in another terminal:

```powershell
cd "Front/todo-front"
npm install
npm start
```

Open `http://localhost:4200`. Angular proxies `/api` to `http://localhost:5255`. On first start, EF Core applies migrations and creates `Back/TODO_API/TODO_API/Data/app.db` if needed.

Register an account, create a category, then add tasks. You can edit, complete and delete tasks, search their names or descriptions, filter by category, and move between pages. A category with tasks cannot be deleted until its tasks are moved or removed.

## Checks

```powershell
dotnet build Back/TODO_API/TODO_API.slnx
cd Front/todo-front
npm run build
npm test
```
