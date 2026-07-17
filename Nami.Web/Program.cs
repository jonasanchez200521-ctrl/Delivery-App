using Nami.Web.Components;
using Nami.Web.Services;
using Nami.Web.State;

namespace Nami.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5214";

            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

            builder.Services.AddScoped<AuthState>();
            builder.Services.AddScoped<ApiClient>();
            builder.Services.AddScoped<AuthApiService>();
            builder.Services.AddScoped<CatalogApiService>();
            builder.Services.AddScoped<CartApiService>();
            builder.Services.AddScoped<OrderApiService>();
            builder.Services.AddScoped<UserApiService>();
            builder.Services.AddScoped<PromotionApiService>();
            builder.Services.AddScoped<ReportApiService>();
            builder.Services.AddScoped<NotificationApiService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
