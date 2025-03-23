using UnoLib1.Entity;
using UnoLib1.Interfaces;

namespace UnoApp6.Presentation;

public partial record LoginModel(IDispatcher Dispatcher, INavigator Navigator, IAuthenticationService Authentication, ILogger<LoginModel> Logger, IUserRepository UserRepository, ITestRepository TestRepository)
{
    public string Title { get; } = "Login";
    
    public IState<string> LoginName => State<string>.Value(this, () => string.Empty);
    public IState<string> Password => State<string>.Value(this, () => string.Empty);
    
    public async ValueTask Login(CancellationToken token)
    {
        Logger.LogInformation("Button clicked");
        
        var credentials = new Dictionary<string, string>
        {
            { "login", (await LoginName)! },
            { "password", (await Password)! }
        };
        
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = (await LoginName)!,
            Age = 10
        };
        
        var test = new TestEntity
        {
            Id = Guid.NewGuid(),
            Ean = "1234567890123",
            Quantity = 10
        };
        
        await UserRepository.AddAsync(user);
        await TestRepository.AddAsync(test);
        
        Logger.LogInformation("Login credentials: {Credentials}", credentials);
        
        var success = await Authentication.LoginAsync(Dispatcher, credentials);
        if (success)
        {
            await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack, cancellation: token);
        }
    }
}
