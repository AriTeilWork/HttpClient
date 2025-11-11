using System.Collections.Generic;
using System.Threading.Tasks;

namespace JokeConsole
{
    public interface IJokeService
    {
        Task<List<Joke>> GetJokesAsync();
    }
}
