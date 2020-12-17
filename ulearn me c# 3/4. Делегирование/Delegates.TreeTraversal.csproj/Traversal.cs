using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<TOut> TreeTraversal<TIn, TOut>(
            TIn root,
            Func<TIn, IEnumerable<TIn>> getNext,
            Func<TIn, IEnumerable<TOut>> action)
        {
            var stack = new Stack<TIn>(new[] {root});
            var visited = new HashSet<TIn>();

            while (stack.Any())
            {
                var current = stack.Pop();
                visited.Add(current);

                foreach (var next in getNext(current))
                    if (!visited.Contains(next))
                    {
                        stack.Push(next);
                        visited.Add(next);
                    }

                foreach (var item in action(current))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            return TreeTraversal(
                root,
                category => category.Categories,
                category => category.Products);
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            return TreeTraversal(
                root,
                job => job.Subjobs,
                job => job.Subjobs.Count > 0 ? new Job[0] : new []{ job });
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            return TreeTraversal(
                root,
                tree =>
                {
                    var next = new List<BinaryTree<T>>();
                    if (tree.Left != null) next.Add(tree.Left);
                    if (tree.Right != null) next.Add(tree.Right);

                    return next;
                },
                tree => tree.Left != null || tree.Right != null ? new T[0] : new []{ tree.Value });
        }
    }
}