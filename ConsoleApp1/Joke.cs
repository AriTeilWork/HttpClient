namespace JokeConsole
{
    public abstract class Joke
    {
        // Base class - common behavior can go here
    }

    public class OneLiner : Joke
    {
        public string Content { get; set; } = string.Empty;

        public override string ToString()
        {
            return Content;
        }
    }

    public class TwoLiner : Joke
    {
        public string Setup { get; set; } = string.Empty;
        public string Delivery { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Setup} - {Delivery}";
        }
    }
}
