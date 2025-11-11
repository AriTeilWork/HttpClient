using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMA.Interfaces;
using LMA.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JokeConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Register a single HttpClient for ApiService and map IJokeService to ApiService
                    services.AddHttpClient<IJokeService, ApiService>();
                })
                .Build();

            // Resolve the service from DI
            var jokeService = host.Services.GetRequiredService<IJokeService>();

            List<Joke> jokes = await jokeService.GetJokesAsync();

            Console.WriteLine("Jokes:");

            int i = 1;
            foreach (Joke joke in jokes)
            {
                Console.WriteLine($"{i}. {joke}");
                i++;
            }

            await host.StopAsync();
        }
    }
}
