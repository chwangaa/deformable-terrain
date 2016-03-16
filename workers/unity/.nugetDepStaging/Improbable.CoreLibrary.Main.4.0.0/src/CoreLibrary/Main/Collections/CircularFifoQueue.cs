using System;

namespace Improbable.Corelib.Collections
{
    /// <summary>
    ///     <para>
    ///         A first-in-first out queue with a fixed capacity. This queue replaces oldest elements if capacity is
    ///         exceeded.
    ///     </para>
    ///     <para><b>Important</b>: this collection is not thread-safe.</para>
    /// </summary>
    /// <typeparam name="T">the type of elements in this queue.</typeparam>
    public class CircularFifoQueue<T> where T : struct
    {
        private readonly T[] buffer;
        private int indexOfFirstElement;

        public CircularFifoQueue(int capacity)
        {
            Capacity = capacity;
            buffer = new T[capacity];
        }

        public int Capacity { get; private set; }

        public int Count { get; private set; }

        public void Enqueue(T element)
        {
            var indexOfFirstFreeSlot = (indexOfFirstElement + Count) % Capacity;
            buffer[indexOfFirstFreeSlot] = element;
            if (Count < Capacity)
            {
                ++Count;
            }
            else
            {
                IncrementIndexOfFirstElement();
            }
        }

        public T Dequeue()
        {
            T element;
            if (TryDequeue(out element))
            {
                return element;
            }
            throw new InvalidOperationException("There is no element in the queue.");
        }

        public T Peek()
        {
            T element;
            if (TryPeek(out element))
            {
                return element;
            }
            throw new InvalidOperationException("There is no element in the queue.");
        }

        public bool TryDequeue(out T element)
        {
            if (TryPeek(out element))
            {
                IncrementIndexOfFirstElement();
                --Count;
                return true;
            }
            return false;
        }

        public bool TryPeek(out T element)
        {
            var isNotEmpty = Count != 0;
            element = isNotEmpty ? buffer[indexOfFirstElement] : default(T);
            return isNotEmpty;
        }

        public void Clear()
        {
            Count = 0;
        }

        private void IncrementIndexOfFirstElement()
        {
            indexOfFirstElement = (indexOfFirstElement + 1) % Capacity;
        }
    }
}