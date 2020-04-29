using MovieX.Pages;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MovieX
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(SearchPage));
        }
        private void navigationBar_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (sender.SelectedItem == searchPage)
            {
                contentFrame.Navigate(typeof(SearchPage));
            }
            else if (sender.SelectedItem == viewPage)
            {
                contentFrame.Navigate(typeof(MovieViewPage));
            }
            else if (sender.SelectedItem == wishedPage)
            {
                contentFrame.Navigate(typeof(WishedMovies));
            }
            else if (sender.SelectedItem == manualAdd)
            {
                contentFrame.Navigate(typeof(ManualAdd));
            }
            else if (sender.SelectedItem == optionPage)
            {
                contentFrame.Navigate(typeof(Settings));
            }
        }
    }
}
