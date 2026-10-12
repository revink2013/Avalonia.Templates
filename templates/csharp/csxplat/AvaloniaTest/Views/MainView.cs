using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using AvaloniaTest.ViewModels;
#if (DrawerMainPageChosen || NavigationMainPageChosen || TabbedMainPageChosen)
using System;
#endif
#if (DrawerMainPageChosen)
using System.Reflection;
using Avalonia.Controls.Templates;
using Avalonia.Platform;
#endif

namespace AvaloniaTest.Views;

#if (ContentMainPageChosen)
public class MainView : ContentPage
#elif (TabbedMainPageChosen)
public class MainView : TabbedPage
#elif (DrawerMainPageChosen)
public class MainView : DrawerPage
#elif (NavigationMainPageChosen)
public class MainView : NavigationPage
#else
public class MainView : UserControl
#endif
{
#if (DrawerMainPageChosen)
    protected readonly ListBox _drawerList;
    protected ContentPage _contentPage;
#endif
    public MainView()
    {
#if (TabbedMainPageChosen)
        var res = Application.Current?.Resources ?? throw new ArgumentOutOfRangeException("Could not get current running application!");
        Pages = 
		[
			new ContentPage()
			{
				Header = "Home",
                Icon = new PathIcon(){ Data = res["HomeIcon"] as StreamGeometry, },
				Content = new TextBox()
				{
					HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
					VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
					[!TextBox.TextProperty] = CompiledBinding.Create<MainViewModel, string>(vm => vm.Greeting),
				}
			},
			new ContentPage()
			{
				Header = "Settings",
                Icon = new PathIcon(){ Data = res["SettingsIcon"] as StreamGeometry, },
				Content = new TextBlock()
				{
					VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
					HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
					Text = "This is a text block",
				}
			}
		];
#elif (DrawerMainPageChosen)
        string assembly = Assembly.GetExecutingAssembly().GetName().Name ?? "";
        var res = Application.Current?.Resources ?? throw new ArgumentOutOfRangeException("Could not get current running application!");
        Label header_label = new Label()
        {
            FontSize = 16,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Content = "Avalonia HU",
        };
        DrawerHeader = new DockPanel()
        {
            Margin = new(10),
            LastChildFill = true,
            Children =
            {
                header_label,
                new Image()
                {
                    Source = new Avalonia.Media.Imaging.Bitmap
                    (
                        AssetLoader.Open(new($"avares://{assembly}/Assets/Icon.png"))
                    ),
                    Height = 150,
                    Margin = new(10),
                    Stretch = Avalonia.Media.Stretch.Uniform,
                }
            }
        };
        DockPanel.SetDock(header_label, Dock.Bottom);
        DrawerFooter = new Grid()
        {
            Margin = new(10),
            Children =
            {
                new Label()
                {
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Content = "Avalonia-L",
                }
            }
        };
        Drawer = _drawerList = new ListBox()
        {
            SelectionMode = SelectionMode.AlwaysSelected,
            Items =
            {
                new ListBoxItem()
                {
                    Content = new StackPanel()
                    {
                        Spacing = 15,
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        Children =
                        {
                            new PathIcon(){ Data = res["HomeIcon"] as StreamGeometry, },
                            new Label()
                            {
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                FontSize = 16,
                                VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                Padding = new(10),
                                Content = "Home tab"
                            },
                        },
                    },
                },
                new ListBoxItem()
                {
                    Content = new StackPanel()
                    {
                        Spacing = 15,
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        Children =
                        {
                            new PathIcon()
                            {
                                Data = res["SettingsIcon"] as StreamGeometry,
                            },
                            new Label()
                            {
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                FontSize = 16,
                                VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                Padding = new(10),
                                Content = "Settings tab"
                            },
                        },
                    },
                },
            },
        };
		this.Content = _contentPage = new ContentPage()
        {
            DataTemplates =
            {
				new FuncDataTemplate<HomeViewModel>((value, namescope) => new ContentPage()
					{
                        DataContext = value,
                        Header = "Home",
                        Icon = new PathIcon() {Data = res["HomeIcon"] as StreamGeometry},
                        Content = new TextBlock()
                        {
                            [!TextBox.TextProperty] = CompiledBinding.Create<HomeViewModel, string>(vm => vm.Greeting),
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
							FontSize = 32,
                        }
					}
				),
				new FuncDataTemplate<SettingsViewModel>((value, namescope) => new ContentPage()
					{
                        DataContext = value,
                        Header = "Settings",
                        Icon = new PathIcon() {Data = res["SettingsIcon"] as StreamGeometry},
                        Content = new TextBlock()
                        {
                            [!TextBox.TextProperty] = CompiledBinding.Create<SettingsViewModel, string>(vm => vm.Greeting),
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        }
					}
				),
            }
        };
        _drawerList.SelectionChanged += DrawerList_SelectionChanged;
#elif (NavigationMainPageChosen)
#else
        Content = new TextBox()
        {
            [!TextBox.TextProperty] = CompiledBinding.Create<MainViewModel, string>(vm => vm.Greeting),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        };
#endif
    }
#if (DrawerMainPageChosen)
    protected override async void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdatePage(_drawerList.SelectedIndex);
    }

    private async void DrawerList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_contentPage != null && sender is ListBox listbox)
        {
            var index = listbox.SelectedIndex;
            UpdatePage(index);
        }
    }

    private void UpdatePage(int index)
    {
        ViewModelBase page = index switch
        {
            0 => new HomeViewModel(),
            1 => new SettingsViewModel(),
            _ => throw new NotImplementedException()
        };
		
		if (page != null)
            _contentPage.Content = page;

        IsOpen = false;
    }
#elif (NavigationMainPageChosen)

    protected override async void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (CurrentPage == null)
            await PushAsync(new HomeView()
            {
                DataContext = new HomeViewModel()
            });
    }
#endif
}