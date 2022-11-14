using System;
using System.Drawing;

namespace MagPend
{
    internal class Renderer
    {
        public readonly float dt = 0.025f;
        public readonly int numSteps = 2;
        public readonly Color[] colors = new Color[] {
            Color.FromArgb(243, 198, 119),
            Color.FromArgb(12, 10, 62),
            Color.FromArgb(249, 86, 79),
            Color.FromArgb(179, 63, 98),
            Color.FromArgb(123, 30, 122)
        };

        public Vector[] mags;
        public float[] dim = new float[4];
        public float mu, g, h;
        public int width, height, size;

        public Pendulum[] pendulums;
        
        public Renderer(float mu, float g, float h, int width, int height, int numMags)
        {
            // store param values
            this.mu = mu;
            this.g = g;
            this.h = h;
            this.width = width;
            this.height = height;

            // calc scaling factors
            float ratio = (float)height / width;
            this.size = width * height;
            this.dim[0] = 7;
            this.dim[1] = 3.5f;
            this.dim[2] = ratio * 7;
            this.dim[3] = ratio * 3.5f;

            this.InitPendulums();

            // generate magnet positions
            float t;

            mags = new Vector[numMags];
            for (int i = 0; i < numMags; i ++)
            {
                t = (2 * MathF.PI * i) / numMags;
                mags[i] = new Vector(MathF.Cos(t), MathF.Sin(t));
            }
        }

        public void InitPendulums()
        {
            // Initiate pendulums
            pendulums = new Pendulum[size];

            int k = 0;
            float x, y;
            for(int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    x = dim[0] * ((float)i / width) - dim[1];
                    y = dim[2] * ((float)j / height) - dim[3];

                    pendulums[k] = new Pendulum(x, y);
                    k++; 
                }
            }
        }

        public Color GetClosest(Vector p)
        {
            float v;
            float m = p.DistSq(mags[0]);
            int i = 0;
            for (int j = 0; j < mags.Length; j++)
            {
                v = p.DistSq(mags[j]);
                if (v < m)
                {
                    m = v;
                    i = j;
                }
            }

            return colors[i];
        }

        public void Update(Pendulum p)
        {
            // Update the pendulums
            Vector aN, f, k, r;
            float s;

            for (int i = 0; i < numSteps; i++)
            {
                // Integrate the equations using Beeman integration (first part)
                p.p += dt * p.v + (dt * dt / 6.0f) * ((4.0f * p.a) - p.aP);

                // calculate influence on force from magnets
                aN = new(0, 0);
                foreach (Vector mag in mags)
                {
                    r = mag - p.p;
                    s = MathF.Pow(h * h + r.Dot(r), 1.5f);
                    aN += r / s;
                }

                // add other forces
                f = mu * p.v;
                k = g * p.p;
                p.a -= f + k;

                // integrate equations of motion (second part)
                p.v += (dt / 6.0f) * ((2.0f * aN) + (5.0f * p.a) - p.aP);
                p.aP = p.a;
                p.a = aN;
            }
        }

        public Bitmap RenFrame()
        {
            Bitmap b = new(width, height);

            Color col;
            Pendulum pend;
            int k = 0;
            for (int j = 0; j < height; j ++)
            {
                for (int i = 0; i < width; i ++)
                {
                    pend = pendulums[k];
                    Update(pend);
                    col = GetClosest(pend.p);

                    b.SetPixel(i, j, col);
                    k++;
                }
            }
            return b;
        }
    }
}
