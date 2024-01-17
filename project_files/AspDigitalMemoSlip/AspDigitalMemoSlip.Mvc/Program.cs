using AspDigitalMemoSlip.Mvc.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Add IHttpContextAccessor
builder.Services.AddTransient(s => new AuthenticationHttpHandler(s.GetRequiredService<IHttpContextAccessor>())); // Add AuthenticationHttpMessageHandler

builder.Services.AddHttpClient("MvcClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiUrl"]); // Set the base address
}).AddHttpMessageHandler<AuthenticationHttpHandler>(); // Add AuthenticationHttpMessageHandler to HttpClient pipeline

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
