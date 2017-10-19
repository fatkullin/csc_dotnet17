namespace MiniRoguelike
{
    internal static class Program
    {
        public static void Main()
        {
            var mapLoader = new MapLoader("Map.txt");
            var game = new Game(mapLoader.Load());

            var eventSender = new ConsoleEventSender();
            eventSender.ArrowPressed += game.OnArrowPressed;
            eventSender.EscapePressed += () => eventSender.Stop();
            eventSender.Run();
        }
    }
}
