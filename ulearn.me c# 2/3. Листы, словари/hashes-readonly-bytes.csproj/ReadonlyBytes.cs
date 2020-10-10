using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
	{
		public int Length => bytes.Length;
		public byte this[int index] => bytes[index];

		private readonly byte[] bytes;
		private readonly int hash;

		public ReadonlyBytes(params byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException();
			}
			
			this.bytes = bytes;
			const int prime = 2683;
			
			var result = 0;
			var factor = 1;

			unchecked
			{
				foreach (var b in bytes)
				{
					result += b * factor;
					factor *= prime;
				}
			}

			hash = result;
		}

		public IEnumerator<byte> GetEnumerator()
		{
			return ((IEnumerable<byte>) bytes).GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != typeof(ReadonlyBytes)) return false;
			var objBytes = (ReadonlyBytes) obj;

			if (bytes.Length != objBytes.Length) return false;
			if (bytes.Length == 0) return true;

			return !bytes.Where((t, i) => !t.Equals(objBytes[i])).Any();
		}

		public override int GetHashCode()
		{
			return hash;
		}

		public override string ToString()
		{
			var builder = new StringBuilder("[");
			for (var i = 0; i < bytes.Length; i++)
			{
				builder.Append(bytes[i].ToString());
				if (i < bytes.Length - 1)
				{
					builder.Append(", ");
				}
			}
			builder.Append("]");

			return builder.ToString();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}