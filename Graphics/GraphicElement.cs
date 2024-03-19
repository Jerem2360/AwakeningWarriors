using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.Graphics
{
    using _Graphics = System.Drawing.Graphics;
    public abstract class GraphicElement : Container
    {
        private static long counter = 0;

        private Container owner;
        private long _x, _y;
        protected long x, y;
        private long _w, _h;
        private double _alignx, _aligny;

        private long x_cache, y_cache;
        private double alignx_cache, aligny_cache;

        internal bool hovered;
        internal long id;

        public long X
        {
            get => this._x; 
            set => this.x_cache = value;
        }
        public long Y
        {
            get => this._y; 
            set => this.y_cache = value;
        }
        public Alignment AlignX
        {
            get => this._alignx; 
            set => this.alignx_cache = value;
        }
        public Alignment AlignY
        {
            get => this._aligny; 
            set => this.aligny_cache = value;
        }

        public override Size Size => new Size((int)this._w, (int)this._h);
        public Rectangle Rect => new Rectangle(new Point((int)this.x, (int)this.y), this.Size);
        public GraphicElement(Size size, Container owner)
        {
            this.owner = owner;
            this.owner.AddChild(this);
            this._x = this.x = this.x_cache = this._y = this.y = this.y_cache = 0;
            this._w = size.Width;
            this._h = size.Height;
            this._alignx = this.alignx_cache = this._aligny = this.aligny_cache = -1;
            this.id = counter;
            counter++;
        }
        public void ApplyChanges()
        {
            this._x = this.x_cache;
            this._y = this.y_cache;
            this._alignx = this.alignx_cache;
            this._aligny = this.aligny_cache;
        }
        public void CancelChanges()
        {
            this.x_cache = this._x;
            this.y_cache = this._y;
            this.alignx_cache = this._alignx;
            this.aligny_cache = this._aligny;
        }
        private void UpdatePosWithAlignX()
        {
            /*
            Implement alignment on the X axis if required. 
            */
            if (this._alignx >= 0)
            {
                double alignx_factor = this._alignx / 2;
                double dw = this.owner.Size.Width - this.Size.Width;
                this.x = this._x + (long)(dw * alignx_factor);
            } else
            {
                this.x = this._x;
            }
        }
        private void UpdatePosWithAlignY()
        {
            /*
            Implement alignment on the Y axis if required. 
            */
            if (this._aligny >= 0)
            {
                double aligny_factor = this._aligny / 2;
                double dh = this.owner.Size.Height - this.Size.Height;
                this.y = this._y + (long)(dh * aligny_factor);
            }
        }
        protected void Update()
        {
            this.UpdatePosWithAlignX();
            this.UpdatePosWithAlignY();
        }
        protected internal override void Draw(_Graphics g, Rectangle parentArea)
        {
            Rectangle rect = this.Rect;
            rect.Intersect(parentArea);
            base.Draw(g, rect);
        }
    }
}
