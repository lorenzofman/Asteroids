using UnityEngine;

namespace Assets.AllyaExtension
{
    public class Scheduler : MonoBehaviour
    {
        public static readonly Schedule OnUpdate = new Schedule();
        public static readonly Schedule OnFixedUpdate = new Schedule();
        public static readonly Schedule OnGizmos = new Schedule();

        private void FixedUpdate()
        {
            OnFixedUpdate.Execute();
        }
        
        private void Update()
        {
            OnUpdate.Execute();
        }

        private void OnDrawGizmos()
        {
            OnGizmos.Execute();
        }
    }
}
