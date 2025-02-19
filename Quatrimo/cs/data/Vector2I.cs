
using Microsoft.Xna.Framework;
using System;

namespace Quatrimo
{
    public struct Vector2I
    {
        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x { get; set; }
        public int y { get; set; }

        public static Vector2I zero = new Vector2I(0, 0);

        public static Vector2I operator +(Vector2I v1, int i)
        {
            return new Vector2I(v1.x + i, v1.y + i);
        }
        public static Vector2I operator *(Vector2I v1, int i)
        {
            return new Vector2I(v1.x * i, v1.y * i);
        }

        public static Vector2I operator +(Vector2I v1, Vector2I v2)
        {
            return new Vector2I(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2I operator -(Vector2I v1, Vector2I v2)
        {
            return new Vector2I(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2I operator *(Vector2I v1, Vector2I v2)
        {
            return new Vector2I(v1.x * v2.x, v1.y * v2.y);
        }

        public static bool operator ==(Vector2I v1, Vector2I v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Vector2I v1, Vector2I v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public static implicit operator Vector2(Vector2I v)
        {
            return new Vector2(v.x, v.y);
        }

        public static explicit operator Vector2I(Vector2 v)
        {
            return new Vector2I((int)v.X, (int)v.Y);
        }
    }
}