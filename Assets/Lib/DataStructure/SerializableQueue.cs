using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace UnityLib.DataStructure
{
    [ProtoContract(AsReferenceDefault = true)]
    public class SerializableQueue<T> : IEnumerable<T>
    {
        [ProtoMember(1)]
        T[] array;
        [ProtoMember(2)]
        int head;
        [ProtoMember(3)]
        int tail;
        [ProtoMember(4)]
        int _count;

        public int Count { get { return _count; } }

        public SerializableQueue() { array = new T[4]; }

        public void Enqueue(T o)
        {
            _count++;
            if (_count > array.Length)
                Array.Resize(ref array, array.Length * 2);
            array[head++] = o;

            if (head == array.Length)
                head = 0;
        }

        public T Dequeue()
        {
            if (_count == 0)
                return default(T);

            _count--;
            var o = array[tail++];

            if (tail == array.Length)
                tail = 0;

            return o;
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Cannot peek a queue with 0 element");
            return array[tail];
        }

        public T[] DequeueAll()
        {
            if (_count == 0)
                return null;

            int count = _count;
            var output = new T[count];
            for (int i = 0; i < count; i++)
                output[i] = Dequeue();
            return output;
        }

        public bool Contain(T o)
        {
            if (o == null)
                return false;

            //for (int i = 0; i < array.Length; i++)
            //    if (o.Equals(array[i]))
            //        return true;
            //return false;

            int position = 0;
            int i = tail;
            while (position < _count)
            {
                if (o.Equals(array[i]))
                    return true;
                position++;
                if (i + 1 == array.Length) i = 0; else i++;
            }
            return false;
        }

        public int IndexOf(T o)
        {
            int position = 0;
            int i = tail;
            while (position < _count)
            {
                if (o.Equals(array[i]))
                    return position;
                position++;
                if (i + 1 == array.Length) i = 0; else i++;
            }
            return -1;
        }

        //public int LastIndexOf(T o)
        //{
        //    int position = _count;
        //    int i = head - 1 < 0 ? array.Length : head - 1;
        //    while (position >= 0)
        //    {
        //        if (o.Equals(array[i]))
        //            return position;
        //        position--;
        //        if (i == 0) i = array.Length; else i--;
        //    }
        //    return -1;
        //}

        public void Clear()
        {
            _count = 0;
            head = 0;
            tail = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)array).GetEnumerator();
        }
    }
}