using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.Graphics
{
    using _Graphics = System.Drawing.Graphics;
    public abstract class Container
    {
        public delegate void ChildrenIterator(GraphicElement child);

        protected internal List<GraphicElement> children;
        public abstract Size Size { get; }
        protected Container()
        {
            this.children = new List<GraphicElement>();
        }
        public void ForAllChildren(ChildrenIterator iterator)
        {
            foreach (GraphicElement child in this.children)
            {
                iterator(child);
            }
        }
        public void AddChild(GraphicElement child)
        {
            this.children.Add(child);
        }
        protected internal virtual void Draw(_Graphics g, Rectangle parentArea)
        {
            foreach (GraphicElement child in this.children)
            {
                Rectangle rect = parentArea;
                rect.Intersect(child.Rect);
                child.Draw(g, rect);
            }
        }
    }
}
