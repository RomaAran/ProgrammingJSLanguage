using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

List<Record> records = new List<Record>()
{
    new Record("Я проснулся!", DateTime.Now),
    new Record("Кушаю...", DateTime.Now),
    new Record("АЙ, МЫЛО В ГЛАЗ ПОПАЛО!!!", DateTime.Now)
};

app.MapGet("/records", () =>
{
    return Results.Json(records);
});

app.MapPost("/add", async (HttpContext context) =>
{
    var data = await JsonSerializer.DeserializeAsync<AddRequest>(context.Request.Body);

    if (data == null || string.IsNullOrWhiteSpace(data.text))
    {
        return Results.BadRequest();
    }

    records.Add(new Record(data.text, DateTime.Now));

    return Results.Json(records);
});

app.Run();

record Record(string Text, DateTime Date);

class AddRequest
{
    public string text { get; set; }
}