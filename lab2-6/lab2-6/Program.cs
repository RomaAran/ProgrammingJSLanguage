var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/sum", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();

    int a = int.Parse(form["a"]);
    int b = int.Parse(form["b"]);

    int sum = a + b;

    string html = $@"
    <html>
    <head>
        <meta charset='utf-8'>
        <title>Результат</title>
    </head>
    <body>
        <h2>Результат</h2>
        <p>{a} + {b} = <b>{sum}</b></p>
        <a href='/'>Назад</a>
    </body>
    </html>";

    return Results.Content(html, "text/html");
});

app.Run();