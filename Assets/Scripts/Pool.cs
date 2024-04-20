using System;
using System.Collections.Generic;

public class Pool<T>
{
    private readonly Func<T> CreationFunction;
    private readonly Action<T> AcquiringFunction;
    private readonly Action<T> ReleasingFunction;
    private Queue<T> _queue;

    public Pool(Func<T> creationFunction, Action<T> acquiringFunction, Action<T> releasingFunction)
    {
        CreationFunction = creationFunction;
        AcquiringFunction = acquiringFunction;
        ReleasingFunction = releasingFunction;
        _queue = new Queue<T>();
    }

    public T Acquire()
    {
        var item = _queue.Count > 0 ? _queue.Dequeue() : CreationFunction();
        AcquiringFunction(item);

        return item;
    }

    public void Release(T item)
    {
        ReleasingFunction(item);
        _queue.Enqueue(item);
    }

    public void Preload(int itemsNumber)
    {
        for (var i = 0; i < itemsNumber; i++)
        {
            Release(Acquire());
        }
    }
}
