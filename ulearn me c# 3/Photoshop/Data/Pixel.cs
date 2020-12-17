using System;

namespace MyPhotoshop
{
    public struct Pixel
    {
        private double r;
        private double g;
        private double b;

        public Pixel(double r, double g, double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public double R
        {
            get { return r; }
            set { r = Check(value); }
        }

        public double G
        {
            get { return g; }
            set { g = Check(value); }
        }

        public double B
        {
            get { return b; }
            set { b = Check(value); }
        }

        private double Check(double value)
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public static Pixel operator *(Pixel pixel, double number)
        {
            return new Pixel
            {
                r = pixel.r * number,
                g = pixel.g * number,
                b = pixel.b * number
            };
        }
        
        public static Pixel operator *(double number, Pixel pixel)
        {
            return pixel * number;
        }
    }
}