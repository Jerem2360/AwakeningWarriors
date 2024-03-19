using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AwakeningWarriors.Tileset
{
    internal class Serializer
    {
        private Stream stream;
        private XmlSerializer serializer;
        private object _cache;

        public TsxTileset Data
        {
            get
            {
                if ((this._cache != null) && (this._cache is TsxTileset))
                    return (TsxTileset)this._cache;
                return this.Load();
            }
        }
        public Serializer(Stream stream)
        {
            this.stream = stream;
            this.serializer = new XmlSerializer(typeof(TsxTileset));
            this._cache = null;
        }
        public TsxTileset Load()
        {
            return (TsxTileset)this.serializer.Deserialize(this.stream);
        }

    }

    [XmlRoot("tileset", Namespace = "", IsNullable = false)]
    public class TsxTileset
    {
        [XmlAttribute]
        public string version;
        [XmlAttribute]
        public string tiledversion;
        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public int tilewidth;
        [XmlAttribute]
        public int tileheight;
        [XmlAttribute]
        public int tilecount;
        [XmlAttribute]
        public int columns;
        public TsxImage image;
        public TsxTileset()
        {
            this.version = "1.0";
            this.tiledversion = "1.0";
            this.name = "";
            this.tilewidth = 0;
            this.tileheight = 0;
            this.tilecount = 0;
            this.columns = 0;
            this.image = new TsxImage();
        }
    }
    public class TsxImage
    {
        [XmlAttribute]
        public string source;
        [XmlAttribute]
        public string trans;
        [XmlAttribute]
        public int width;
        [XmlAttribute]
        public int height;

        public TsxImage()
        {
            this.source = "";
            this.trans = "ffffff";
            this.width = 0;
            this.height = 0;
        }
    }
}
