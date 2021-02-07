using Assets.AllyaExtension;

namespace Systems
{
    /// <remarks>
    /// Todo: Bind systems to objects, components or events
    /// </remarks>
    public static class SystemManager
    {
        public static void RegisterSystem(ISystem system)
        {
            Scheduler.OnUpdate.Subscribe(system.OnUpdate);
        }

        public static void DeregisterSystem(ISystem system)
        {
            Scheduler.OnUpdate.Unsubscribe(system.OnUpdate);
        }
    }
}