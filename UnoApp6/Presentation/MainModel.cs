namespace UnoApp6.Presentation;

public partial record MainModel
{
    private readonly INavigator _navigator;
    private readonly ILogger<MainModel> _logger;
    private readonly IAuthenticationService _authentication;

    public MainModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        IAuthenticationService authentication,
        INavigator navigator,
        ILogger<MainModel> logger)
    {
        _navigator = navigator;
        _logger = logger;
        _authentication = authentication;
        Title = "Main";
        Title += $" - {localizer["ApplicationName"]}";
        Title += $" - {appInfo?.Value?.Environment}";
        _authentication.LoggedOut += (sender, args) =>
        {
            _logger.LogInformation("User logged out.");
        };
    }

    public string? Title { get; }

    public IState<string> Name => State<string>.Value(this, () => string.Empty);

    
    public async Task GoToSecond()
    {
        var name = await Name;
        await _navigator.NavigateViewModelAsync<SecondModel>(this, data: new Entity(name!));
    }

    public async ValueTask Logout()
    {
        _logger.LogInformation("Logout");
        try
        {
            await _authentication.LogoutAsync();
            await _navigator.NavigateViewModelAsync<LoginModel>(this, qualifier: Qualifiers.ClearBackStack);
            _logger.LogInformation("Logout success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Logout failed");
        }
    }
}
