using Microsoft.EntityFrameworkCore;
using lab2_11.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Lab2Context>(options =>
    options.UseSqlServer(
        "Server=localhost;Database=lab2-11;Trusted_Connection=True;TrustServerCertificate=True;"
    )
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Create}/{id?}"
);

app.Run();