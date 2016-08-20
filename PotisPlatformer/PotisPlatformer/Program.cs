using System;

namespace Platformer
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (XNA game = new XNA())
            {
                game.Run();
            }
        }
    }
#endif
}

