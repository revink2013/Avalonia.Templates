using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using AvaloniaTest.ViewModels;

namespace AvaloniaTest.Views;

public class SettingsView : ContentPage
{
    public SettingsView()
    { 
        Content = new TextBox()
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            [!TextBox.TextProperty] = CompiledBinding.Create<SettingsViewModel, string>(
                vm => vm.Greeting
            ),
            FontSize = 32,
        };
        DataContext = new SettingsViewModel();
    }

}