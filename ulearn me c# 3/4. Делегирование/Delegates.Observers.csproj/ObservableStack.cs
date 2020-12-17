using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{

	public class StackOperationsLogger
	{
		private StringBuilder Log = new StringBuilder();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Observers += eventData => Log.Append(eventData);
		}

		public string GetLog()
		{
			return Log.ToString();
		}
	}

	public class ObservableStack<T>
	{
		public event Action<StackEventData<T>> Observers;

		public void Notify(object eventData)
		{
			if (Observers == null)
				return;
			
			foreach (var observer in Observers.GetInvocationList())
				observer.DynamicInvoke(eventData);
		}

		List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;

		}
	}
}
