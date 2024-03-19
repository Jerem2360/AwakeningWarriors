using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.Graphics
{
    using _Graphics = System.Drawing.Graphics;
    public class ImageBox : GraphicElement
    {
        private Image img;
        public ImageBox(Image img, double scale, Container owner) : base(new Size((int)(img.Size.Width * scale), (int)(img.Size.Height * scale)), owner)
        {
            this.img = img;
        }
        protected internal override void Draw(_Graphics g, Rectangle parentArea)
        {
            base.Update();

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(img, this.x, this.y, this.Size.Width, this.Size.Height);

            base.Draw(g, parentArea);
        }
    }
}
