using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
	public class Indexer
	{
		private readonly double[] array;

		private int start;
		public int Start
		{
			get => start;
			set
			{
				if (value < 0 || value > array.Length)
					throw new ArgumentException();

				start = value;
			}
		}

		private int length;
		public int Length
		{
			get => length;
			set
			{
				if (start + value > array.Length || start + value < 0 || value < 0) 
					throw new ArgumentException();

				length = value;
			}
		}
		
		public Indexer(double[] range, int start, int length)
		{
			array = range;
			Start = start;
			Length = length;
		}

		public double this[int x]
		{
			get
			{
				if (x > Length - 1 || x < 0)
				{
					throw new IndexOutOfRangeException();
				}

				return array[x + Start];
			}

			set
			{
				if (x > Length - 1 || x < 0)
				{
					throw new IndexOutOfRangeException();
				}

				array[x + Start] = value;
			}
		}
	}
}
