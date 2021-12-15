using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Network;
using DotNetty.Buffers;

namespace ChatServer.Utils {
    public class BoolBitMap : INetworkData, IList<bool>
    {

        private readonly List<bool> Root = new List<bool>();

        public void Encode(IByteBuffer buffer)
        {
            Trim();
            byte[] bytes = BitArrayUtils.ToBytes(Root.ToArray());
            buffer.WriteByte(bytes.Length);
            for (var i = 0; i < bytes.Length; i++)
            {
                buffer.WriteByte(bytes[i]);
            }
        }

        public void Decode(IByteBuffer buffer)
        {
            Root.Clear();
            int length = buffer.ReadByte();
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = buffer.ReadByte();
            }

            bool[] bits = BitArrayUtils.ToBits(bytes);
            for (var i = 0; i < bits.Length; i++)
            {
                Add(bits[i]);
            }
        }

        public void CopyIndexTo(ICollection<int> collection, bool GetBit = true)
        {
            for (var i = 0; i < Root.Count; i++)
            {
                if (Root[i] == GetBit)
                    collection.Add(i);
            }
        }

        public void Trim()
        {
            while (Root.Count > 0 && !Root[0])
                Root.RemoveAt(0);
            while (Root.Count > 0 && !Root[Root.Count - 1])
                Root.RemoveAt(Root.Count - 1);
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return Root.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(bool item)
        {
            Root.Add(item);
        }

        public void Clear()
        {
            Root.Clear();
        }

        public bool Contains(bool item)
        {
            return Root.Contains(item);
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            Root.CopyTo(array, arrayIndex);
        }

        public bool Remove(bool item)
        {
            return Root.Remove(item);
        }

        public int Count => Root.Count;
        public bool IsReadOnly => false;

        public int IndexOf(bool item)
        {
            return Root.IndexOf(item);
        }

        public void Insert(int index, bool item)
        {
            Root.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Root.RemoveAt(index);
        }

        public bool this[int index]
        {
            get => Root[index];
            set
            {
                int need = index - Root.Count + 1;
                for (int i = 0; i < need; i++)
                    Root.Add(false);
                Root[index] = value;
            }
        }
    }
}
