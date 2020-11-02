using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			var st = new Stack<int>();
			var leftToRight = new Dictionary<int, int>();
			var rightToLeft = new Dictionary<int, int>();
			
			for (var i = 0; i < vm.Instructions.Length; i++)
			{
				var instruction = vm.Instructions[i];
				if (instruction == '[')
				{
					st.Push(i);
				} else if (instruction == ']')
				{
					var popIndex = st.Pop();
					leftToRight.Add(popIndex, i);
					rightToLeft.Add(i, popIndex);
				}
			}
			
			vm.RegisterCommand('[', b =>
			{
				if (vm.Memory[vm.MemoryPointer] == 0)
				{
					vm.InstructionPointer = leftToRight[vm.InstructionPointer];
				}
			});
			vm.RegisterCommand(']', b => {
				if (vm.Memory[vm.MemoryPointer] != 0)
				{
					vm.InstructionPointer = rightToLeft[vm.InstructionPointer];
				}
			});
		}
	}
}