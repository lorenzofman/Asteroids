using Assets.AllyaExtension;

namespace Systems
{
    public static class SystemManager
    {
        /// <summary>
        /// Todo: Bind systems to objects, components or events
        /// </summary>
        /// <param name="system"></param>
        public static void RegisterSystem(ISystem system)
        {
            Scheduler.OnUpdate.Subscribe(system.OnUpdate);   
        }
    }
}