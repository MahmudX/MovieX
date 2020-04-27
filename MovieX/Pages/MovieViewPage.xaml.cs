using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI.Xaml.Controls;

namespace MovieX.Pages
{
    public sealed partial class MovieViewPage : Page
    {
        public List<MovieDataModel> Movies { get; set; }
        public MovieViewPage()
        {
            this.InitializeComponent();
            Movies = DataAccess.GetMovieData().ToList();
        }
        private void MovieDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            if (e.Column.Tag.ToString() == "Title")
            {
                MovieDataGrid.ItemsSource = Movies.OrderBy(c => c.Title).ToList();
            }
            else if (e.Column.Tag.ToString() == "Rated")
            {
                //MovieDataGrid.ItemsSource = Movies.OrderBy(c => c.Rated).ToList();
                MovieDataGrid.ItemsSource = DataAccess.GetMovieData("Rated");
            }
            else if (e.Column.Tag.ToString() == "Genre")
            {
                MovieDataGrid.ItemsSource = Movies.OrderBy(c => c.Genre).ToList();
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
            DataAccess.DeleteMovie(item);
            var t = DataAccess.GetMovieData();
            Movies = t.ToList();
            MovieDataGrid.ItemsSource = Movies;
        }

        private async void fetchMissingDatabtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                var item = (MovieDataModel)MovieDataGrid.SelectedItem;
                MovieDataModel m = await MovieApi.GetMovieDataByNameAsync(item.Title);
                DataAccess.DeleteMovie(item);
                DataAccess.AddMovieData(m);
                Movies = DataAccess.GetMovieData().ToList();
                MovieDataGrid.ItemsSource = Movies;
            }
            catch
            {
            }
        }
    }
}
