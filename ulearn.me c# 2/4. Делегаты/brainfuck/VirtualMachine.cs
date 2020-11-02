using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public VirtualMachine(string program, int memorySize)
		{
			Memory = new byte[memorySize];
			Instructions = program;
			Commands = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			if (Commands.ContainsKey(symbol))
			{
				Commands[symbol] = execute;
			}
			else
			{
				Commands.Add(symbol, execute);
			}
		}

		private Dictionary<char, Action<IVirtualMachine>> Commands { get; }
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				var instruction = Instructions[InstructionPointer];
				if (Commands.ContainsKey(instruction))
				{
					Commands[instruction](this);
				}

				InstructionPointer++;
			}
		}
	}
}