var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

string usersPath = Path.Combine(Directory.GetCurrentDirectory(), "users");

if (!Directory.Exists(usersPath))
{
    Directory.CreateDirectory(usersPath);
}

app.MapPost("/register", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();

    string login = form["login"].ToString();
    string password = form["password"].ToString();

    string filePath = Path.Combine(usersPath, login + ".txt");

    if (File.Exists(filePath))
    {
        return Results.Content(@"
            <h2>Пользователь уже существует!</h2>
            <a href='/reg.html'>Назад</a>
        ", "text/html; charset=utf-8");
    }

    File.WriteAllText(filePath, password);

    return Results.Content(@"
        <h2>Регистрация успешна!</h2>
        <a href='/login.html'>Войти</a>
    ", "text/html; charset=utf-8");
});

app.MapPost("/login", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();

    string login = form["login"].ToString();
    string password = form["password"].ToString();

    string filePath = Path.Combine(usersPath, login + ".txt");

    if (!File.Exists(filePath))
    {
        return Results.Content(@"
            <h2>Пользователь не найден!</h2>
            <a href='/login.html'>Назад</a>
        ", "text/html; charset=utf-8");
    }

    string savedPassword = File.ReadAllText(filePath);

    if (savedPassword == password)
    {
        return Results.Content($@"
            <h2>Добро пожаловать, {login}!</h2>
        ", "text/html; charset=utf-8");
    }
    else
    {
        return Results.Content(@"
            <h2>Неверный пароль!</h2>
            <a href='/login.html'>Назад</a>
        ", "text/html; charset=utf-8");
    }
});

app.Run();