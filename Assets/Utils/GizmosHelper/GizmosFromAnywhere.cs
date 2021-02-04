using System;
using System.Collections.Generic;

namespace Atlas.RasterRendering.Internal
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
    }
}