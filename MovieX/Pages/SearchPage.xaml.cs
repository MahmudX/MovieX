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
            GridResult.SelectedItem = null;
            movieSearchResult = await MovieApi.GetMovieSearchResult(searchBox.Text);
            GridResult.ItemsSource = movieSearchResult.Search;
        }
        private void saveToWatchedBtn_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.AddMovieDataAsync(movie, DataAccess.MovieTable.WatchedMovies);
            DataAccess.AddFilterTableAsync(movie.Genre.Split(','), DataAccess.FilterTable.CategoryTable);
            DataAccess.AddFilterTableAsync(movie.Year.Split(','), DataAccess.FilterTable.YearTable);
            DataAccess.AddFilterTableAsync(movie.Rated.Split(','), DataAccess.FilterTable.RatedTable);
        }

        private void saveToWishdBtn_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.AddMovieDataAsync(movie, DataAccess.MovieTable.WishedMovies);
            DataAccess.AddFilterTableAsync(movie.Genre.Split(','), DataAccess.FilterTable.CategoryTable);
            DataAccess.AddFilterTableAsync(movie.Year.Split(','), DataAccess.FilterTable.YearTable);
            DataAccess.AddFilterTableAsync(movie.Rated.Split(','), DataAccess.FilterTable.RatedTable);
        }

        private async void GridResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var item = (MovieSearchDataModel)GridResult.SelectedItem;
            try
            {
                movie = await MovieApi.GetMovieDataByIdAsync(item.ImdbId);
                try
                {
                    selectedImage.Source = new BitmapImage(new Uri(movie.Poster, UriKind.Absolute));
                }
                catch (Exception)
                {
                }
                selectedPlot.Text = movie.Plot;
                selectedTitle.Text = movie.Title;
                selectedRating.Text = movie.ImdbRating;
                selectedYear.Text = movie.Year;
                saveToWatchedBtn.IsEnabled = true;
                saveToWishdBtn.IsEnabled = true;
                saveToWatchedBtn.Visibility = Visibility.Visible;
                saveToWishdBtn.Visibility = Visibility.Visible;
            }
            catch (Exception) { }
        }
    }
}
