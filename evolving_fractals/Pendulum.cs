using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagPend
{
    internal class Pendulum
    {
        public Vector p, a, aP, aN, v;
        public Pendulum(float x, float y) {
            this.p = new(x, y);
            this.v = new(0, 0);
            this.a = new(0, 0);

            this.aP = new(0, 0);
            this.aN = new(0, 0);
        }
    }
}
