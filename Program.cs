using System;

namespace DB.Diver
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DiverGame game = new DiverGame())
            {
                game.Run();
            }
        }
    }
}

