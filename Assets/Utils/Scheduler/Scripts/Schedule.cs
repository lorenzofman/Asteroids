using System;
using System.Collections.Generic;
using UnityEngine;

public class SchedulingException : Exception
{
    private readonly Action source;
    private readonly Exception exception;
    public SchedulingException(Exception exception, Action source)
    {
        this.exception = exception;
        this.source = source;
    }

    public override string ToString()
    {
        return $"Exception while executing {source.Method} in {source.Target} -> {exception}";
    }
}

public class Schedule
{
    private readonly SortedList<int, Action> actions = new SortedList<int, Action>(new DuplicateKeyComparer<int>());
    private readonly Queue<Action> pendingActions = new Queue<Action>();
    private readonly Queue<Exception> exceptions = new Queue<Exception>();
    
    private bool executing;
    public int Count => actions.Count;

    public void Subscribe(Action action, int priority = 0)
    {
        IterationSafe(() => ImmediateSubscribe(action, priority));
    }

    public void Unsubscribe(Action action, bool bestEffort = false)
    {
        IterationSafe(() => ImmediateUnsubscribe(action, bestEffort));
    }

    public void SubscribeOnce(Action action, int priority = 0)
    {
        IterationSafe(() => ImmediateSubscribe(Proxy, priority));

        void Proxy()
        {
            action.Invoke();
            Unsubscribe(Proxy);
        }
    }

    public void Clear()
    {
        IterationSafe(ImmediateClear);
    }

    public void Execute()
    {
        Flush(pendingActions);
        executing = true;
        foreach (Action action in actions.Values)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                // exceptions.Enqueue(new SchedulingException(ex, action));
            }
        }
        executing = false;
        WarnAboutExceptions();
        Flush(pendingActions);
    }

    private void IterationSafe(Action action)
    {
        if (executing)
        {
            pendingActions.Enqueue(action);
        }
        else
        {
            action.Invoke();
        }
    }

    private void ImmediateSubscribe(Action action, int priority = 0)
    {
        actions.Add(priority, action);
    }

    private void ImmediateUnsubscribe(Action action, bool bestEffort)
    {
        int index = actions.IndexOfValue(action);
        if (bestEffort && index < 0)
        {
            return;
        }
        actions.RemoveAt(index);
    }

    private void ImmediateClear()
    {
        executing = false;
        actions.Clear();
        pendingActions.Clear();
    }

    private void WarnAboutExceptions()
    {
        while (exceptions.Count > 0)
        {
            Debug.LogWarning(exceptions.Dequeue());
        }
    }

    private static void Flush(Queue<Action> queue)
    {
        while (queue.Count > 0)
        {
            queue.Dequeue().Invoke();
        }
    }
}
