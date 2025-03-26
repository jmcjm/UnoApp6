using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Uno.Resizetizer;
using UnoApp6.Services;
using UnoLib1;
using UnoLib1.Interfaces;
using UnoLib1.Mapping;
using UnoLib1.Repository;

namespace UnoApp6;

public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);
                }, enableUnoLogging: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                // Register Json serializers (ISerializer and ISerializer)
                .UseSerialization((context, services) => services
                    .AddContentSerializer(context)
                    .AddJsonTypeInfo(WeatherForecastContext.Default.IImmutableListWeatherForecast))
                .UseHttp((context, services) => services
                    // Register HttpClient
#if DEBUG
                    // DelegatingHandler will be automatically injected into Refit Client
                    .AddTransient<DelegatingHandler, DebugHttpHandler>()
#endif
                    .AddSingleton<IWeatherCache, WeatherCache>()
                    .AddRefitClient<IApiClient>(context))
                // .UseAuthentication(u => u.AddOidc())
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IAuthenticationService, JwtAuthenticationService>();
                    services.AddHttpClient<IAuthenticationService, JwtAuthenticationService>(client =>
                    {
                        client.BaseAddress = new Uri(context.Configuration["ApiSettings:Url"] ??
                                                     throw new ArgumentNullException());
                    });
                    var databasePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "MyData.db");
                    
                    Console.WriteLine($"Database path: {databasePath}");
                    
                    services.AddDbContext<UnoLibDbContext>(options =>
                        options.UseSqlite($"Data Source={databasePath}"));

                    services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
                    services.AddAutoMapper(typeof(MappingProfile));
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<ITestRepository, TestRepository>();
                    services.AddScoped<ICredsRepository, CredsRepository>();
                })
                .UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = await builder.NavigateAsync<Shell>
        (initialNavigate: async (services, navigator) =>
        {
            // TODO: Na pewno da się to dać w lepszy miejscu
            var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
            await databaseInitializer.InitializeAsync();

            var auth = services.GetRequiredService<IAuthenticationService>();
            var authenticated = await auth.RefreshAsync();
            if (authenticated)
            {
                await navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.Nested);
            }
            else
            {
                await navigator.NavigateViewModelAsync<LoginModel>(this, qualifier: Qualifiers.Nested);
            }
        });
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellModel)),
            new ViewMap<LoginPage, LoginModel>(),
            new ViewMap<MainPage, MainModel>(),
            new DataViewMap<SecondPage, SecondModel, Entity>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                Nested:
                [
                    new RouteMap("Login", View: views.FindByViewModel<LoginModel>()),
                    new RouteMap("Main", View: views.FindByViewModel<MainModel>(), IsDefault: true),
                    new RouteMap("Second", View: views.FindByViewModel<SecondModel>()),
                ]
            )
        );
    }
}
