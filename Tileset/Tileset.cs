using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.Tileset
{
    using _Graphics = System.Drawing.Graphics;
    /// <summary>
    /// Represents a tileset as an array of bitmaps.
    /// Each tile is associated with a Bitmap.
    /// </summary>
    public class Tileset
    {
        private static Dictionary<string, Tileset> cache = new Dictionary<string, Tileset>();


        private Bitmap img;
        private Size tile_size;
        private int columns;
        private string name;
        private int tilecount;

        public string Name => this.name;
        public int TileCount => this.tilecount;

        private Tileset(string name)
        {
            Resource<Stream> fs_res = Resources.GetFileStream("tilesets.tsx." + name);
            Serializer serializer = new Serializer(fs_res.Data);
            TsxTileset data = serializer.Data;
            this.columns = data.columns;
            this.tile_size = new Size(data.tilewidth, data.tileheight);
            this.name = name;
            this.tilecount = data.tilecount;

            Resource<Bitmap> img_res = Resources.GetImage(data.image.source);
            this.img = img_res.Data;

        }
        /// <summary>
        /// Retrieves the tileset identified by the given name.
        /// </summary>
        /// <param name="name">Name that identifies the requested tileset.</param>
        /// <returns>The tileset identified by the given name, or null if it was not found.</returns>
        public static Tileset Get(string name)
        {
            if (cache.ContainsKey(name))
            {
                return cache[name];
            }
            Tileset result;
            result = new Tileset(name);
            cache.Add(name, result);
            return result;
        }
        private Bitmap GetImage(int index)
        {
            int x = index % this.columns;
            int y = (index - x) / this.columns;
            Rectangle rect = new Rectangle(x * this.tile_size.Width, y * this.tile_size.Height, this.tile_size.Width, this.tile_size.Height);
            return this.img.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        /// <summary>
        /// Retrieves the Tile at the given index.
        /// </summary>
        public Tile this[int index]
        {
            get
            {
                return new Tile(this.GetImage(index));
            }
        }
    }
    public class Tile
    {
        [Flags]
        public enum FlipDirection: uint
        {
            None = 0,
            Horizontal = 0x80000000,
            Vertical = 0x40000000,
            Diagonal = 0x20000000,
            AllDirections = Horizontal | Vertical | Diagonal,
            IgnoredFlag = 0x10000000,
            All = AllDirections | IgnoredFlag,
        }

        private Bitmap bitmap;
        public int X, Y;
        public float Scale;
        public Rectangle Rect => new Rectangle(this.X, this.Y, (int)(this.bitmap.Width * this.Scale), (int)(this.bitmap.Height * this.Scale));
        public Bitmap Bitmap => bitmap;

        internal Tile(Bitmap img)
        {
            this.bitmap = img;
            this.X = this.Y = 0;
            this.Scale = 1;
        }
        private Tile(int width, int height)
        {
            this.bitmap = new Bitmap(width, height);
            this.X = this.Y = 0;
            this.Scale = 1;
        }
        internal void Draw(_Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(this.bitmap, this.X, this.Y, this.bitmap.Width * this.Scale, this.bitmap.Height * this.Scale);
        }
        public void Flip(FlipDirection direction)
        {
            switch (direction)
            {
                case FlipDirection.Horizontal:
                    this.bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX); 
                    break;

                case FlipDirection.Vertical:
                    this.bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;

                case FlipDirection.Diagonal:
                    this.bitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
            }
        }
        public static Tile Empty(int width, int height)
        {
            return new Tile(width, height);
        }
    }
}
