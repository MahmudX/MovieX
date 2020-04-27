using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Services.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MovieX.Pages
{
    public sealed partial class SearchPage : Page
    {
        public static MovieDataModel movie;
        public static MovieSearchData movieSearchResult;
        public SearchPage()
        {
            this.InitializeComponent();
        }
        private async void searchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            movieSearchResult = await MovieApi.GetMovieSearchResult(searchBox.Text);
            GridResult.ItemsSource = movieSearchResult.Search;
        }

        private void saveToWatchedBtn_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.AddMovieData(movie);
        }

        private void saveToWishdBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void GridResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveToWatchedBtn.IsEnabled = true;
            saveToWishdBtn.IsEnabled = true;
            var item = (MovieSearchDataModel)GridResult.SelectedItem;
            movie = await MovieApi.GetMovieDataByIdAsync(item.ImdbId);
            selectedImage.Source = new BitmapImage(new Uri(movie.Poster, UriKind.Absolute));
            selectedPlot.Text = movie.Plot;
            selectedTitle.Text = movie.Title;
            selectedRating.Text = movie.ImdbRating;
            selectedYear.Text = movie.Year;
        }
    }
}
