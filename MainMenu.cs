using AwakeningWarriors.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors
{
    class MainMenu
    {
        static Screen screen = new Screen("MainMenu");
        public static void Register()
        {
            Root.Window.AddScreen(screen);
        }
        public static void Prepare()
        {
            Root.Window.SetActiveScreen(screen);
            Root.Window.MenuTitle = "Main Menu";

            Graphics.Button PlayButton = new Graphics.Button("Play", 2, new System.Drawing.Size(120, 40), screen, OnPlayButtonClicked);
            PlayButton.AlignX = Alignment.Center;
            PlayButton.AlignY = Alignment.Center;
            //MainMenuBtn.UseHandCursor = true;
            PlayButton.ApplyChanges();
        }
        static void OnPlayButtonClicked(params object[] args)
        {
            Console.WriteLine("Starting game.");
            PlayerView.Prepare();
        }
    }
}
