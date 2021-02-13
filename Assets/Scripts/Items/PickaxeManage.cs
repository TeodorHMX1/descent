using Destructible;
using UnityEngine;
using Walls;
using ZeoFlow.Pickup;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
	/// <summary>
	///     <para> PickaxeManage </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class PickaxeManage : MonoBehaviour, IOnAttached
	{

		public Animation animator;
		public KeyCode swingKey = KeyCode.Mouse0;
		public MeleeArea meleeArea;
		public AudioClip sound;
		
		private AudioSource _audioData;
		private bool _isAnimatorNull;
		private bool _isBoxColliderNotNull;
		private BoxCollider _boxCollider;
		
		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			_boxCollider = GetComponent<BoxCollider>();
			_isBoxColliderNotNull = _boxCollider != null;
			_isAnimatorNull = animator == null;
			_audioData = GetComponent<AudioSource>();
		}

		/// <summary>
		///     <para> ONUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			if (_isBoxColliderNotNull) _boxCollider.enabled = false;
			
			if (_isAnimatorNull) return;
			
			if (!Input.GetKeyDown(swingKey)) return;
			if (animator.isPlaying) return;
			
			animator.Play(Constants.Animations.PickaxeSwing);
			_audioData.PlayOneShot(sound);
			Invoke(nameof(BroadcastMeleeDamage), .2f);
		}

		/// <summary>
		///     <para> BroadcastMeleeDamage </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void BroadcastMeleeDamage()
		{
			if (meleeArea == null) return;
			meleeArea.OnMeleeDamage();
		}
	}
}