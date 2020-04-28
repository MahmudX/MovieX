using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace MovieX
{
    class DataAccess
    {
        public async static void InitializeMovieDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("moviesdatabase.db", CreationCollisionOption.OpenIfExists);
            // await ApplicationData.Current.LocalFolder.CreateFileAsync("moviesdatabase.db", CreationCollisionOption.ReplaceExisting);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string watchedTable = "CREATE TABLE IF NOT EXISTS \"WatchedMovies\" " +
                    "(\"Title\" TEXT," +
                    "\"Year\"  INTEGER, \"Rated\" TEXT," +
                    "\"Released\" TEXT, \"Runtime\" TEXT," +
                    "\"Genre\" TEXT, \"Director\" TEXT," +
                    "\"Actors\" TEXT,\"Plot\" TEXT," +
                    "\"Language\" TEXT,\"Country\" TEXT, \"ImdbRating\" TEXT," +
                    "\"ImageUrl\" TEXT, \"Type\" TEXT, \"ID\" TEXT UNIQUE)";
                string wishedTable = watchedTable.Replace("WatchedMovies", "WishedMovies");

                string categoryTable =
                    "CREATE TABLE IF NOT EXISTS \"CategoryTable\" (\"Category\" TEXT UNIQUE)";
                string yearTable =
                    "CREATE TABLE IF NOT EXISTS \"YearTable\" (\"Year\" TEXT UNIQUE)";
                string ratedTable =
                    "CREATE TABLE IF NOT EXISTS \"RatedTable\" (\"Rated\" TEXT UNIQUE)";
                SqliteCommand createTable = new SqliteCommand(watchedTable, db);
                createTable.ExecuteReader();
                createTable = new SqliteCommand(categoryTable, db);
                createTable.ExecuteReader();
                createTable = new SqliteCommand(wishedTable, db);
                createTable.ExecuteReader();
                createTable = new SqliteCommand(yearTable, db);
                createTable.ExecuteReader();
                createTable = new SqliteCommand(ratedTable, db);
                createTable.ExecuteReader();
                db.Close();
            }
        }
        public async static void AddMovieDataAsync(MovieDataModel movie, MovieTable table)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = string.Format("INSERT OR REPLACE INTO {0} VALUES (@Title,@Year,@Rated,@Released,@Runtime,@Genre,@Director,@Actors,@Plot,@Language,@Country,@IMDb,@ImageUrl,@Type,@ID);", table.ToString());
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
                insertCommand.Parameters.AddWithValue("@Type", movie.Type);
                insertCommand.Parameters.AddWithValue("@ID", movie.imdbID);
                await Task.Run(() => insertCommand.ExecuteReader());
                db.Close();
            }
        }
        public static async void AddFilterTableAsync(string[] genres, FilterTable filterTable)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                foreach (var genre in genres)
                {
                    db.Open();
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;
                    insertCommand.CommandText =
                        string.Format("INSERT OR REPLACE INTO {0} VALUES (@data);", filterTable.ToString());
                    insertCommand.Parameters.AddWithValue("@data", genre.Trim());
                    await Task.Run(() => insertCommand.ExecuteReader());
                    db.Close();
                }
            }
        }
        public static List<MovieDataModel> GetMovieData(MovieTable movieTable)
        {
            List<MovieDataModel> entries = new List<MovieDataModel>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {

                db.Open();
                SqliteCommand selectCommand = new SqliteCommand
                    (string.Format("SELECT * from {0}  ORDER BY \"Title\"", movieTable), db);
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
                        Poster = query.GetString(12),
                        Type = query.GetString(13),
                        imdbID = query.GetString(14)
                    };
                    entries.Add(movie);
                }

                db.Close();
            }
            return entries;
        }
        public async static void DeleteMovieAsync(MovieDataModel movie, MovieTable table)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = string.Format("DELETE from {0} WHERE \"Title\" = @Title AND " +
                    "\"Runtime\" = @Runtime AND \"Director\" = @Director", table);
                insertCommand.Parameters.AddWithValue("@Title", movie.Title);
                insertCommand.Parameters.AddWithValue("@Runtime", movie.Runtime);
                insertCommand.Parameters.AddWithValue("@Director", movie.Director);
                await Task.Run(() => insertCommand.ExecuteReader());
                db.Close();
            }
        }
        public static List<string> GetFilterNames(FilterTable filter)
        {
            List<string> categories = new List<string>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "moviesdatabase.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand
                    (string.Format("SELECT * from {0} ORDER BY \"Category\" Asc", filter.ToString()), db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                string movie;
                while (query.Read())
                {
                    movie = query.GetString(0);
                    categories.Add(movie);
                }
                db.Close();
            }
            return categories;
        }
        public enum MovieTable
        {
            WishedMovies, WatchedMovies
        }
        public enum FilterTable
        {
            CategoryTable, YearTable, RatedTable
        }
    }
}
