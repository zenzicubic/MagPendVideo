namespace MagPend
{
    internal class Vector {
        public float x, y;
        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector operator +(Vector a, Vector b) => new(a.x + b.x, a.y + b.y);
        public static Vector operator -(Vector a, Vector b) => new(a.x - b.x, a.y - b.y);
        public static Vector operator *(float a, Vector b) => new(b.x * a, b.y * a);
        public static Vector operator /(Vector b, float a) => new(b.x / a, b.y / a);

        public float Dot(Vector v)
        {
            return this.x * v.x + this.y * v.y;
        }

        public float DistSq(Vector v)
        {
            Vector w = this - v;
            return w.Dot(w);
        }
    }
}
