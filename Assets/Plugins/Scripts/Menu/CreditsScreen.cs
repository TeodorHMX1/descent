using System;
using UnityEngine;

namespace Menu
{
    /// <summary>
    ///     <para> MainMenu </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class CreditsScreen : MonoBehaviour
    {
        public Animation animationCredits;

        [Header("Main Menu Items")]
        public GameObject mainMenu;
        public GameObject optionsMenu;
        
        private AnimationClip _animationClip;
        private AnimationClip _clip;

        /// <summary>
        ///     <para> OnEnable </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void OnEnable()
        {
            // create a new AnimationClip
            _clip = new AnimationClip {legacy = true};

            // create a curve to move the GameObject and assign to the clip
            var keys = new Keyframe[2];
            keys[0] = new Keyframe(0.0f, -1030f);
            keys[1] = new Keyframe(26.0f, 1340f);
            var curve = new AnimationCurve(keys);
            _clip.SetCurve("", typeof(Transform), "localPosition.y", curve);
            
            // now animate the GameObject
            animationCredits.AddClip(_clip, _clip.name);
            animationCredits.Play(_clip.name);
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            if (animationCredits.isPlaying) return;
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}