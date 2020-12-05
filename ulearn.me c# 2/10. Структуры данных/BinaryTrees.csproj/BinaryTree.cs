using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class TreeNode<T> where T : IComparable
    {
        public T Value { get; set; }
        public TreeNode<T> Left { get; set; }
        public TreeNode<T> Right { get; set; }

        public int SubTreeSize { get; set; }
    }

    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private TreeNode<T> rootNode;
        private TreeNode<T> currentNode;
        private int totalCount;
        
        public void Add(T key)
        {
            totalCount++;
            
            if (rootNode == null)
            {
                rootNode = new TreeNode<T>
                {
                    Left = null,
                    Right = null,
                    Value = key,
                    SubTreeSize = 1,
                };
                return;
            }

            var current = rootNode;
            var nodeToInsert = new TreeNode<T>
            {
                Left = null,
                Right = null,
                Value = key,
                SubTreeSize = 1
            };

            while (true)
            {
                current.SubTreeSize++;
                
                if (key.CompareTo(current.Value) < 0)
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                        continue;
                    }

                    current.Left = nodeToInsert;
                    break;
                }

                if (current.Right != null)
                {
                    current = current.Right;
                    continue;
                }

                current.Right = nodeToInsert;
                break;
            }
        }

        public bool Contains(T key)
        {
            if (rootNode == null)
            {
                return false;
            }

            var current = rootNode;
            while (true)
            {
                if (current.Value.CompareTo(key) > 0)
                {
                    if (current.Left == null)
                    {
                        return false;
                    }

                    current = current.Left;
                }
                else if (current.Value.CompareTo(key) < 0)
                {
                    if (current.Right == null)
                    {
                        return false;
                    }

                    current = current.Right;
                }
                else
                {
                    return true;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (rootNode == null)
            {
                yield break;
            }
            
            for (var i = 0; i < rootNode.SubTreeSize; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int i]
        {
            get
            {
                currentNode = rootNode;
                if (currentNode == null || currentNode.SubTreeSize <= i)
                    throw new IndexOutOfRangeException();

                var count = 0;
                var currentNodeIndex = currentNode.Left?.SubTreeSize ?? 0;
                while (true)
                {
                    if (currentNodeIndex == i)
                        return currentNode.Value;

                    if (currentNodeIndex > i)
                    {
                        currentNode = currentNode.Left;
                        currentNodeIndex = (currentNode.Left?.SubTreeSize ?? 0) + count;
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                        count = currentNodeIndex + 1;
                        currentNodeIndex = count + (currentNode.Left?.SubTreeSize ?? 0);
                    }
                }
            }
        }
    }
}