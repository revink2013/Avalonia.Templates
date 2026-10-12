using Avalonia.Controls;
using Avalonia.Data;
using AvaloniaTest.ViewModels;

namespace AvaloniaTest.Views;

public class HomeView : ContentPage
{
    protected readonly Button SettingButton;
    public HomeView()
    {
        DataContext = new HomeViewModel();
        Content = new StackPanel()
        {
            Children = 
            {
                new TextBox()
                {
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    [!TextBox.TextProperty] = CompiledBinding.Create<HomeViewModel, string>(
                        vm => vm.Greeting
                    ),
                },
                (SettingButton = new Button() { Content = "Open Settings", }),
            }
        };
        SettingButton.Click += SettingButton_Click;
    }

    private void SettingButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (this.Navigation != null)
        {
            Navigation.PushAsync(
                new SettingsView(){DataContext = new SettingsViewModel()}
            );
        }
    }
}