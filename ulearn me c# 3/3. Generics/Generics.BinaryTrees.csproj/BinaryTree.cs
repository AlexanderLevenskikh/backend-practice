using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    class BinaryTreeNode<T> where T : IComparable
    {
        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }
        public T Value { get; set; }

        public BinaryTreeNode(T value = default)
        {
            Value = value;
        }
    }
    
    class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        public BinaryTreeNode<T> Root { get; set; }

        public T Value => Root.Value;
        public BinaryTreeNode<T> Left => Root.Left;
        public BinaryTreeNode<T> Right => Root.Right;
        
        public BinaryTree() {}

        public BinaryTree(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Add(T value)
        {
            var newNode = new BinaryTreeNode<T>(value);
            
            if (Root == null)
            {
                Root = newNode;
                return;
            }

            var currentNode = Root;

            while (true)
            {
                if (value.CompareTo(currentNode.Value) > 0)
                {
                    if (currentNode.Right == null)
                    {
                        currentNode.Right = newNode;
                        break;
                    }
                    
                    currentNode = currentNode.Right;
                }
                else
                {
                    if (currentNode.Left == null)
                    {
                        currentNode.Left = newNode;
                        break;
                    }
                    
                    currentNode = currentNode.Left;
                }
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            var stack = new Stack<BinaryTreeNode<T>>();
            var current = Root;
            
            while (stack.Count > 0 || current != null)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }
                else
                {
                    current = stack.Pop();
                    yield return current.Value;
                    current = current.Right;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] array) where T : IComparable
        {
            return new BinaryTree<T>(array);
        }
    }
}
