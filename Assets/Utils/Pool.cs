using System.Collections.Generic;

public readonly struct Pool<T>
{
    private readonly ICreator<T> creator;
    /* Todo: NativePool */
    private readonly Stack<T> internalStack;
    
    public Pool(ICreator<T> creator, int preAllocated = 10)
    {
        this.creator = creator;
        internalStack = new Stack<T>();
        for (int i = 0; i < preAllocated; i++)
        {
            internalStack.Push(creator.Create());
        }
    }

    public T Retrieve() => internalStack.Count == 0 ? creator.Create() : internalStack.Pop();

    public void Return(T obj) => internalStack.Push(obj);
}