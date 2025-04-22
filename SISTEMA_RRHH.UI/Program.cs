using Microsoft.AspNetCore.Authentication.Cookies;
using SISTEMA_RRHH.UI.Seguridad; // Importamos el filtro NoCacheFilter

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios con filtro global
builder.Services.AddControllersWithViews(options =>
{
    // Evita que las páginas queden en caché
    options.Filters.Add<NoCacheFilter>();
});

builder.Services.AddSession();

// Autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/ControladorLogin/Ingresar";
        options.AccessDeniedPath = "/ControladorLogin/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

// Política para RRHH o Sistemas
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RRHHoSistemas", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == "DepartamentoID" &&
                (c.Value == "1" || c.Value == "3"))
        ));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ControladorLogin}/{action=Ingresar}/{id?}");

app.Run();
