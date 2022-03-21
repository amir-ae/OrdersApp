

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<IDbConnection, DbConnection>();
builder.Services.AddSingleton<IProviderData, ProviderData>();
builder.Services.AddSingleton<IOrderData, OrderData>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.EnsurePopulated();

app.Run();
