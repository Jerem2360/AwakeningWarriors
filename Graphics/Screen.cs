using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.Graphics
{
    public class Screen : Container
    {
        private string name;
        public string Name => name;
        public override Size Size => Root.Window.ClientBounds.Size;
        public Screen(string name)
        {
            this.name = name;
        }

    }
}
