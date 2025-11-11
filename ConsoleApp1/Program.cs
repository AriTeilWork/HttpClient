using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokeConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            IJokeService jokeService = new JokeServiceJokeDevApi2(httpClient);
            List<Joke> jokes = await jokeService.GetJokesAsync();

            Console.WriteLine("Jokes:");

            int i = 1;
            foreach (Joke joke in jokes)
            {
                Console.WriteLine($"{i}. {joke}");
                i++;
            }
        }
    }
}
