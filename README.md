# TODO APP

Angular-фронт + .NET 10 REST API, EF Core і SQLite. У кожного акаунта свої категорії та задачі.

## Що потрібно

- .NET 10 SDK
- Node.js 22+ і npm

## Запуск

Перший термінал — API:

```powershell
cd "Back/TODO_API/TODO_API"
dotnet run --launch-profile http
```

Другий термінал — фронт:

```powershell
cd "Front/todo-front"
npm install
npm start
```

Відкрити http://localhost:4200. API слухає http://localhost:5255, документація — http://localhost:5255/scalar/v1.

База створюється сама при першому запуску (`Back/TODO_API/TODO_API/Data/app.db`), міграції застосовуються автоматично. JWT-налаштування для локалки вже є в `appsettings.json`.

Далі реєструєш акаунт, створюєш категорію і додаєш задачі. Задачі можна редагувати, позначати виконаними й видаляти, шукати за назвою чи описом, фільтрувати за категорією та гортати сторінки. Категорію із задачами не видалити, поки задачі не перенесені або не видалені.

## Перевірки

```powershell
dotnet build Back/TODO_API/TODO_API.slnx
```

```powershell
cd Front/todo-front
npm run build
npm test
```
