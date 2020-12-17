using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace FluentApi.Graph
{
    [TestFixture]
    public class DotGraphBuilder_Tests
    {
        [Test]
        public void EmptyDirectedGraph()
        {
            var dot = DotGraphBuilder.DirectedGraph("EmptyGraph").Build();
            AssertAreSame("digraph EmptyGraph { }", dot);
        }

        [Test]
        public void EmptyUndirectedGraph()
        {
            var dot = DotGraphBuilder.UndirectedGraph("EmptyGraph").Build();
            AssertAreSame("graph EmptyGraph { }", dot);
        }

        [Test]
        public void JustNodes()
        {
            var dot = DotGraphBuilder.DirectedGraph("NoEdges")
                .AddNode("n1")
                .AddNode("n2")
                .Build();
            AssertAreSame("digraph NoEdges { n1; n2 }", dot);
        }

        [Test]
        public void JustNodesWithAttributes()
        {
            var dot = DotGraphBuilder.DirectedGraph("NoEdges")
                .AddNode("n1").With(c => c.Color("black").Shape(NodeShape.Box))
                .AddNode("n2").With(c => c.Shape(NodeShape.Ellipse).FontSize(12).Label("node №2"))
                .Build();
            AssertAreSame(@"digraph NoEdges { 
n1 [color=black; shape=box]; n2 [fontsize=12; label=""node №2""; shape=ellipse] }", dot);
        }

        [Test]
        public void JustEdges()
        {
            var dot =
                DotGraphBuilder
                    .DirectedGraph("G")
                    .AddEdge("a", "b")
                    .AddEdge("a", "x")
                    .Build();
            AssertAreSame("digraph G { a -> b; a -> x }", dot);
        }

        [Test]
        public void JustEdgesWithAttributes()
        {
            var dot =
                DotGraphBuilder
                    .UndirectedGraph("G")
                    .AddEdge("a", "b").With(a => a.Label("ab").FontSize(12).Color("black"))
                    .AddEdge("a", "x")
                    .AddEdge("x", "y").With(a => a.Weight(2).Color("red"))
                    .Build();
            AssertAreSame(@"graph G { 
a -- b [color=black; fontsize=12; label=ab]; a -- x; x -- y [color=red; weight=2] }", dot);
        }

        [Test]
        public void NodesBeforeEdges()
        {
            var dot =
                DotGraphBuilder
                    .UndirectedGraph("G")
                    .AddNode("b").With(a => a.Shape(NodeShape.Box))
                    .AddEdge("a", "b").With(e => e.Weight(3.14))
                    .Build();
            AssertAreSame("graph G { b [shape=box]; a -- b [weight=3.14] }", dot);
        }

        [Test]
        public void EdgesBeforeNodes()
        {
            var dot =
                DotGraphBuilder
                    .UndirectedGraph("G")
                    .AddEdge("a", "b").With(e => e.Weight(3.14))
                    .AddNode("b").With(a => a.Shape(NodeShape.Box))
                    .Build();
            AssertAreSame("graph G { b [shape=box]; a -- b [weight=3.14] }", dot);
        }

        [Test]
        public void NamesAreEscaped()
        {
            // Используйте готовый код Graph, DotFormatWriter, чтобы пройти этот тест "бесплатно"
            var dot = DotGraphBuilder.DirectedGraph("my graph")
                .AddNode("42 is the answer").With(a => a.Color("#00ff00"))
                .AddNode("-3.14")
                .AddNode("\"quotes\"")
                .AddEdge("3", "abc").With(a => a.Label("long text"))
                .AddEdge("3x", "a b c").With(a => a.Label("1.234"))
                .Build();
            AssertAreSame(@"digraph ""my graph"" {
""42 is the answer"" [color=""#00ff00""];
-3.14;
""\""quotes\"""";
3 -> abc [label=""long text""];
""3x"" -> ""a b c"" [label=1.234]
}", dot);
        }

        [Test]
        public void WithAcceptsEmptyAction()
        {
            var dot =
                DotGraphBuilder.UndirectedGraph("G")
                    .AddNode("node").With(c => { })
                    .AddEdge("a", "b").With(c => { })
                    .Build();
            AssertAreSame("graph G { node; a -- b }", dot);
        }

        private static void AssertAreSame(string expectedDot, string actualDot)
        {
            Assert.AreEqual(NormalizeString(expectedDot), NormalizeString(actualDot));
        }

        private static string NormalizeString(string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }
    }
}