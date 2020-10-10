using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{

	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var windowPoints = new Queue<DataPoint>();
			var windowMaximums = new LinkedList<DataPoint>();
			
			foreach (var currentData in data)
			{
				windowPoints.Enqueue(currentData);
				
				if (windowPoints.Count > windowWidth)
				{
					windowMaximums.Remove(windowPoints.Dequeue());
				}

				var lastMaximum = windowMaximums.Last;
				while (lastMaximum != null && currentData.OriginalY > lastMaximum.Value.OriginalY)
				{
					lastMaximum = lastMaximum.Previous;
					windowMaximums.RemoveLast();
				}

				windowMaximums.AddLast(currentData);

				currentData.MaxY = windowMaximums.First.Value.OriginalY;
				yield return currentData;
			}
		}
	}
}