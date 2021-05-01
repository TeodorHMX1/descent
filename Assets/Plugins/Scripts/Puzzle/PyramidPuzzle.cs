using System;
using System.Collections.Generic;
using System.Linq;
using Menu;
using UnityEngine;
using UnityEngine.Serialization;

namespace Puzzle
{
    /// <summary>
    ///     <para> PyramidPuzzle </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public enum PyramidModel
    {
        Default,
        FlashNext,
    }

    /// <summary>
    ///     <para> PyramidPuzzle </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class PyramidPuzzle : MonoBehaviour
    {
        [Header("Pyramids")]
        public List<PyramidController> pyramids = new List<PyramidController>();

        [Header("On Win")]
        public GameObject pickableObject;
        public Light light;
        public SlidingDoor slidingDoor;

        [Header("Model")]
        public PyramidModel pyramidModel = PyramidModel.Default;

        private bool _onWinCreated;
        private bool _isPickableObjectNotNull;
        private bool _isLightNotNull;
        private bool _isSlidingDoorNotNull;

        /// <summary>
        ///     <para> Start </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Start()
        {
            _isLightNotNull = light != null;
            _isPickableObjectNotNull = pickableObject != null;
            _isSlidingDoorNotNull = slidingDoor != null;
            if (_isLightNotNull)
            {
                light.enabled = false;
            }
            if (_isSlidingDoorNotNull)
            {
                slidingDoor.Closed();
            }
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            if (Pause.IsPaused)
            {
                return;
            }
            if (pyramids.Count(pyramid => pyramid.IsWinState()) < pyramids.Count) return;

            if (!IsCompleted())
            {
                if (_isLightNotNull)
                {
                    light.enabled = false;
                }
                if (_isSlidingDoorNotNull)
                {
                    slidingDoor.Closed();
                }
                return;
            }

            _onWinCreated = true;
            if (_isLightNotNull)
            {
                light.enabled = true;
            }
            if (_isSlidingDoorNotNull)
            {
                slidingDoor.Opened();
            }
            
            if (!_isPickableObjectNotNull) return;
            if (pickableObject.GetComponent<IOnComplete>() != null)
                pickableObject.GetComponent<IOnComplete>().ONCompleted();
        }

        /// <summary>
        ///     <para> IsCompleted </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <returns param="isCompleted"></returns>
        public bool IsCompleted()
        {
            return !(pyramids.Count(pyramid => pyramid.IsWinState()) < pyramids.Count);
        }


        /// <summary>
        ///     <para> Reset </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void Reset()
        {
            foreach (var pyramid in pyramids)
            {
                pyramid.Reset();
            }
            if (_isLightNotNull)
            {
                light.enabled = false;
            }
        }
    }
}