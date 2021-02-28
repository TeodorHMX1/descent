using System;
using Destructible;
using Override;
using UnityEngine;
using UnityEngine.Rendering;
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
		public MeshRenderer objMeshRenderer;

		private bool _isAttached;
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
		}

		private void Update()
		{
			if (!_isAttached) return;
			
			if (_isBoxColliderNotNull) _boxCollider.enabled = false;
			
			if (_isAnimatorNull) return;
			
			if (!Input.GetKeyDown(swingKey)) return;
			if (animator.isPlaying) return;
			
			animator.Play(Constants.Animations.PickaxeSwing);
			new AudioBuilder()
				.WithClip(sound)
				.WithName("Pickaxe_Swing")
				.WithVolume(SoundVolume.Normal)
				.Play();
			Invoke(nameof(BroadcastMeleeDamage), .2f);
		}

		/// <summary>
		///     <para> ONUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
            if (_isAttached) return;
            _isAttached = true;
            objMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
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

		public bool IsAttached => _isAttached;
	}
}