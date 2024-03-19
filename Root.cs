using AwakeningWarriors.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwakeningWarriors
{
    using _Form = System.Windows.Forms.Form;
    using _ControlStyles = System.Windows.Forms.ControlStyles;
    using _PaintEventArgs = System.Windows.Forms.PaintEventArgs;
    using _MouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using _MouseEventHandler = System.Windows.Forms.MouseEventHandler;
    using _Point = System.Drawing.Point;

    public class Root
    {
        private static string title_prefix = "Awakening Warriors";
        public static Root Window = new Root();

        private class _MyForm : _Form
        {
            public void OptimizeBuffer()
            {
                this.SetStyle(_ControlStyles.OptimizedDoubleBuffer | _ControlStyles.AllPaintingInWmPaint | _ControlStyles.UserPaint, true);
            }
        }
        public delegate void Handler<T>(object sender, T e) where T : EventArgs;
        private delegate void _Handler(object sender, EventArgs e);

        private _MyForm _form;

        private string _menutitle;
        private bool visible;
        private List<Graphics.Screen> screens;
        private int active_screen;
        private Dictionary<long, List<EventHandler>> mouse_hover_handlers;
        private Dictionary<long, List<EventHandler>> mouse_enter_handlers;
        private Dictionary<long, List<EventHandler>> mouse_leave_handlers;
        private Dictionary<long, List<EventHandler>> click_handlers;

        public string MenuTitle
        {
            get => this._menutitle; set { 
                this._menutitle = value; 
                if (value.Length > 0) {
                    this._form.Text = title_prefix + " - " + value;
                } else
                {
                    this._form.Text = title_prefix;
                }
            }
        }
        public Graphics.Screen ActiveScreen
        {
            get => this.active_screen >= 0 ? this.screens[this.active_screen] : null; 
            set
            {
                if (value == null)
                {
                    this.active_screen = -1;
                }
                int index = this.screens.FindIndex((Graphics.Screen cmp) => cmp.Name == value.Name);
                if (index >= 0)
                {
                    this.active_screen = index;
                }
            }
        }
        public _Point MousePosition => this._form.PointToClient(_MyForm.MousePosition);
        internal Rectangle ClientBounds => this._form.ClientRectangle;
        public Cursor Cursor {
            get => this._form.Cursor;
            set => this._form.Cursor = value;
        }
        public Size Dimensions
        {
            get => new Size(this._form.Bounds.Width, this._form.Bounds.Height);
            set => this._form.Bounds = new Rectangle(this._form.Bounds.Location, value);
        }

        public Root()
        {
            this._form = new _MyForm();
            this._form.OptimizeBuffer();
            this.MenuTitle = "";
            this.visible = false;
            this.screens = new List<Graphics.Screen>();
            this.ActiveScreen = null;

            this.mouse_hover_handlers = new Dictionary<long, List<EventHandler>>();
            this.mouse_enter_handlers = new Dictionary<long, List<EventHandler>>();
            this.mouse_leave_handlers = new Dictionary<long, List<EventHandler>>();
            this.click_handlers = new Dictionary<long, List<EventHandler>>();

            this._form.Paint += this.OnPaint;
            this._form.MouseMove += this.OnHover;
            this._form.MouseClick += this.OnClick;
        }
        public void AddScreen(Graphics.Screen screen)
        {
            this.screens.Add(screen);
        }
        public void SetActiveScreen(Graphics.Screen screen)
        {
            int where = this.screens.FindIndex((Graphics.Screen cmp) => cmp.Name == screen.Name);
            if (where < 0)
                return;
            this.active_screen = where;
            this._form.Invalidate();
        }
        private void AddSpecialEventHandler_MouseHover(EventHandler handler, GraphicElement target)
        {
            if (!this.mouse_hover_handlers.ContainsKey(target.id))
            {
                this.mouse_hover_handlers.Add(target.id, new List<EventHandler>());
            }
            this.mouse_hover_handlers[target.id].Add(handler);
        }
        private void AddSpecialEventHandler_MouseEnter(EventHandler handler, GraphicElement target)
        {
            if (!this.mouse_enter_handlers.ContainsKey(target.id))
            {
                this.mouse_enter_handlers.Add(target.id, new List<EventHandler>());
            }
            this.mouse_enter_handlers[target.id].Add(handler);
        }
        private void AddSpecialEventHandler_MouseLeave(EventHandler handler, GraphicElement target)
        {
            if (!this.mouse_leave_handlers.ContainsKey(target.id))
            {
                this.mouse_leave_handlers.Add(target.id, new List<EventHandler>());
            }
            this.mouse_leave_handlers[target.id].Add(handler);
        }
        private void AddSpecialEventHandler_Click(EventHandler handler, GraphicElement target)
        {
            if (!this.click_handlers.ContainsKey(target.id))
            {
                this.click_handlers.Add(target.id, new List<EventHandler>());
            }
            this.click_handlers[target.id].Add(handler);
        }
        private EventHandler _MakeEventHandler<T>(Handler<T> handler) where T : EventArgs
        {
            return new EventHandler((object sender, EventArgs e) => { 
                if (e is T t) 
                    handler(sender, t); 
            });
        }
        public bool AddEventHandler<T>(string name, GraphicElement target, Handler<T> handler) where T : EventArgs
        {
            switch (name)
            {
                case "Paint":
                    return false;

                case "MouseEnter":
                    this.AddSpecialEventHandler_MouseEnter(this._MakeEventHandler(handler), target);
                    return true;

                case "MouseLeave":
                    this.AddSpecialEventHandler_MouseLeave(this._MakeEventHandler(handler), target);
                    return true;

                case "MouseHover":
                    this.AddSpecialEventHandler_MouseHover(this._MakeEventHandler(handler), target);
                    return true;

                case "Click":
                    this.AddSpecialEventHandler_Click(this._MakeEventHandler(handler), target);
                    return true;
            }

            EventInfo _event = this._form.GetType().GetEvent(name);


            if (_event == null)
                return false;

            _event.AddEventHandler(this._form, this._MakeEventHandler(handler));
            return true;
        }
        public void Show(bool async = false)
        {
            if (this.visible)
                return;

            if (async)
            {
                this._form.Show();
            } else
            {
                this._form.ShowDialog();
            }
            this.visible = true;
        }
        public void Close()
        {
            if (!this.visible) 
                return;

            this._form.Close();
        }
        private void OnPaint(object sender, _PaintEventArgs e)
        {
            if (this.ActiveScreen == null)
                return;

            this.ActiveScreen.Draw(e.Graphics, e.ClipRectangle);
        }
        internal void OnHover(object sender, EventArgs e)
        {
            this.ActiveScreen.ForAllChildren((GraphicElement elem) => this.MakeHoverChecks(sender, elem));
        }
        private void MakeHoverChecks(object sender, Graphics.GraphicElement target)
        {
            if (target.Rect.Contains(this.MousePosition))
            {
                if (!target.hovered)
                {
                    target.hovered = true;
                    this._form.Invalidate(target.Rect);

                    // call MouseEnter event handlers
                    if (this.mouse_enter_handlers.ContainsKey(target.id))
                    {
                        foreach (EventHandler handler in this.mouse_enter_handlers[target.id])
                        {
                            handler(sender, EventArgs.Empty);
                        }
                    }
                }
                // call MouseHover event handlers
                if (this.mouse_hover_handlers.ContainsKey(target.id))
                {
                    foreach (EventHandler handler in this.mouse_hover_handlers[target.id])
                    {
                        handler(sender, EventArgs.Empty);
                    }
                }

            } else
            {
                if (target.hovered)
                {
                    target.hovered = false;
                    this._form.Invalidate(target.Rect);

                    // call MouseLeave event handlers
                    if (this.mouse_leave_handlers.ContainsKey(target.id))
                    {
                        foreach (EventHandler handler in this.mouse_leave_handlers[target.id])
                        {
                            handler(sender, EventArgs.Empty);
                        }
                    }
                }
            }

            target.ForAllChildren((GraphicElement elem) => this.MakeHoverChecks(sender, elem));
        }
        internal void OnClick(object sender, _MouseEventArgs e)
        {
            this.ActiveScreen.ForAllChildren((GraphicElement elem) => this.PropagateClicks(sender, elem, e));
        }
        private void PropagateClicks(object sender, GraphicElement target, _MouseEventArgs e)
        {
            if (this.click_handlers.ContainsKey(target.id) && target.Rect.Contains(this.MousePosition))
            {
                foreach (EventHandler handler in this.click_handlers[target.id])
                {
                    handler(sender, e);
                }
            }

            target.ForAllChildren((GraphicElement elem) => this.PropagateClicks(sender, elem, e));
        }
    }
}
