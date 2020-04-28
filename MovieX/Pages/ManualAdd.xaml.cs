using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieX.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManualAdd : Page
    {
        public ManualAdd()
        {
            this.InitializeComponent();
        }

        private void titletext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titletext.Text))
            {
                savewatchedbtn.IsEnabled = false;
                savewishedbtn.IsEnabled = false;
            }
            else
            {
                savewatchedbtn.IsEnabled = true;
                savewishedbtn.IsEnabled = true;
            }
        }

        private void savewishedbtn_Click(object sender, RoutedEventArgs e)
        {
            MovieDataModel movie = new MovieDataModel()
            {
                Title = titletext.Text,
                Runtime = durationText.Text,
                Year = yearText.Text,
                Actors = castText.Text,
                Type = typeText.Text,
                Genre = genreText.Text
            };
            DataAccess.AddMovieDataAsync(movie, DataAccess.MovieTable.WishedMovies);

        }

        private void savewatchedbtn_Click(object sender, RoutedEventArgs e)
        {
            MovieDataModel movie = new MovieDataModel()
            {
                Title = titletext.Text,
                Runtime = durationText.Text,
                Year = yearText.Text,
                Actors = castText.Text,
                Type = typeText.Text,
                Genre = genreText.Text
            };
            DataAccess.AddMovieDataAsync(movie, DataAccess.MovieTable.WatchedMovies);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void durationText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
