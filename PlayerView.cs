using AwakeningWarriors.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors
{
    public class PlayerView
    {
        static Screen screen = new Screen("PlayerView");

        public static void Register()
        {
            Root.Window.AddScreen(screen);
        }
        public static void Prepare()
        {
            Root.Window.SetActiveScreen(screen);

            Console.WriteLine("Loading maps...");
            Map exterior = new Map("exterior", screen);
            exterior.AlignX = Alignment.Center;
            exterior.AlignY = Alignment.Center;
            exterior.Zoom = 1.85f;
        }
    }
}
