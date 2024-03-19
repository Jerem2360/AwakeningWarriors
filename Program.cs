using AwakeningWarriors.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AwakeningWarriors
{
    internal class Program
    {
        static void Main(string[] args)
        {

            MainMenu.Register();
            PlayerView.Register();

            MainMenu.Prepare();

            Root.Window.Dimensions = new Size(1366, 768);
            Root.Window.Show();
        }
    }
}
