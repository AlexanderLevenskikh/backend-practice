using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));
			vm.RegisterCommand('+', b =>
			{
				if (b.Memory[b.MemoryPointer] == 255)
				{
					b.Memory[b.MemoryPointer] = 0;
				}
				else
				{
					b.Memory[b.MemoryPointer]++;
				}
			});
			vm.RegisterCommand('-', b => 
			{
				if (b.Memory[b.MemoryPointer] == 0)
				{
					b.Memory[b.MemoryPointer] = 255;
				}
				else
				{
					b.Memory[b.MemoryPointer]--;
				}
			});
			vm.RegisterCommand(',', b =>
			{
				var symbol = (char)read();
				b.Memory[b.MemoryPointer] = (byte)symbol;
			});
			vm.RegisterCommand('>', b =>
			{
				if (b.MemoryPointer == b.Memory.Length - 1)
				{
					b.MemoryPointer = 0;
				}
				else
				{
					b.MemoryPointer++;
				}
			});
			vm.RegisterCommand('<', b =>
			{
				if (b.MemoryPointer == 0)
				{
					b.MemoryPointer = b.Memory.Length - 1;
				}
				else
				{
					b.MemoryPointer--;
				}
			});

			var symbols = Enumerable
				.Range('a', 'z' - 'a' + 1)
				.Concat(Enumerable.Range('A', 'Z' - 'A' + 1))
				.Concat(Enumerable.Range('0', '9' - '0' + 1))
				.Select(x => (char) x)
				.ToArray();

			foreach (var symbol in symbols)
			{
				vm.RegisterCommand(symbol, b => b.Memory[b.MemoryPointer] = (byte)symbol);
			}
		}
	}
}