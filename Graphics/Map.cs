using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.Graphics
{
    using _MapReader = MapReader.Map;
    using _Graphics = System.Drawing.Graphics;
    public class Map : GraphicElement
    {
        internal enum Direction
        {
            Left,
            Right, 
            Top, 
            Bottom
        }

        public static double ZoomSpeed = 30.0d;

        private _MapReader _mapReader;
        private double zoom;
        private Point view_coordinates;
        private int view_movement_speed;

        public double Zoom
        {
            get => this.zoom;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Zoom cannot be negative or be 0.");
                this.zoom = value;
            }
        }
        public Map(string source, Container owner) : base(_MapReader.Get(source).PixelSize, owner)
        {
            this.zoom = 1;
            this._mapReader = _MapReader.Get(source);
            this.view_coordinates = new Point(0, 0);
            this.view_movement_speed = 16;
        }
        protected internal override void Draw(_Graphics g, Rectangle parentArea)
        {
            base.Update();

            // apply the zoom level:
            float w = (float)(this._mapReader.PixelSize.Width * this.zoom);
            float h = (float)(this._mapReader.PixelSize.Height * this.zoom);

            // center the map:
            int x = (int)(this.x + ((this.Owner.Size.Width - w) / 2));
            int y = (int)(this.y + ((this.Owner.Size.Height - h) / 2));

            // take in account the coordinates of the user view, adjusted with the zoom level:
            x += (int)(this.view_coordinates.X * this.zoom);
            y += (int)(this.view_coordinates.Y * this.zoom);

            for (int i = 0; i < this._mapReader.LayerCount; i++)  // loop over each layer
            {
                g.DrawImage(this._mapReader.GetLayer(i), x, y, w, h);
            }

            base.Draw(g, parentArea);
        }
        public void ZoomIn(int delta)
        {
            this.zoom *= 1 + (1.0d / (1/ZoomSpeed * delta));
        }
        public void ZoomOut(int delta)
        {
            this.zoom *= 1 - (1.0d / (1/ZoomSpeed * delta));
        }
        /// <summary>
        /// Move the user view by the given number of pixels in the given direction.
        /// This is equivalent to moving the map the same amount of pixels in the 
        /// opposite direction. 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="direction"></param>
        internal void MoveView(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    this.view_coordinates.X += this.view_movement_speed;
                    break;

                case Direction.Right:
                    this.view_coordinates.X -= this.view_movement_speed;
                    break;

                case Direction.Top:
                    this.view_coordinates.Y += this.view_movement_speed;
                    break;

                case Direction.Bottom:
                    this.view_coordinates.Y -= this.view_movement_speed;
                    break;
            }
        }
    }
}
