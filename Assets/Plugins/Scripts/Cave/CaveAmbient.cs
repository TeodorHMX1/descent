using System;
using Menu;
using Override;
using UnityEngine;

namespace Cave
{
	/// <summary>
	///     <para> CaveAmbient </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class CaveAmbient : MonoBehaviour
    {
        [Header("Sound")]
        public AudioClip ambientSound;
        public SoundVolume volume = SoundVolume.Weak;
        
        [Header("Visuals")]
        public Color color = new Color(68, 68, 68, 221);

        private string _id;

        private void Start()
        {
            _id = AudioInstance.ID();
        }

        private void Update()
        {
            if (Pause.IsPaused) return;

            new AudioBuilder()
                .WithClip(ambientSound)
                .WithName("Cave_AmbientSound_" + _id)
                .WithVolume(volume)
                .Play(true);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, .3f);
        }
    }
}