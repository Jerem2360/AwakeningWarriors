using AwakeningWarriors.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AwakeningWarriors
{
    using _MouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using _KeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using _Keys = System.Windows.Forms.Keys;
    using _Timer = System.Windows.Forms.Timer;
    public class PlayerView
    {
        static Screen screen = new Screen("PlayerView");
        public static Map exterior;
        private static _Timer update_timer;
        private static Dictionary<_Keys, bool> pressed_keys = new Dictionary<_Keys, bool>();

        public static void Register()
        {
            Root.Window.AddScreen(screen);
        }
        public static void Prepare()
        {
            Root.Window.SetActiveScreen(screen);

            Console.WriteLine("Loading maps...");
            exterior = new Map("exterior", screen);
            exterior.AlignX = Alignment.Center;
            exterior.AlignY = Alignment.Center;
            exterior.Zoom = 1.85f;
            Root.Window.AddEventHandler("MouseWheel", exterior, (object caller, _MouseEventArgs e) => { OnMouseWheel(e.Delta); });
            Root.Window.AddEventHandler("KeyDown", exterior, (object caller, _KeyEventArgs e) => { OnKeyDown(e.KeyCode, e.Alt, e.Control); });
            Root.Window.AddEventHandler("KeyUp", exterior, (object caller, _KeyEventArgs e) => { OnKeyUp(e.KeyCode, e.Alt, e.Control); });

            update_timer = new _Timer();
            update_timer.Interval = 1000 / 60;  // 60 fps
            update_timer.Tick += (object sender, EventArgs e) => { Tick(); };
            update_timer.Start();
        }
        public static void Cleanup()
        {
            if (update_timer.Enabled)
            {
                update_timer.Stop();
            }
        }
        private static bool IsKeyPressed(_Keys key)
        {
            return pressed_keys.ContainsKey(key) && pressed_keys[key];
        }
        public static void Tick()
        {
            if (IsKeyPressed(_Keys.Z))
            {
                exterior.MoveView(Map.Direction.Top);
            }
            else if (IsKeyPressed(_Keys.S))
            {
                exterior.MoveView(Map.Direction.Bottom);
            }
            if (IsKeyPressed(_Keys.Q))
            {
                exterior.MoveView(Map.Direction.Left);
            }
            else if (IsKeyPressed(_Keys.D))
            {
                exterior.MoveView(Map.Direction.Right);
            }
            Root.Window.Update();
        }
        public static void OnMouseWheel(int delta)
        {
            if (delta > 0)
            {
                exterior.ZoomIn(delta);
                return;
            }
            if (delta < 0)
            {
                exterior.ZoomOut(-delta);
            }
        }
        public static void OnKeyDown(_Keys keycode, bool alt, bool control)
        {
            pressed_keys[keycode] = true;
        }
        public static void OnKeyUp(_Keys keycode, bool alt, bool control) 
        {
            pressed_keys[keycode] = false;
        }
    }
}
