using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;

namespace UnoApp6.Presentation;

public sealed partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
        Loaded += LoginPage_Loaded;
    }

    private void LoginPage_Loaded(object sender, RoutedEventArgs e)
    {
        TxtLoginName.Focus(FocusState.Programmatic);
    }

    private void Page_LostFocus(object sender, RoutedEventArgs e)
    {
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
