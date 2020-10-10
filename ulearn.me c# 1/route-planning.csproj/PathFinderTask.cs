using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoutePlanning
{
	public static class PathFinderTask
	{
		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
			var shortestDistance = double.PositiveInfinity;
			var shortestOrder = new int[checkpoints.Length];
			var order = new int[checkpoints.Length];

			MakePermutations(order, 1, checkpoints, ref shortestDistance, ref shortestOrder);
			return shortestOrder;
		}

		private static void MakePermutations(int[] order, int position, Point[] checkpoints,
			ref double shortestDistance, ref int[] bestOrder)
		{
			var currentOrder = new int[position];
			Array.Copy(order, currentOrder, position);
			var pathLength = checkpoints.GetPathLength(currentOrder);

			if (!(pathLength < shortestDistance)) return;
			
			if (position == order.Length)
			{
				shortestDistance = pathLength;
				bestOrder = (int[])order.Clone();
				return;
			}

			for (var i = 1; i < order.Length; i++)
			{
				var index = Array.IndexOf(order, i, 0, position);
				if (index != -1)
					continue;
				order[position] = i;
				MakePermutations(order, position + 1, checkpoints, ref shortestDistance,
					ref bestOrder);
			}
		}
	}
}