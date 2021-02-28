using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    /// <summary>
    ///     <para> MainMenu </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class CreditsScreen : MonoBehaviour
    {
        [Header("Animator")]
        public CreditsTrigger creditsTrigger;
        public GameObject animationHolder;

        [Header("Main Menu Items")]
        public GameObject mainMenu;
        public GameObject optionsMenu;

        [Header("Customization")] [Range(.1f, 2f)]
        public float speed = 1f;
        
        private AnimationClip _clip;

        /// <summary>
        ///     <para> OnEnable </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void OnEnable()
        {
            var position = animationHolder.transform.localPosition;
            position = new Vector3(
                position.x,
                - 1290f,
                position.z
            );
            animationHolder.transform.localPosition = position;
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            var position = animationHolder.transform.localPosition;
            switch (creditsTrigger.TriggerState)
            {
                case TriggerState.Default:
                    position = new Vector3(
                        position.x,
                        position.y+1 * speed,
                        position.z
                    );
                    animationHolder.transform.localPosition = position;
                    break;
                case TriggerState.OnPause:
                    animationHolder.transform.localPosition = position;
                    break;
                case TriggerState.OnResume:
                    animationHolder.transform.localPosition = position;
                    creditsTrigger.TriggerState = TriggerState.Default;
                    break;
                case TriggerState.OnDrag:
                    position = new Vector3(
                        position.x,
                        position.y + (creditsTrigger.DragSpeed * speed),
                        position.z
                    );
                    animationHolder.transform.localPosition = position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!(position.y > 1410f)) return;
            
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}