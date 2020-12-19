using System;
using System.Collections.Generic;

namespace Memory.API
{
    public class APIObject : IDisposable
    {
        private List<int> allocatedIds = new List<int>();
        public APIObject(int number)
        {
            allocatedIds.Add(number);
            MagicAPI.Allocate(number);
        }

        ~APIObject()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var allocatedId in allocatedIds)
            {
                MagicAPI.Free(allocatedId);
            }
            allocatedIds.Clear();
        }
    }
}
