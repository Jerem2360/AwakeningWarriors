using AwakeningWarriors.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwakeningWarriors
{
    internal class Program
    {
        static Graphics.Screen MainMenu = new Graphics.Screen("MainMenu");
        static void Main(string[] args)
        {

            Root.Window.AddScreen(MainMenu);


            SetupMainMenu();


            Root.Window.Show();
        }
        static void OnPlayButtonClicked(params object[] args)
        {
            Console.WriteLine("Play was clicked");
        }
        static void SetupMainMenu()
        {
            Root.Window.SetActiveScreen(MainMenu);
            Root.Window.MenuTitle = "Main Menu";

            Graphics.Button MainMenuBtn = new Graphics.Button("Play ball because this text must be very very loooong ...", new System.Drawing.Size(150, 100), MainMenu, OnPlayButtonClicked);
            MainMenuBtn.AlignX = Alignment.Center;
            MainMenuBtn.AlignY = Alignment.Center;
            MainMenuBtn.UseHandCursor = true;
            MainMenuBtn.ApplyChanges();

        }
    }
}
