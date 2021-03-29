using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public enum TriggerState
    {
        Default,
        OnPause,
        OnResume,
        OnDrag
    }

    /// <summary>
    ///     <para> CreditsTrigger </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class CreditsTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,
        IBeginDragHandler, IEndDragHandler
    {
        private readonly Events.Vector2 _onSwipe = new Events.Vector2();
        private readonly Events.Vector2 _onSwipeEnd = new Events.Vector2();

        private readonly Events.Vector2 _onSwipeStart = new Events.Vector2();
        private Vector2 _lastPosition = Vector2.zero;
        public TriggerState TriggerState { get; set; } = TriggerState.Default;
        public float DragSpeed { get; private set; }

        /// <summary>
        ///     <para> OnBeginDrag </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            TriggerState = TriggerState.OnDrag;
            _lastPosition = eventData.position;
            _onSwipeStart.Invoke(eventData.position);
        }

        /// <summary>
        ///     <para> OnDrag </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            var direction = eventData.position - _lastPosition;
            _lastPosition = eventData.position;

            _onSwipe.Invoke(direction);
            DragSpeed = direction.y;
        }

        /// <summary>
        ///     <para> OnEndDrag </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            TriggerState = TriggerState.Default;
            _onSwipeEnd.Invoke(eventData.position);
        }

        /// <summary>
        ///     <para> OnPointerDown </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            IOnPause();
        }

        /// <summary>
        ///     <para> OnPointerUp </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            IOnResume();
        }

        /// <summary>
        ///     <para> IOnPause </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void IOnPause()
        {
            TriggerState = TriggerState.OnPause;
        }

        /// <summary>
        ///     <para> IOnResume </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void IOnResume()
        {
            TriggerState = TriggerState.OnResume;
        }
    }
}