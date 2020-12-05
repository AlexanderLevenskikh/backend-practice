using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree
{
    public static class DiskTreeTask
    {
        public static IEnumerable<string> Solve(IEnumerable<string> directories)
        {
            var main = new Node("main", -1);
            foreach (var directory in directories
                .Select(path => path.Split('\\')))
                AddPathToTree(directory, main);

            return CreateDirectories(main, new List<string>());
        }

        private static List<string> CreateDirectories(Node main, List<string> directories)
        {
            foreach (var directory in main.SubDirs)
            {
                directories.Add(new string(' ', directory.Level) + directory.Name);
                directories = CreateDirectories(directory, directories);
            }

            return directories;
        }

        private static void AddPathToTree(IEnumerable<string> directories, Node main)
        {
            var parent = main;
            var currentNode = main;
            foreach (var directory in directories)
            {
                currentNode = currentNode.SubDirs.FirstOrDefault(x => x.Name == directory);
                if (currentNode == null)
                {
                    var newNode = new Node(directory, parent.Level + 1);
                    parent.SubDirs.Add(newNode);
                    currentNode = newNode;
                }

                parent = currentNode;
            }
        }

        private class Node
        {
            public Node(string name, int level)
            {
                Name = name;
                Level = level;
            }

            public string Name { get; }

            public int Level { get; }

            public SortedSet<Node> SubDirs { get; } = new SortedSet<Node>(new StrCompareOrdinal());

            public override string ToString() => Name;
        }

        private class StrCompareOrdinal : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                if (x == null || y == null)
                    throw new ArgumentException();

                return string.CompareOrdinal(x.Name, y.Name);
            }
        }
    }
}