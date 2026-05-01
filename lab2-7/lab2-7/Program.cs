var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/time", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();

    double offset = double.Parse(form["offset"], System.Globalization.CultureInfo.InvariantCulture);

    DateTime utcNow = DateTime.UtcNow;
    DateTime resultTime = utcNow.AddHours(offset);

    string html = $@"
    <html>
    <head>
        <meta charset='utf-8'>
        <title>Время</title>
    </head>
    <body>
        <h2>Текущее время</h2>
        <p>UTC{(offset >= 0 ? "+" : "")}{offset}: <b>{resultTime:HH:mm:ss}</b></p>

        <a href='/'>Назад</a>
    </body>
    </html>";

    return Results.Content(html, "text/html");
});

app.Run();