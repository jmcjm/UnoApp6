using UnoLib1.Entity;
using UnoLib1.Interfaces;

namespace UnoApp6.Presentation;

public partial record LoginModel(IDispatcher Dispatcher, INavigator Navigator, ILogger<LoginModel> Logger, IAuthenticationService Authentication, ICredsRepository CredsRepository)
{
    public string Title { get; } = "Login";
    
    public IState<string> LoginName => State<string>.Value(this, () => CredsRepository.GetAsync().GetAwaiter().GetResult()?.Username ?? string.Empty);
    public IState<string> Password => State<string>.Value(this, () => CredsRepository.GetAsync().GetAwaiter().GetResult()?.Password ?? string.Empty);
    
    public async ValueTask Login(CancellationToken token)
    {
        Logger.LogInformation("Button clicked");
        
        var username = (await LoginName)!;
        var password = (await Password)!;
        
        var credentials = new Dictionary<string, string>
        {
            { "login", username },
            { "password", password }
        };

        //Jeżeli jakieś są zapisane to, je usuwamy robiąc miejsce na nowe
        await CredsRepository.DeleteAsync();

        
        Logger.LogInformation("Login credentials: {Credentials}", credentials);
        
        var success = await Authentication.LoginAsync(Dispatcher, credentials);
        Logger.LogInformation("Login result: {Result}", success);
        if (success)
        {
            //Jeżeli udało się zalogować, to zapisujemy credsy, bo są poprawne
            await CredsRepository.AddAsync(new CredsEntity(Guid.NewGuid(), username, password));
            await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack, cancellation: token);
        }
    }
}
