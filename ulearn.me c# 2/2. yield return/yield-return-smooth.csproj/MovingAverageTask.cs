using System.Collections.Generic;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var windowQueue = new Queue<double>();
			double sum = 0;
			
			foreach (var currentData in data)
			{
				windowQueue.Enqueue(currentData.OriginalY);
				sum += currentData.OriginalY;
				
				if (windowQueue.Count > windowWidth)
				{
					sum -= windowQueue.Dequeue();
				}

				currentData.AvgSmoothedY = sum / windowQueue.Count;
				yield return currentData;
			}
		}
	}
}