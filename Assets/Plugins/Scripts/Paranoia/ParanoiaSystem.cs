using System.Collections.Generic;
using Menu;
using UnityEngine;

namespace Paranoia
{
    /// <summary>
    ///     <para> HeartbeatSpeed </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public enum HeartbeatSpeed
    {
        Normal = 10,
        NormalV2 = 12,
        NormalV3 = 14,
        Increased = 16,
        IncreasedV2 = 18,
        IncreasedV3 = 20,
        MaxSpeed = 22
    }

    /// <summary>
    ///     <para> ParanoiaSystem </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class ParanoiaSystem : MonoBehaviour
    {
        public ParanoiaEntrances numberOfEntrances = ParanoiaEntrances.Two;
        public List<ParanoiaTrigger> paranoiaTriggers = new List<ParanoiaTrigger>();
        public ParanoiaHolder paranoiaHolder;

        private ParanoiaState _paranoiaBoxState = ParanoiaState.Outside;
        private int _systemId = -1;

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            if (Pause.IsPaused || _systemId == -1) return;

            switch (numberOfEntrances)
            {
                // 1 entry
                case ParanoiaEntrances.One:
                {
                    var trigger = paranoiaTriggers[0];
                    _paranoiaBoxState = trigger.GETWasCollided() ? ParanoiaState.Inside : ParanoiaState.Outside;
                    break;
                }
                // 2 entries
                case ParanoiaEntrances.Two:
                {
                    var trigger1 = paranoiaTriggers[0];
                    var trigger2 = paranoiaTriggers[1];

                    if (trigger1.GETWasCollided() && !trigger2.GETWasCollided())
                        _paranoiaBoxState = ParanoiaState.Inside;
                    else
                        _paranoiaBoxState = ParanoiaState.Outside;

                    break;
                }
                case ParanoiaEntrances.Three:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
                case ParanoiaEntrances.Four:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
                case ParanoiaEntrances.Five:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
                default:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
            }

            paranoiaHolder.SetBoxState(_systemId, _paranoiaBoxState);
        }

        /// <summary>
        ///     <para> SetId </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void SetId(int id)
        {
            _systemId = id;
        }
    }
}