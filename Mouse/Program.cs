using System;

namespace Mouse
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Главная точка входа приложения.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

