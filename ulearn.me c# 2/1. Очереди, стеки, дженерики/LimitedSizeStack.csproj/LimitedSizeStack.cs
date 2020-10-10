using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStackItem<T>
    {
        public LimitedSizeStackItem<T> Prev { get; set; }
        public LimitedSizeStackItem<T> Next { get; set; }
        public T Value { get; set; }
    }
    
    public class LimitedSizeStack<T>
    {
        private LimitedSizeStackItem<T> head;
        private LimitedSizeStackItem<T> tail;
        private int limit;
        public int Count { get; set; }
        
        public LimitedSizeStack(int limit)
        {
            this.limit = limit;
            Count = 0;
        }

        public void Push(T item)
        {
            if (limit == 0)
            {
                return;    
            }
            
            if (limit == Count)
            {
                tail = tail.Next;
                tail.Prev = null;
                Count--;
            }
            
            if (head == null)
            {
                tail = head = new LimitedSizeStackItem<T>
                {
                    Next = null,
                    Prev = null,
                    Value = item
                };
                Count++;
            } else
            {
                var nextHead = new LimitedSizeStackItem<T>
                {
                    Next = null,
                    Prev = head,
                    Value = item
                };

                head.Next = nextHead;
                head = nextHead;
                Count++;
            }
        }

        public T Pop()
        {
            if (Count == 1)
            {
                var item = head;
                head = null;
                tail = null;
                Count--;

                return item.Value;
            }

            var prevHead = head;
            head = head.Prev;
            Count--;

            return prevHead.Value;
        }
    }
}
