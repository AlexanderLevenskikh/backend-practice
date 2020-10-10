using System;
using System.Collections.Generic;
using System.Drawing;
using GeometryTasks;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        public static readonly Dictionary<Segment, Color> Colors = new Dictionary<Segment, Color>();

        public static void SetColor(this Segment segment, Color color)
        {
            if (!Colors.ContainsKey(segment))
            {
                Colors.Add(segment, color);
            }
            else
            {
                Colors[segment] = color;
            }
        }

        public static Color GetColor(this Segment segment)
        {
            return Colors.ContainsKey(segment) ? Colors[segment] : Color.Black;
        }
    }
}