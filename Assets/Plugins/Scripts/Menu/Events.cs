using System;
using UnityEngine.Events;

namespace Menu
{
    /// <summary>
    ///     <para> Events - Container class for Serializable Events in UnityEditor </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public static class Events {
        [Serializable]
        public class Vector2 : UnityEvent<UnityEngine.Vector2> { }
    }
}