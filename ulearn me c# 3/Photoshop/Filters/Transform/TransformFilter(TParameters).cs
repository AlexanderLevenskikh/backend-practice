using System;
using System.Drawing;

namespace MyPhotoshop
{
    public class TransformFilter<TParameters> : ParametrizedFilter<TParameters> 
        where TParameters : IParameters, new()
    {
        private string name;
        private ITransformer<TParameters> transformer;

        public TransformFilter(string name, ITransformer<TParameters> transformer)
        {
            this.name = name;
            this.transformer = transformer;
        }

        public override Photo Process(Photo original, TParameters parameters)
        {
            var oldSize = new Size(original.width, original.height);
            transformer.Prepare(oldSize, parameters);
            var result = new Photo(transformer.ResultSize.Width, transformer.ResultSize.Height);
            for (var x = 0; x < transformer.ResultSize.Width; x++)
            for (var y = 0; y < transformer.ResultSize.Height; y++)
            {
                var point = new Point(x, y);
                var oldPoint = transformer.PointMap(point);
                if (oldPoint != null)
                    result[x, y] = original[oldPoint.Value.X, oldPoint.Value.Y];
            }

            return result;
        }

        public override string ToString()
        {
            return name;
        }
    }
}