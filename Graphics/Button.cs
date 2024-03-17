using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AwakeningWarriors.Graphics
{
    public class Button : GraphicElement
    {
        public delegate void ClickCallback(params object[] args);

        private string text;
        private Bitmap img;
        private Bitmap hover_img;
        private long border_width;
        private ClickCallback click_callback;
        private object[] args;

        public bool UseHandCursor;

        private Button(Size size, Container owner, ClickCallback cb, object[] args) : base(size, owner)
        {
            this.border_width = 2;

            this.click_callback = cb;
            this.args = new object[0];
            if (args != null)
                this.args = args;

            this.UseHandCursor = false;

            Root.Window.AddEventHandler<MouseEventArgs>("Click", this, this.OnClick);
            Root.Window.AddEventHandler<EventArgs>("MouseEnter", this, this.OnMouseEnter);
            Root.Window.AddEventHandler<EventArgs>("MouseLeave", this, this.OnMouseLeave);
        }
        public Button(string text, Size size, Container owner, ClickCallback cb = null, object[] args = null) : this(size, owner, cb, args)
        {
            this.text = text;
            this.img = this.BakeDefaultImage();
            this.hover_img = this.BakeDefaultHoverImage();
        }
        public Button(Bitmap img, Container owner, ClickCallback cb = null, object[] args = null) : this(img.Size, owner, cb, args) 
        {
            this.text = "";
            this.img = this.hover_img = img;
        }
        public Button(Bitmap img, Bitmap hover_img, Container owner, ClickCallback cb = null, object[] args = null) : this(SizeMax(img.Size, hover_img.Size), owner, cb, args)
        {
            this.text = "";
            this.img = img;
            this.hover_img = hover_img;
        }
        private static Size SizeMax(Size ob1, Size ob2)
        {
            return new Size(Math.Max(ob1.Width, ob2.Width), Math.Max(ob1.Height, ob2.Height));
        }
        private void OnClick(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
            {
                this.click_callback(this.args);
            }
        }
        private Bitmap BakeDefaultImage()
        {
            Bitmap result = new Bitmap(this.Size.Width, this.Size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);

            double fontsize = this.Size.Width * this.Size.Height;
            fontsize = Math.Sqrt(fontsize) / 10;
            
            Font font = new Font("Helvetica", (float)fontsize, GraphicsUnit.Pixel);
            SizeF text_dims = g.MeasureString(this.text, font, this.Size.Width);

            double dw = this.Size.Width - text_dims.Width;
            double dh = this.Size.Height - text_dims.Height;

            long text_x = (long)(dw / 2);
            long text_y = (long)(dh / 2);

            Rectangle outer_rect = this.Rect;
            Rectangle inner_rect = new Rectangle(
                (int)this.border_width,
                (int)this.border_width,
                (int)(this.Rect.Width - (2 * this.border_width)),
                (int)(this.Rect.Height - (2 * this.border_width))
            );

            g.FillRectangle(new SolidBrush(Color.FromArgb(0x70, 0x70, 0xFF)), outer_rect);
            g.FillRectangle(new SolidBrush(Color.FromArgb(0x50, 0x50, 0xFF)), inner_rect);
            g.DrawString(this.text, font, new SolidBrush(Color.White), new RectangleF(text_x, text_y, text_dims.Width, text_dims.Height));

            return result;
        }
        private Bitmap BakeDefaultHoverImage()
        {
            Bitmap result = new Bitmap(this.Size.Width, this.Size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);

            double fontsize = this.Size.Width * this.Size.Height;
            fontsize = Math.Sqrt(fontsize) / 10;

            Font font = new Font("Helvetica", (float)fontsize, GraphicsUnit.Pixel);
            SizeF text_dims = g.MeasureString(this.text, font, this.Size.Width);

            double dw = this.Size.Width - text_dims.Width;
            double dh = this.Size.Height - text_dims.Height;

            long text_x = (long)(dw / 2);
            long text_y = (long)(dh / 2);

            Rectangle outer_rect = this.Rect;
            Rectangle inner_rect = new Rectangle(
                (int)this.border_width,
                (int)this.border_width,
                (int)(this.Rect.Width - (2 * this.border_width)),
                (int)(this.Rect.Height - (2 * this.border_width))
            );

            g.FillRectangle(new SolidBrush(Color.FromArgb(0x90, 0x90, 0xFF)), outer_rect);
            g.FillRectangle(new SolidBrush(Color.FromArgb(0x70, 0x70, 0xFF)), inner_rect);
            g.DrawString(this.text, font, new SolidBrush(Color.White), new RectangleF(text_x, text_y, text_dims.Width, text_dims.Height));

            return result;
        }
        protected internal override void Draw(System.Drawing.Graphics g, Rectangle parentArea)
        {
            base.Update();
            if (this.hovered)
            {
                g.DrawImage(this.hover_img, this.x, this.y);
            } else
            {
                g.DrawImage(this.img, this.x, this.y);
            }
            base.Draw(g, parentArea);
        }
        private void OnMouseEnter(object sender, EventArgs e)
        {
            if (this.UseHandCursor)
                Root.Window.Cursor = Cursors.Hand;
        }
        private void OnMouseLeave(object sender, EventArgs e) 
        {
            Root.Window.Cursor = Cursors.Default;
        }
    }
}
