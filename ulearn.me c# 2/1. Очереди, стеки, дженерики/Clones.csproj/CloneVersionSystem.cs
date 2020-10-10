using System;
using System.Collections.Generic;

namespace Clones
{
    public class LinkedStackItem<T>
    {
        public LinkedStackItem<T> Prev { get; set; }
        public T Item { get; set; }
    }

    public class LinkedStack<T> : ICloneable where T : class
    {
        private LinkedStackItem<T> head;
        private LinkedStackItem<T> tail;

        public void Push(T item)
        {
            if (head == null)
            {
                head = tail = new LinkedStackItem<T>
                {
                    Item = item,
                    Prev = null
                };
            }
            else
            {
                var prevHead = head;
                head = new LinkedStackItem<T>
                {
                    Item = item,
                    Prev = prevHead
                };
            }
        }

        public T Pop()
        {
            if (head == null)
            {
                throw new InvalidOperationException();
            }

            if (head == tail)
            {
                var value = head.Item;
                head = null;
                tail = null;

                return value;
            }

            var prevHead = head;
            head = head.Prev;

            return prevHead.Item;
        }

        public T Last()
        {
            return head?.Item;
        }

        public object Clone()
        {
            return new LinkedStack<T>
            {
                head = head,
                tail = tail
            };
        }
    }

    public enum CloneVersionSystemCommand
    {
        Learn,
        Rollback,
        Relearn,
        Clone,
        Check
    }

    public class CloneCharacter : ICloneable
    {
        private LinkedStack<string> programs;
        private LinkedStack<string> rollbacks;

        public CloneCharacter()
        {
            programs = new LinkedStack<string>();
            rollbacks = new LinkedStack<string>();
        }

        public object Clone()
        {
            return new CloneCharacter()
            {
                programs = (LinkedStack<string>) programs.Clone(),
                rollbacks = (LinkedStack<string>) rollbacks.Clone()
            };
        }

        public void Learn(string program)
        {
            programs.Push(program);
        }

        public void Rollback()
        {
            var lastProgram = programs.Pop();
            rollbacks.Push(lastProgram);
        }

        public void Relearn()
        {
            var lastRollback = rollbacks.Pop();
            programs.Push(lastRollback);
        }

        public string Check()
        {
            var last = programs.Last();
            
            return last ?? "basic";
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        private Dictionary<string, CloneCharacter> clones;
        private int nextCloneId;

        public CloneVersionSystem()
        {
            clones = new Dictionary<string, CloneCharacter> {{"1", new CloneCharacter()}};
            nextCloneId = 2;
        }

        public string Execute(string query)
        {
            var queryChunks = query.Split(' ');
            var commandStr = queryChunks[0];

            CloneVersionSystemCommand command;
            switch (commandStr)
            {
                case "learn":
                    command = CloneVersionSystemCommand.Learn;
                    break;
                case "rollback":
                    command = CloneVersionSystemCommand.Rollback;
                    break;
                case "relearn":
                    command = CloneVersionSystemCommand.Relearn;
                    break;
                case "clone":
                    command = CloneVersionSystemCommand.Clone;
                    break;
                case "check":
                    command = CloneVersionSystemCommand.Check;
                    break;
                default:
                    throw new ArgumentException();
            }

            var cloneId = queryChunks[1];
            var programId = command == CloneVersionSystemCommand.Learn
                ? queryChunks[2]
                : null;

            var clone = clones[cloneId];

            switch (command)
            {
                case CloneVersionSystemCommand.Learn:
                    clone.Learn(programId);
                    break;
                case CloneVersionSystemCommand.Rollback:
                    clone.Rollback();
                    break;
                case CloneVersionSystemCommand.Relearn:
                    clone.Relearn();
                    break;
                case CloneVersionSystemCommand.Clone:
                    var nextClone = clone.Clone();
                    clones.Add(nextCloneId.ToString(), (CloneCharacter)nextClone);
                    nextCloneId++;
                    break;
                case CloneVersionSystemCommand.Check:
                    return clone.Check();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }
}