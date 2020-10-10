using System.Collections.Generic;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			DataPoint prevData = null;

			foreach (var currentData in data)
			{
				if (prevData == null)
				{
					currentData.ExpSmoothedY = currentData.OriginalY;
				}
				else
				{
					currentData.ExpSmoothedY = alpha * currentData.OriginalY + (1 - alpha) * prevData.ExpSmoothedY;
				}

				prevData = currentData;
				yield return currentData;
			}
		}
	}
}