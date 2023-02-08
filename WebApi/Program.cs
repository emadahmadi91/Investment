var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebApiServices();

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
public partial class Program { }
