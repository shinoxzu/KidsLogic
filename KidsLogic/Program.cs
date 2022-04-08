using KidsLogic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:8083");

builder.Services.AddMvc();
builder.Services.AddHttpClient();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataBaseContext>(options => options.UseNpgsql(connection));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/account/login");
builder.Services.AddAuthorization();

builder.Services.AddSingleton<PasswordGenerator>();

IConfigurationSection? mailData = builder.Configuration.GetSection("MailSettings");
builder.Services.AddSingleton(new EmailService(mailData["Mail"], mailData["Password"]));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseForwardedHeaders(new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

app.Run();