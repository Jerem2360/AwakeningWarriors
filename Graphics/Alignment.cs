using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwakeningWarriors.Graphics
{
    public struct Alignment
    {
        public static Alignment Default = -1;
        public static Alignment Minimum = 0;
        public static Alignment Center = 1;
        public static Alignment Maximum = 2;

        private double val;
        public Alignment(double val)
        {
            if ((val != -1) && ((val < 0) || (val > 2)))
            {
                throw new ArgumentException($"Expected a floating point number between 0 and 2, but got {val} instead.");
            }
            this.val = val;
        }
        public static implicit operator Alignment(double val) {  return new Alignment(val); }
        public static implicit operator double(Alignment val) { return val.val; }
    }
}
