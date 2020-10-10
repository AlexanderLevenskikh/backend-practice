using System;
using System.Text;

namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
    {
        private byte[] documentBytes;
        private Vector vector;
        private Document document;
        private Segment segment;
        private Cat cat;
        private Robot robot;

        public GhostsTask()
        {
            documentBytes = new byte[]{0, 1};
            vector = new Vector(0, 0);
            document = new Document("", Encoding.Default, documentBytes);
            segment = new Segment(new Vector(0, 0), new Vector(1, 1));
            cat = new Cat("", "", DateTime.Now);
            robot = new Robot("");
        }
        public void DoMagic()
        {
            vector.Add(new Vector(1, 1));
            segment.Start.Add(new Vector(1, 1));
            cat.Rename("abc!");
            Robot.BatteryCapacity++;
            documentBytes[0] = 21;
        }

        // Чтобы класс одновременно реализовывал интерфейсы IFactory<A> и IFactory<B> 
        // придется воспользоваться так называемой явной реализацией интерфейса.
        // Чтобы отличать методы создания A и B у каждого метода Create нужно явно указать, к какому интерфейсу он относится.
        // На самом деле такое вы уже видели, когда реализовывали IEnumerable<T>.

        Vector IFactory<Vector>.Create() => vector;
        Segment IFactory<Segment>.Create() => segment;
        Document IFactory<Document>.Create() => document;
        Cat IFactory<Cat>.Create() => cat;
        Robot IFactory<Robot>.Create() => robot;
    }
}