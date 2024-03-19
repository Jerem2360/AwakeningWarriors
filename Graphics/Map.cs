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
        private _MapReader _mapReader;
        private float zoom;

        public float Zoom
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
        }

        protected internal override void Draw(_Graphics g, Rectangle parentArea)
        {
            base.Update();

            for (int i = 0; i < this._mapReader.LayerCount; i++)  // loop over each layer
            {
                g.DrawImage(this._mapReader.GetLayer(i), parentArea.X, parentArea.Y, this._mapReader.PixelSize.Width * this.zoom, this._mapReader.PixelSize.Height * this.zoom);
            }

            base.Draw(g, parentArea);
        }
    }
}
