using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.MapReader
{
    using _Graphics = System.Drawing.Graphics;
    internal class Map
    {
        private class _TilesetInfo
        {
            public string Name;
            public long FirstGID;

            public Tileset.Tileset TileSet => Tileset.Tileset.Get(this.Name); 

            public _TilesetInfo(string name, long firstGID)
            {
                this.Name = name;
                this.FirstGID = firstGID;
            }
        }

        internal class _Layer
        {
            private List<long> tiles;
            private string name;
            private Map map;
            
            public _Layer(TmxLayer hLayer, in Map map)
            {
                this.name = hLayer.name;
                this.tiles = new List<long>();

                string[] data = hLayer.data.Split(new string[] { "," }, StringSplitOptions.None);

                int len = data.Length;
                string plural = len <= 1 ? "" : "s";
                Console.WriteLine($" {hLayer.width}x{hLayer.height} for total of {len} tile{plural}.");

                foreach (string s in data)
                {
                    long num = 0;
                    if (!long.TryParse(s, out num))
                    {
                        Console.WriteLine($"Warning: found non-integer tile GID {s}, falling back to 0.");
                    }
                    this.tiles.Add(num);
                }

                this.map = map;
            }
            public Tileset.Tile At(int x, int y)
            {
                int index = (int)(y * this.map.ncols + x);
                long gid = this.tiles[index];

                Tileset.Tile.FlipDirection direction = (Tileset.Tile.FlipDirection)(gid & (long)Tileset.Tile.FlipDirection.AllDirections);

                gid &= ~(long)Tileset.Tile.FlipDirection.All;

                if (gid <= 0)
                {
                    return Tileset.Tile.Empty(this.map.TileSize.Width, this.map.TileSize.Height);
                }
                gid -= 1;

                long offset = 0;

                Tileset.Tileset tileset;

                for (int i = 0; i < this.map.tsx_info.Count; i++)
                {
                    if (
                        (gid < this.map.tsx_info[i].FirstGID + this.map.tsx_info[i].TileSet.TileCount)
                    )
                    {
                        tileset = this.map.tsx_info[i].TileSet;
                        int lid = (int)(gid - offset);
                        Tileset.Tile res = tileset[lid];
                        res.Flip(direction);
                        return res;
                    }
                    offset += this.map.tsx_info[i].TileSet.TileCount;
                }
                Console.WriteLine($"Warning: found tile GID {gid} which is out of range. Falling back to an empty tile.");
                return Tileset.Tile.Empty(this.map.TileSize.Width, this.map.TileSize.Height);
            }
            public Bitmap Assemble()
            {
                Bitmap result = new Bitmap(this.map.Size.Width * this.map.TileSize.Width, this.map.Size.Height * this.map.TileSize.Height);
                _Graphics g = _Graphics.FromImage(result);
                
                for (int j = 0; j < this.map.Size.Width; j++)  // loop over all the rows of a given layer
                {
                    for (int k = 0; k < this.map.Size.Height; k++)  // loop over the tiles of a given row
                    {
                        Tileset.Tile tile = this.At(k, j);
                        tile.X = (int)(this.map.TileSize.Width * k);
                        tile.Y = (int)(this.map.TileSize.Height * j);
                        tile.Draw(g);
                        //Console.WriteLine($"X = {tile.X}; Y = {tile.Y}");
                    }
                }
                g.Flush();
                return result;
            }
        }

        private static Dictionary<string, Map> cache = new Dictionary<string, Map>();

        private List<_TilesetInfo> tsx_info;
        private Size tile_size;
        private long nrows;
        private long ncols;
        private List<Bitmap> layers;

        public Size Size => new Size((int)ncols, (int)nrows);
        public Size PixelSize => new Size((int)(this.ncols * this.tile_size.Width), (int)(this.nrows * this.tile_size.Height));
        public long LayerCount => this.layers.Count;
        public Size TileSize => tile_size;

        private Map(string name) {
            Console.WriteLine($"Searching for resources of map 'maps.tmx.{name}'...");
            Resource<Stream> fs_res = Resources.GetFileStream("maps.tmx." + name);
            Serializer serializer = new Serializer(fs_res.Data);
            TmxMap data = serializer.Data;


            int len = data.tilesets.Count;
            string plural = len <= 1 ? "" : "s";
            Console.WriteLine($"Found the {len} following tileset{plural}:");
            foreach ( var tileset in data.tilesets )
            {
                Console.WriteLine($"  tileset '{tileset.source}'");
            }

            len = data.layers.Count;
            plural = len <= 1 ? "" : "s";
            Console.WriteLine($"Found {len} layer{plural}.");

            this.tile_size = new Size(data.tile_width, data.tile_height);
            this.nrows = data.width;
            this.ncols = data.height;

            Console.WriteLine("Loading tilesets...");

            this.tsx_info = new List<_TilesetInfo>();
            foreach (TmxTileset tsx in data.tilesets)
            {
                Console.WriteLine($"  {tsx.source}: firstgid={tsx.firstgid}");
                
                this.tsx_info.Add(new _TilesetInfo(tsx.source, tsx.firstgid));
            }

            Console.WriteLine("Assembling layers ...");

            this.layers = new List<Bitmap>();
            foreach (TmxLayer layer in data.layers)
            {
                Console.Write($"  layer '{layer.name}': ");

                Bitmap _layer = new _Layer(layer, this).Assemble();
                this.layers.Add(_layer);
            }

            Console.WriteLine("Done.");

        }
        public Bitmap GetLayer(int layer)
        {
            return this.layers[layer];
        }
        public static Map Get(string name) { 
            if (cache.ContainsKey(name)) 
                return cache[name];
            Map result = new Map(name);
            cache.Add(name, result);
            return result;
        }
    }
}
