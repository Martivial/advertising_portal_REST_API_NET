
using Projekt_ASP.Services;

var builder = WebApplication.CreateBuilder(args);

// Obsługa widoków MVC.
builder.Services.AddControllersWithViews();

// Sesja użytkownika.
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Klient HTTP do komunikacji z API.
builder.Services.AddHttpClient("AdvertisingApi", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]
        ?? "http://localhost:5051");
});

// Serwis obsługujący ogłoszenia przez API.
builder.Services.AddScoped<AdsApiClient>();

var app = builder.Build();

// Obsługa błędów.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// Domyślna trasa, np. /Home/Index, /Ads/Create.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();