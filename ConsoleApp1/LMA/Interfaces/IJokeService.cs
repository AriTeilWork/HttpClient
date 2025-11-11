using System.Collections.Generic;
using System.Threading.Tasks;
using JokeConsole;

namespace LMA.Interfaces
{
    public interface IJokeService
    {
        Task<List<Joke>> GetJokesAsync();
    }
}
