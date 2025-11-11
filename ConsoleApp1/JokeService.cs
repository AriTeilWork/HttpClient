using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JokeConsole
{
    public class JokeService : IJokeService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

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
                        if (item.GetProperty("type").GetString() == "single")
                        {
                            jokes.Add(new Joke { Content = item.GetProperty("joke").GetString() });
                        }
                        else
                        {
                            string setup = item.GetProperty("setup").GetString();
                            string delivery = item.GetProperty("delivery").GetString();
                            jokes.Add(new Joke { Content = $"{setup} - {delivery}" });
                        }
                    }
                }
                else
                {
                    // If the API returns a single joke object
                    var root = doc.RootElement;
                    if (root.GetProperty("type").GetString() == "single")
                    {
                        jokes.Add(new Joke { Content = root.GetProperty("joke").GetString() });
                    }
                    else
                    {
                        string setup = root.GetProperty("setup").GetString();
                        string delivery = root.GetProperty("delivery").GetString();
                        jokes.Add(new Joke { Content = $"{setup} - {delivery}" });
                    }
                }

                return jokes;
            }
            catch (Exception ex)
            {
                // On error, return a fallback list
                return new List<Joke>
                {
                    new Joke{ Content = "Could not fetch jokes: " + ex.Message }
                };
            }
        }
    }
}
