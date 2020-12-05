using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public MessageType Type { get; set; }
        public MessageTopic Topic { get; set; }
        public string Name { get; set; }

        public Category(string name, MessageType type, MessageTopic topic)
        {
            Name = name;
            Type = type;
            Topic = topic;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return true;
            if (!(obj is Category)) return base.Equals(obj);

            var category = (Category) obj;
            return category.Type == Type && category.Topic == Topic && Name == category.Name;
        }

        public override int GetHashCode()
        {
            return 100 * Type.GetHashCode() + 10 * Topic.GetHashCode() + Name.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 0;
            
            if (!(obj is Category)) throw new ArgumentException();

            var category = (Category) obj;

            if (category.Name != Name)
            {
                return String.Compare(Name, category.Name, StringComparison.Ordinal);
            }

            if (category.Type != Type)
            {
                return Type.CompareTo(category.Type);
            }

            if (category.Topic != Topic)
            {
                return Topic.CompareTo(category.Topic);
            }

            return 0;
        }

        public override string ToString()
        {
            return $"{Name}.{Enum.GetName(Type.GetType(), Type)}.{Enum.GetName(Topic.GetType(), Topic)}";
        }

        public static bool operator <=(Category a, Category b)
        {
            return a.CompareTo(b) != 1;
        }

        public static bool operator >=(Category a, Category b)
        {
            return a.CompareTo(b) != -1;
        }

        public static bool operator <(Category a, Category b)
        {
            return a.CompareTo(b) == -1;
        }

        public static bool operator >(Category a, Category b)
        {
            return a.CompareTo(b) == 1;
        }

        public static bool operator ==(Category a, Category b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Category a, Category b)
        {
            return !a.Equals(b);
        }
    }
}