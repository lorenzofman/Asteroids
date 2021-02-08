using System;
using Assets.AllyaExtension;

namespace Systems
{
    public static class SystemManager
    {
        public static void RegisterSystem(ISystem system, IBind bind, int priority = 0)
        {
            Action systemUpdate = system.OnUpdate;
            /* Unsubscribe method when binding ends */
            bind?.Bind(() =>
            {
                if (system is IDisposableSystem disposable)
                {
                    disposable.OnStop();
                }
                Scheduler.OnUpdate.Unsubscribe(systemUpdate, true);
            });
            Scheduler.OnUpdate.Subscribe(systemUpdate, priority);
        }
        
        public static void DeregisterSystem(ISystem system)
        {
            Scheduler.OnUpdate.Unsubscribe(system.OnUpdate, true);
        }
    }
}