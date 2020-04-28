using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieX
{
    class MovieApi
    {
        public async static Task<MovieDataModel> GetMovieDataByNameAsync(string movieTitle)
        {
            var http = new HttpClient();
            var response = await http.GetAsync(string.Format("http://www.omdbapi.com/?apikey=e2d74f62&t={0}", movieTitle.Replace(' ', '+')));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MovieDataModel>(result);
        }
        public async static Task<MovieDataModel> GetMovieDataByIdAsync(string ID)
        {
            var http = new HttpClient();
            var response = await http.GetAsync(String.Format("http://www.omdbapi.com/?apikey=e2d74f62&i={0}", ID));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MovieDataModel>(result);
        }
        public async static Task<MovieSearchData> GetMovieSearchResult(string query)
        {
            var http = new HttpClient();
            var response = await http.GetAsync(string.Format("http://www.omdbapi.com/?apikey=e2d74f62&s={0}", query.Replace(' ', '+')));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MovieSearchData>(result);
        }
    }
    public partial class MovieSearchData
    {
        public List<MovieSearchDataModel> Search { get; set; }
        public string TotalResults { get; set; }
        public string Response { get; set; }
    }

    public partial class MovieSearchDataModel
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string ImdbId { get; set; }
        private string _type;
        public string Type { get { return _type.ToUpperInvariant(); } set { _type = value; } }
        public string Poster { get; set; }
    }
    public class MovieDataModel
    {
        public string Title { get; set; } = "";
        public string Year { get; set; } = "";
        public string Rated { get; set; } = "";
        public string Released { get; set; } = "";
        public string Runtime { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Director { get; set; } = "";
        public string Actors { get; set; } = "";
        public string Plot { get; set; } = "";
        public string Language { get; set; } = "";
        public string Country { get; set; } = "";
        public string Poster { get; set; } = "";
        public string ImdbRating { get; set; } = "";
        public string imdbID { get; set; } = "";
        public string Type { get; set; } = "";
    }
}
