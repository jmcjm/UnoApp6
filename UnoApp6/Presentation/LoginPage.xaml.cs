using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;
using UnoLib1.Interfaces;

namespace UnoApp6.Presentation;

public sealed partial class LoginPage : Page
{
    private bool _isLoaded;
  
    public LoginPage()
    {
        InitializeComponent();
        Loaded += LoginPage_Loaded;
        Unloaded += LoginPage_Unloaded;
    }
    
    private void LoginPage_Loaded(object sender, RoutedEventArgs e)
    {
        TxtLoginName.Focus(FocusState.Programmatic);
        _isLoaded = true;
    }
    
    private void LoginPage_Unloaded(object sender, RoutedEventArgs e)
    {
        _isLoaded = false;
    }

    private void Page_LostFocus(object sender, RoutedEventArgs e)
    {
        if (!_isLoaded) return;
        
        DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
        {
            await Task.Delay(100); // Slightly longer delay to allow focus transitions
            var focusedElement = FocusManager.GetFocusedElement(XamlRoot);
            
            if (focusedElement is Control control && control.Name == "TxtPassword")
            {
                // Jeżeli jesteśmy w polu hasła, to nie chcemy nic robić
            }
            else
            {
                // Jeżeli focus uciekał gdziekolwiek indziej to przywracamy go na pole loginu
                TxtLoginName.Focus(FocusState.Programmatic);
            }
        });
    }
}
