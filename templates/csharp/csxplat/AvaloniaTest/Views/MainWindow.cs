using System.Reflection;
using Avalonia.Controls;
using Avalonia.Platform;
using AvaloniaTest.ViewModels;

namespace AvaloniaTest.Views;

public class MainWindow : Window
{
    public MainWindow()
    {
        string assembly = Assembly.GetExecutingAssembly().GetName().Name ?? "";
		Icon = new(
			AssetLoader.Open(
				new(
					$"avares://{assembly}/Assets/avalonia-logo.ico"
				)
			)
		);
		Title = "AvaloniaTest";
		Content = new MainView() { DataContext = new MainViewModel()};

    }
}