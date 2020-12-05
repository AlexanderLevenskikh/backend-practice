namespace MyPhotoshop
{
    public class Photo
    {
        public readonly int width;
        public readonly int height;
        private readonly Pixel[,] data;

        public Pixel this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; }
        }

        public Photo(int width, int height)
        {
            this.width = width;
            this.height = height;
            data = new Pixel[width, height];
        }
    }
}