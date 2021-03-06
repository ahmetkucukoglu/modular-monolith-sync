using Ecommerce.Bootstrapper;
using Ecommerce.Inventory;
using Ecommerce.Order;
using Ecommerce.Payment;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddInventory()
    .AddOrder()
    .AddPayment()
    .AddBootstrapper();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.MapGet("/", () => "Welcome to E-commerce Modular Monolith App.");

app.Run();