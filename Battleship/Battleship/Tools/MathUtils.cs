using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Asteroid.Tools
{
    /**
     * Simple class for simple math calculations
     */
    public class MathUtils
    {
        public static Random rnd = new Random();

        public static float random(float max)
        {
            return (float)rnd.NextDouble() * max;
        }

        public static float random()
        {
            return (float)rnd.NextDouble();
        }

        public static float random(float min, float max)
        {
            return min + (float)rnd.NextDouble() * (max - min);
        }

        public static int random(int max)
        {
            return rnd.Next(max);
        }

        public static int random(int min, int max)
        {
            return min + rnd.Next(0, max - min + 1);
        }

        public static bool randomBool()
        {
            return rnd.Next(2) == 1 ? true : false;
        }

        public static float atan(float degree)
        {
            return (float)Math.Atan((double)degree);
        }

        /**
         * Clamps the vector within target range
         */
        public static void clamp(Vector2 vec, float min, float max)
        {
            if (vec.X < min) vec.X = min;
            else if (vec.X > max) vec.X = max;

            if (vec.Y < min) vec.Y = min;
            else if (vec.Y > max) vec.Y = max;
        }
    }
}
