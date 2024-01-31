using Microsoft.EntityFrameworkCore;
using QuqueBot.Data;
using QuqueBot.Interfaces;
using QuqueBot.Repositories;
using QuqueBot.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});

builder.Services.AddScoped<IQueuesRepo, QueuesRepo>();
builder.Services.AddScoped<ITelegramUsersRepo, TelegramUsersRepo>();

builder.Services.AddTelegramBotClient(builder.Configuration["TelegramApiKey"]);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseTelegramBotClient();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();