using SignalrImplementation.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("https://localhost:5173") // Specify the allowed origin(s)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()); // Necessary for SignalR
});

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");

app.Run();
