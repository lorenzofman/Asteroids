using System;
using System.Collections.Generic;
using UnityEngine;

namespace GizmosHelper
{
    /// <summary>
    /// Renderize no Gizmos fora de um 'OnDrawGizmos'
    /// </summary>
    /// <remarks>
    /// A depuração com Gizmos é muito poderosa, porém ter que guardar os
    /// estados para desenhar corretamente é muito ruim e polui o código.
    /// </remarks>
    internal static class GizmosFromAnywhere
    {
        internal static readonly Queue<Action> QueuedActions = new Queue<Action>();
        internal static readonly List<Action> PersistentGizmos = new List<Action>();

        /// <summary>
        /// Enfileira uma operação Gizmos em qualauqer instante para ser
        /// renderizada na hora apropriada
        /// </summary>
        internal static void DrawGizmos(Action action)
        {
            QueuedActions.Enqueue(action);
        }

        internal static void DrawPersistent(Action action)
        {
            PersistentGizmos.Add(action);
        }

        public static void DrawLine(Vector3 a, Vector3 b)
        {
            QueuedActions.Enqueue(() =>
            {
                Gizmos.DrawLine(a, b);
            });
        }
        
        public static void Color(Color color)
        {
            QueuedActions.Enqueue(() =>
            {
                Gizmos.color = color;
            });
        }

        public static void DrawCircle(Vector3 position, float radius)
        {
            QueuedActions.Enqueue(() =>
            {
                Gizmos.DrawSphere(position, radius);
            }); 
        }
    }
}