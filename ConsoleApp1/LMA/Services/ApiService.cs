using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JokeConsole;
using LMA.Interfaces;

namespace LMA.Services
{
    public class ApiService : IJokeService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Joke>> GetJokesAsync()
        {
            try
            {
                string url = "https://v2.jokeapi.dev/joke/Programming?blacklistFlags=nsfw,religious,political,racist,sexist,explicit&amount=3";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                var doc = await JsonDocument.ParseAsync(stream);

                var jokes = new List<Joke>();

                if (doc.RootElement.TryGetProperty("jokes", out JsonElement jokesElement))
                {
                    foreach (var item in jokesElement.EnumerateArray())
                    {
                        string type = item.GetProperty("type").GetString();
                        if (type == "single")
                        {
                            jokes.Add(new OneLiner { Content = item.GetProperty("joke").GetString() ?? string.Empty });
                        }
                        else
                        {
                            jokes.Add(new TwoLiner
                            {
                                Setup = item.GetProperty("setup").GetString() ?? string.Empty,
                                Delivery = item.GetProperty("delivery").GetString() ?? string.Empty
                            });
                        }
                    }
                }
                else
                {
                    var root = doc.RootElement;
                    string type = root.GetProperty("type").GetString();
                    if (type == "single")
                    {
                        jokes.Add(new OneLiner { Content = root.GetProperty("joke").GetString() ?? string.Empty });
                    }
                    else
                    {
                        jokes.Add(new TwoLiner
                        {
                            Setup = root.GetProperty("setup").GetString() ?? string.Empty,
                            Delivery = root.GetProperty("delivery").GetString() ?? string.Empty
                        });
                    }
                }

                return jokes;
            }
            catch (Exception ex)
            {
                return new List<Joke>
                {
                    new OneLiner{ Content = "Could not fetch jokes: " + ex.Message }
                };
            }
        }
    }
}
