using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MovieX.Pages
{
    public sealed partial class WishedMovies : Page
    {
        public List<MovieDataModel> Movies { get; set; }
        public WishedMovies()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Movies = await Task.Run(() =>
                DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies));
            var categories = await Task.Run(() =>
                DataAccess.GetFilterNames(DataAccess.FilterTable.CategoryTable));
            var years = await Task.Run(() =>
                DataAccess.GetFilterNames(DataAccess.FilterTable.YearTable));
            var ratings = await Task.Run(() =>
                DataAccess.GetFilterNames(DataAccess.FilterTable.RatedTable));

            categories.Insert(0, "All");
            years.Insert(0, "All");
            ratings.Insert(0, "All");

            categoryFilterCombo.ItemsSource = categories;
            yearFilterCombo.ItemsSource = years;
            ratedFilterCombo.ItemsSource = ratings;
        }
        private async void MovieDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            if (e.Column.Tag.ToString() == "Title")
            {
                MovieDataGrid.ItemsSource =
                    await Task.Run(() => Movies.OrderBy(c => c.Title).ToList());
            }
            else if (e.Column.Tag.ToString() == "Rated")
            {
                MovieDataGrid.ItemsSource =
                    await Task.Run(() => Movies.OrderBy(c => c.Rated).ToList());
            }
            else if (e.Column.Tag.ToString() == "Genre")
            {
                MovieDataGrid.ItemsSource =
                    await Task.Run(() => Movies.OrderBy(c => c.Genre).ToList());
            }
            else if (e.Column.Tag.ToString() == "Year")
            {
                MovieDataGrid.ItemsSource =
                    await Task.Run(() => Movies.OrderBy(c => c.Year).ToList());
            }
            else if (e.Column.Tag.ToString() == "Rating")
            {
                MovieDataGrid.ItemsSource =
                    await Task.Run(() => Movies.OrderBy(c => c.ImdbRating).ToList());
            }
        }
        private void searchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            MovieDataGrid.ItemsSource = from movie in Movies
                                        where movie.Title.Contains(searchBox.Text, StringComparison.OrdinalIgnoreCase)
                                        select movie;
        }
        private void itemDeleteBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = (MovieDataModel)MovieDataGrid.SelectedItem;
            DataAccess.DeleteMovieAsync(item,DataAccess.MovieTable.WishedMovies);
            Movies = DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies);
            MovieDataGrid.ItemsSource = Movies;
        }
        private async void fetchMissingDatabtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = (MovieDataModel)MovieDataGrid.SelectedItem;
            try
            {
                MovieDataModel m;
                if (!string.IsNullOrWhiteSpace(item.imdbID))
                {
                    m = await MovieApi.GetMovieDataByIdAsync(item.imdbID);
                }
                else
                {
                    m = await MovieApi.GetMovieDataByNameAsync(item.Title);
                }
                DataAccess.DeleteMovieAsync(item,DataAccess.MovieTable.WishedMovies);
                DataAccess.AddMovieDataAsync(m, DataAccess.MovieTable.WishedMovies);
                Movies = DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies);
                MovieDataGrid.ItemsSource = Movies;
            }
            catch
            {
            }

        }
        private void categoryFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string innerText = (string)categoryFilterCombo.SelectedItem;
            if (innerText == "All")
            {
                Movies = DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies);
            }
            else
            {
                Movies = (from movie in DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies)
                          where movie.Genre.Contains(innerText,
                          StringComparison.OrdinalIgnoreCase)
                          select movie).ToList();
            }
            MovieDataGrid.ItemsSource = Movies;
        }
        private void ratedFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string innerText = (string)ratedFilterCombo.SelectedItem;
            if (innerText == "All")
            {
                Movies = DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies);
            }
            else
            {
                Movies = (from movie in DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies)
                          where movie.Rated.Contains(innerText,
                          StringComparison.OrdinalIgnoreCase)
                          select movie).ToList();
            }
            MovieDataGrid.ItemsSource = Movies;
        }
        private void yearFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string innerText = (string)yearFilterCombo.SelectedItem;
            if (innerText == "All")
            {
                Movies = DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies);
            }
            else
            {
                Movies = (from movie in DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies)
                          where movie.Year.Contains(innerText,
                          StringComparison.OrdinalIgnoreCase)
                          select movie).ToList();
            }
            MovieDataGrid.ItemsSource = Movies;
        }

        private void itemAddtoWatchBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = (MovieDataModel)MovieDataGrid.SelectedItem;
            DataAccess.AddMovieDataAsync(item, DataAccess.MovieTable.WatchedMovies);
            DataAccess.DeleteMovieAsync(item, DataAccess.MovieTable.WishedMovies);
            Movies = DataAccess.GetMovieData(DataAccess.MovieTable.WishedMovies);
            MovieDataGrid.ItemsSource = Movies;
        }
    }
}
