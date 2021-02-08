using System;
using UnityEngine;

namespace GizmosHelper
{
    /// <summary>
    /// Consumidor de Gizmos
    /// </summary>
    public class GizmosConsumer : MonoBehaviour
    {
        /// <summary>
        /// Consome gizmos da lista no momento apropriado
        /// </summary>
        private void OnDrawGizmos()
        {
            while (GizmosFromAnywhere.QueuedActions.Count > 0)
            {
                GizmosFromAnywhere.QueuedActions.Dequeue()?.Invoke();
            }

            foreach (Action persistentGizmo in GizmosFromAnywhere.PersistentGizmos)
            {
                persistentGizmo?.Invoke();
            }
        }
    }
}