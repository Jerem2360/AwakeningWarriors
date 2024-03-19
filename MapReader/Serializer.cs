using AwakeningWarriors.Tileset;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AwakeningWarriors.MapReader
{
    internal class Serializer
    {
        private Stream stream;
        private XmlSerializer serializer;
        private object _cache;

        public TmxMap Data
        {
            get
            {
                if ((this._cache != null) && (this._cache is TsxTileset))
                    return (TmxMap)this._cache;
                return this.Load();
            }
        }

        public Serializer(Stream stream)
        {
            this.stream = stream;
            this.serializer = new XmlSerializer(typeof(TmxMap));
            this._cache = null;
        }
        public TmxMap Load()
        {
            return (TmxMap)this.serializer.Deserialize(this.stream);
        }
    }

    [XmlRoot("map", Namespace = "", IsNullable = false)]
    public class TmxMap
    {
        [XmlElement("layer")]
        public List<TmxLayer> layers;
        [XmlElement("tileset")]
        public List<TmxTileset> tilesets;
        [XmlElement("objectgroup")]
        public List<TmxObjectGroup> obj_groups;
        [XmlAttribute]
        public string version;
        [XmlAttribute("tiledversion")]
        public string tiled_version;
        [XmlAttribute]
        public string orientation;
        [XmlAttribute("renderorder")]
        public string render_order;
        [XmlAttribute]
        public int width;
        [XmlAttribute]
        public int height;
        [XmlAttribute("tilewidth")]
        public int tile_width;
        [XmlAttribute("tileheight")]
        public int tile_height;
        [XmlAttribute]
        public int infinite;
        [XmlAttribute("nextlayerid")]
        public int next_layer_id;
        [XmlAttribute("nextobjectid")]
        public int next_object_id;

        public TmxMap()
        {
            this.layers = new List<TmxLayer>();
            this.tilesets = new List<TmxTileset>();
            this.obj_groups = new List<TmxObjectGroup>();
            this.version = "1.0";
            this.tiled_version = "1.0";
            this.orientation = "";
            this.render_order = "";
            this.width = 0;
            this.height = 0;
            this.tile_width = 0;
            this.tile_height = 0;
            this.infinite = 0;
            this.next_layer_id = 0;
            this.next_object_id = 0;
        }
        public override string ToString() => $"<TmxMap: layers={this.layers},tilesets={this.tilesets},objectgroups={this.obj_groups},version='{this.version}',tiledversion='{this.tiled_version}',orientation='{this.orientation}',renderorder='{this.render_order}',width={this.width},height={this.height},tilewidth={this.tile_width},tileheight={this.tile_height},infinite={this.infinite},nextlayerid={this.next_layer_id},nextobjectid={this.next_object_id}>";
    }

    public class TmxTileset
    {
        [XmlAttribute]
        public string source;
        [XmlAttribute]
        public int firstgid;

        public TmxTileset()
        {
            this.source = "";
            this.firstgid = 0;
        }
        public override string ToString() => $"<TmxTileset: source='{this.source}',firstgid={this.firstgid}>";
    }

    public class TmxLayer
    {
        [XmlAttribute]
        public int id;
        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public int width;
        [XmlAttribute]
        public int height;
        public string data;

        public TmxLayer()
        {
            this.id = 0;
            this.name = "";
            this.width = 0;
            this.height = 0;
            this.data = "";
        }
        public override string ToString() => $"<TmxLayer: id={this.id},name='{this.name}',width={this.width},height={this.height},data='{this.data}'>";
    }

    public class TmxObjectGroup
    {
        [XmlAttribute]
        public int id;
        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public int visible;
        [XmlElement("object")]
        public List<TmxObject> objects;

        public TmxObjectGroup()
        {
            this.id = 0;
            this.name = "";
            this.visible = 0;
            this.objects = new List<TmxObject>();
        }
        public override string ToString() => $"<TmxObjectGroup: id={this.id},name='{this.name}',visible={this.visible}, objects={this.objects}>";
    }

    public class TmxObject
    {
        [XmlAttribute]
        public int id;
        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public string type;
        [XmlAttribute]
        public double x;
        [XmlAttribute]
        public double y;
        [XmlAttribute]
        public double width;
        [XmlAttribute]
        public double height;

        public TmxObject()
        {
            this.id = 0;
            this.name = "";
            this.type = "";
            this.x = 0.0;
            this.y = 0.0;
            this.width = 0.0;
            this.height = 0.0;
        }
    };

}
