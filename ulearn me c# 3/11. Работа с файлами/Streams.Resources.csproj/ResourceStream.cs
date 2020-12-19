using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams.Resources
{
    public class ResourceReaderStream : Stream
    {
        private BufferedStream stream;
        private string key;
        private bool seeked = false;
        private bool returned = false;

        public ResourceReaderStream(Stream stream, string key)
        {
            this.stream = new BufferedStream(stream, Constants.BufferSize);
            this.key = key;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!seeked) SeekValue();
            if (returned) return 0;

            return ReadFieldValue(buffer, offset, count);
        }

        private void SeekValue()
        {
            var byteList = new List<byte>();

            var prevIsZero = false;
            while (true)
            {
                var byteRead = (byte) stream.ReadByte();

                if (byteRead == 0 && !prevIsZero)
                {
                    prevIsZero = true;
                    continue;
                }

                prevIsZero = false;
                
                byteList.Add(byteRead);
                if (byteList.Count > key.Length)
                    byteList.RemoveAt(0);

                if (Encoding.ASCII.GetString(byteList.ToArray()) != key && byteRead != 255) continue;

                seeked = true;
                break;
            }
            
            var firstSepByte = (byte) stream.ReadByte();
            var secondSepByte = (byte) stream.ReadByte();

            if (firstSepByte != 0 && secondSepByte != 1)
                throw new EndOfStreamException();
        }

        public int ReadFieldValue(byte[] buffer, int offset, int count)
        {
            int counter;

            for (counter = offset; counter < count + offset;)
            {
                if (counter >= count + offset - 1) return counter;

                var currentByte = (byte) stream.ReadByte();

                if (currentByte == 255)
                {
                    if (counter == 0)
                        throw new EndOfStreamException();

                    break;
                }

                if (currentByte != 0)
                {
                    buffer[counter++] = currentByte;
                    continue;
                }

                var prevIsZero = false;
                while (currentByte == 0)
                {
                    if (counter >= count + offset - 1) return counter;
                    currentByte = (byte) stream.ReadByte();

                    if (currentByte == 1)
                    {
                        if (!prevIsZero)
                        {
                            returned = true;
                            return counter;
                        }
                        
                        buffer[counter++] = 0;
                        buffer[counter++] = currentByte;
                        break;
                    }

                    if (prevIsZero)
                    {
                        buffer[counter++] = 0;
                    }
                    
                    if (currentByte == 255)
                    {
                        if (prevIsZero)
                        {
                            returned = true;
                            return counter;
                        } 
                        
                        throw new EndOfStreamException();
                    }
                    
                    prevIsZero = true;

                    if (currentByte != 0)
                        buffer[counter++] = currentByte;
                }
            }

            returned = true;
            Check();
            
            return counter;
        }

        private void Check()
        {
            var firstSepByte = (byte) stream.ReadByte();
            var secondSepByte = (byte) stream.ReadByte();

            if (firstSepByte != 0 && secondSepByte != 1)
                throw new EndOfStreamException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
            // nothing to do
        }
    }
}