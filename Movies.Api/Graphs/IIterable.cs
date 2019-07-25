﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Movies.Api.Graphs
{
    public interface IIterable<T>
    {
        IIterator<T> Iterator();
    }

    public interface IIterator<T> : IDisposable
    {
        bool HasNext { get; }
        T Next();
        void Remove();
    }

    public sealed class EnumerableAdapter<T> : IIterable<T>
    {
        private readonly IEnumerable<T> enumerable;

        public EnumerableAdapter(IEnumerable<T> enumerable) => this.enumerable = enumerable;

        public IIterator<T> Iterator() => new EnumeratorAdapter<T>(enumerable.GetEnumerator());
    }

    public sealed class EnumeratorAdapter<T> : IIterator<T>
    {
        private readonly IEnumerator<T> enumerator;
        private bool fetchedNext = false;
        private bool nextAvailable = false;
        private T next;

        public EnumeratorAdapter(IEnumerator<T> enumerator) => this.enumerator = enumerator;

        public bool HasNext { get; }

        public void Dispose() => enumerator.Dispose();

        public T Next()
        {
            CheckNext();

            if (!nextAvailable)
                throw new InvalidOperationException();

            fetchedNext = false; //consumed.
            return next;
        }

        public void Remove() => throw new NotSupportedException("Remove() operation is not supported.");

        void CheckNext()
        {
            if (fetchedNext)
                return;

            nextAvailable = enumerator.MoveNext();

            if (nextAvailable)
                next = enumerator.Current;

            fetchedNext = true;
        }
    }

    public sealed class IterableAdapter<T> : IEnumerable<T>
    {
        private readonly IIterable<T> iterable;

        public IterableAdapter(IIterable<T> iterable) => this.iterable = iterable;

        public IEnumerator<T> GetEnumerator() => new IteratorAdapter<T>(iterable.Iterator());
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class IteratorAdapter<T> : IEnumerator<T>
    {
        private readonly IIterator<T> iterator;
        private bool gotCurrent = false;
        private T current;

        public IteratorAdapter(IIterator<T> iterator) => this.iterator = iterator;

        public T Current
        {
            get
            {
                if (gotCurrent)
                    return current;

                throw new InvalidOperationException($"Could not get current {typeof(T).Name}");
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose() => iterator.Dispose();
        public bool MoveNext()
        {
            gotCurrent = iterator.HasNext;

            if (gotCurrent)
                current = iterator.Next();

            return gotCurrent;
        }

        public void Reset() => throw new NotSupportedException("Reset() operation is not supported.");
    }

    // A C# joke: I treat coworkers like I treat IDisposable: I'm contractually obligated to deal with unmanaged resources.
}
