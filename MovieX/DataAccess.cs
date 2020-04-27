using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace MovieX
{
    class DataAccess
    {
        public async static void InitializeMovieDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("moviesdatabase.db", CreationCollisionOption.OpenIfExists);
            //await ApplicationData.Current.LocalFolder.CreateFileAsync("moviesdatabase.db", CreationCollisionOption.ReplaceExisting);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string tableCommand = "CREATE TABLE IF NOT EXISTS \"watchedmovies\" " +
                    "(\"Title\" TEXT," +
                    "\"Year\"  INTEGER," +
                    "\"Rated\" TEXT," +
                    "\"Released\" TEXT," +
                    "\"Runtime\" TEXT," +
                    "\"Genre\" TEXT," +
                    "\"Director\" TEXT," +
                    "\"Actors\" TEXT," +
                    "\"Plot\" TEXT UNIQUE," +
                    "\"Language\" TEXT," +
                    "\"Country\" TEXT," +
                    "\"ImdbRating\" TEXT," +
                    "\"ImageUrl\" TEXT UNIQUE)";
                //@,@,@IMDb,@ImageUrl
                //string tableCommand = "CREATE TABLE IF NOT " +
                //    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                //    "Text_Entry NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
                db.Close();
            }
        }
        public static void AddMovieData(MovieDataModel movie)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "INSERT OR REPLACE INTO watchedmovies VALUES (@Title,@Year,@Rated,@Released,@Runtime,@Genre,@Director,@Actors,@Plot,@Language,@Country,@IMDb,@ImageUrl);";
                insertCommand.Parameters.AddWithValue("@Title", movie.Title);
                insertCommand.Parameters.AddWithValue("@Year", movie.Year);
                insertCommand.Parameters.AddWithValue("@Rated", movie.Rated);
                insertCommand.Parameters.AddWithValue("@Released", movie.Released);
                insertCommand.Parameters.AddWithValue("@Runtime", movie.Runtime);
                insertCommand.Parameters.AddWithValue("@Genre", movie.Genre);
                insertCommand.Parameters.AddWithValue("@Director", movie.Director);
                insertCommand.Parameters.AddWithValue("@Actors", movie.Actors);
                insertCommand.Parameters.AddWithValue("@Plot", movie.Plot);
                insertCommand.Parameters.AddWithValue("@Language", movie.Language);
                insertCommand.Parameters.AddWithValue("@Country", movie.Country);
                insertCommand.Parameters.AddWithValue("@IMDb", movie.ImdbRating);
                insertCommand.Parameters.AddWithValue("@ImageUrl", movie.Poster);
                insertCommand.ExecuteReader();
                db.Close();
            }
        }
        public static ObservableCollection<MovieDataModel> GetMovieData(string sortby = "Title")
        {
            ObservableCollection<MovieDataModel> entries = new ObservableCollection<MovieDataModel>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    (string.Format("SELECT * from watchedmovies ORDER BY \"{0}\" Asc", sortby), db);

                //SqliteCommand tableCommand = new SqliteCommand("SELECT * FROM watchedmovies",db);
                //tableCommand.ExecuteReader();
                SqliteDataReader query = selectCommand.ExecuteReader();
                MovieDataModel movie;
                while (query.Read())
                {
                    movie = new MovieDataModel
                    {
                        Title = query.GetString(0),
                        Year = query.GetString(1),
                        Rated = query.GetString(2),
                        Released = query.GetString(3),
                        Runtime = query.GetString(4),
                        Genre = query.GetString(5),
                        Director = query.GetString(6),
                        Actors = query.GetString(7),
                        Plot = query.GetString(8),
                        Language = query.GetString(9),
                        Country = query.GetString(10),
                        ImdbRating = query.GetString(11),
                        Poster = query.GetString(12)
                    };
                    entries.Add(movie);
                }

                db.Close();
            }

            return entries;
        }
        public static void DeleteMovie(MovieDataModel movie)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "DELETE from watchedmovies WHERE \"Title\" = @Title AND " +
                    "\"Runtime\" = @Runtime AND \"Director\" = @Director";
                insertCommand.Parameters.AddWithValue("@Title", movie.Title);
                insertCommand.Parameters.AddWithValue("@Runtime", movie.Runtime);
                insertCommand.Parameters.AddWithValue("@Director", movie.Director);
                insertCommand.ExecuteReader();
                db.Close();
            }
        }
    }
}
