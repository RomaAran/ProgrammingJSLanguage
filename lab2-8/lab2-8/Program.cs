using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();



app.MapPost("/calculate", async (HttpContext context) =>
{
    var data = await JsonSerializer.DeserializeAsync<CalcRequest>(context.Request.Body);

    if (data == null)
        return Results.BadRequest(new { error = "Нет данных" });

    double result = 0;

    switch (data.op)
    {
        case "+": result = data.a + data.b; break;
        case "-": result = data.a - data.b; break;
        case "*": result = data.a * data.b; break;
        case "/":
            if (data.b == 0)
                return Results.BadRequest(new { error = "Деление на ноль" });
            result = data.a / data.b;
            break;
    }

    return Results.Json(new { result = result });
});

app.Run();
record CalcRequest(double a, double b, string op);