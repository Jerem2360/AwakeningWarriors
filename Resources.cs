using AwakeningWarriors.Properties;
using Microsoft.SqlServer.Server;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors
{
    class IncorrectResourceTypeException : Exception
    {
        public IncorrectResourceTypeException(string message) : base(message) { }
    }
    class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message) { }
    }

    public abstract class Resource<T>
    {
        protected T handle;
        public T Data => this.handle;
        protected Resource() { this.handle = default(T); }
        protected Resource(T handle) { this.handle = handle; }

        public Resource(Resource<object> source)
        {
            this.handle = (T)source.handle;
        }

    }
    public abstract class Resource : Resource<object>
    {

    }
    public class ImageResource : Resource<Bitmap>
    {
        private ImageResource(Bitmap source) : base(source) { }
        public ImageResource(ResourceManager mgr, string name) : base((Bitmap)mgr.GetObject(name)) {}
        public static ImageResource LoadFromDisk(string path)
        {
            return new ImageResource(new Bitmap(path));
        }
    }
    public class StringResource : Resource<string>
    {
        public StringResource(ResourceManager mgr, string name) : base(mgr.GetString(name)) { }
    }
    public class FileResource : Resource<Stream>
    {
        private bool need_close = false;
        private FileResource(Stream source) : base(source) { }
        public FileResource(ResourceManager mgr, string name) : base(new MemoryStream((byte[])mgr.GetObject(name))) { }
        public static FileResource LoadFromDisk(string path, FileMode mode)
        {
            Stream fs = File.Open(path, mode);
            FileResource res = new FileResource(fs);
            res.need_close = true;
            return res;
        }
        ~FileResource()
        {
            if (this.need_close) { this.handle.Close(); this.need_close = false; };
        }
    }
    static class Resources
    {
        private delegate RT ResourceInitializer<RT>(ResourceManager mgr, string name);
        private enum RType
        {
            None,
            Image,
            String,
            File,
        }
        private static Dictionary<string, RType> rtype_cache = new Dictionary<string, RType>();
        private static Dictionary<string, object> resource_cache = new Dictionary<string, object>();

        public static ResourceManager ResourceManager => _resources.ResourceManager;

        private static object CheckCache(string name, RType rt)
        {
            if (rtype_cache.ContainsKey(name) && resource_cache.ContainsKey(name))
            {
                if (rtype_cache[name] == RType.None)
                {
                    throw new ResourceNotFoundException($"Could not find resource '{name}'.");
                }
                if (rtype_cache[name] != rt)
                {
                    throw new IncorrectResourceTypeException($"This resource is not of type '{rt}'. It is of type '{rtype_cache[name]}'.");
                }
                return resource_cache[name];
            }
            return null;
        }
        private static void RegisterToCache(string name, object resource, RType rt)
        {
            resource_cache[name] = resource;
            rtype_cache[name] = rt;
        }
        private static Resource<T> FindResource<RT, T>(string name, RType rt, ResourceInitializer<RT> init) where RT : Resource<T>
        {
            object cache_res = CheckCache(name, rt);
            if (!ReferenceEquals(cache_res, null))
            {
                return (RT)cache_res;
            }
            Resource<T> res = init(ResourceManager, name);
            RegisterToCache(name, res, rt);
            return res;
        }

        public static Resource<Bitmap> GetImage(string name)
        {
            return FindResource<ImageResource, Bitmap>(name, RType.Image,
                    (ResourceManager mgr, string _name) => new ImageResource(mgr, _name)
                );
        }
        public static Resource<string> GetString(string name)
        {
            return FindResource<StringResource, string>(name, RType.String,
                    (ResourceManager mgr, string _name) => new StringResource(mgr, _name)
                );
        }
        public static Resource<Stream> GetFileStream(string name)
        {
            return FindResource<FileResource, Stream>(name, RType.File,
                    (ResourceManager mgr, string _name) => new FileResource(mgr, _name)
                );
        }
    }
}
