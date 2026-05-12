using Microsoft.AspNetCore.Authentication;
using Musica.Components;
using Musica;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

var connectionString = builder.Configuration.GetConnectionString("SQLConnection");

builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication("Cookie").AddCookie("Cookie", options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Login";
    options.LogoutPath = "/Logout";
});

var app = builder.Build();

app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync();
    
    return Results.Redirect("/");
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


app.UseAntiforgery();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
