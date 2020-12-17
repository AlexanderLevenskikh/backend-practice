using System;
using System.Collections.Generic;
using System.Globalization;

namespace FluentApi.Graph
{
    public class DotGraphBuilder
    {
        private Graph g;

        public static DotGraphBuilder DirectedGraph(string graphName)
        {
            var builder = new DotGraphBuilder
            {
                g = new Graph(graphName, true, true)
            };

            return builder;
        }

        public static DotGraphBuilder UndirectedGraph(string graphName)
        {
            var builder = new DotGraphBuilder
            {
                g = new Graph(graphName, false, true)
            };

            return builder;
        }

        public NodeBuilder AddNode(string nodeName)
        {
            var node = g.AddNode(nodeName);

            return new NodeBuilder(this, node);
        }

        public EdgeBuilder AddEdge(string sourceNode, string destinationNode)
        {
            var edge = g.AddEdge(sourceNode, destinationNode);

            return new EdgeBuilder(this, edge);
        }

        public string Build()
        {
            return g.ToDotFormat();
        }
    }

    public enum NodeShape
    {
        Box,
        Ellipse,
    }

    public class NodeBuilder : BaseItemBuilder
    {
        public NodeBuilder(DotGraphBuilder graphBuilder, GraphNode node) : base(graphBuilder,
            new AttributeBuilder(node.Attributes))
        {
        }
    }
    
    public class EdgeBuilder : BaseItemBuilder
    {
        public EdgeBuilder(DotGraphBuilder graphBuilder, GraphEdge edge) : base(graphBuilder,
            new AttributeBuilder(edge.Attributes))
        {
        }
    }

    public class BaseItemBuilder
    {
        private DotGraphBuilder graphBuilder;
        private AttributeBuilder attributeBuilder;

        public BaseItemBuilder(DotGraphBuilder graphBuilder, AttributeBuilder attributeBuilder)
        {
            this.attributeBuilder = attributeBuilder;
            this.graphBuilder = graphBuilder;
        }

        public DotGraphBuilder With(Action<AttributeBuilder> process)
        {
            process(attributeBuilder);

            return graphBuilder;
        }

        public NodeBuilder AddNode(string nodeName)
        {
            return graphBuilder.AddNode(nodeName);
        }

        public EdgeBuilder AddEdge(string sourceNode, string destinationNode)
        {
            return graphBuilder.AddEdge(sourceNode, destinationNode);
        }

        public string Build()
        {
            return graphBuilder.Build();
        }
    }
    
    public class AttributeBuilder
    {
        private readonly Dictionary<string, string> attributes;

        internal AttributeBuilder(Dictionary<string, string> attributes)
        {
            this.attributes = attributes;
        }

        internal AttributeBuilder Color(string color)
        {
            return AddAttribute("color", color);
        }

        internal AttributeBuilder Shape(NodeShape shape)
        {
            string shapeValue;
            switch (shape)
            {
                case NodeShape.Box:
                    shapeValue = "box";
                    break;
                case NodeShape.Ellipse:
                    shapeValue = "ellipse";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return AddAttribute("shape", shapeValue);
        }

        internal AttributeBuilder FontSize(int fontSize)
        {
            return AddAttribute("fontsize", fontSize.ToString());
        }

        internal AttributeBuilder Label(string label)
        {
            return AddAttribute("label", label);
        }

        internal AttributeBuilder Weight(double weight)
        {
            return AddAttribute("weight", weight.ToString(CultureInfo.InvariantCulture));
        }

        private AttributeBuilder AddAttribute(string attributeName, string attributeValue)
        {
            if (!attributes.ContainsKey(attributeName))
            {
                attributes.Add(attributeName, attributeValue);
            }
            else
            {
                attributes[attributeName] = attributeValue;
            }

            return this;
        }
    }
}