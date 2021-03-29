using UnityEngine;

namespace Destructible
{
	public class AutoDamage : MonoBehaviour
	{
		public int startAtHitPoints = 30;
		public float damageIntervalSeconds = 0.5f;
		public int damagePerInterval = 5;

		private bool _isInitialized;
		private Destructible _destructible;
		private bool _autoDamageStarted;
		private bool _isDestructibleNull;

		private void Start()
		{
			_destructible = gameObject.GetComponent<Destructible>();
			_isDestructibleNull = _destructible == null;
			if (_destructible == null)
			{
				Debug.LogWarning("No Destructible object found! AutoDamage removed.");
				Destroy(this);
			}

			_isInitialized = true;
		}

		private void Update()
		{
			if (!_isInitialized) return;
			if (_isDestructibleNull) return;
			if (_autoDamageStarted) return;

			if (!(_destructible.currentHitPoints <= startAtHitPoints)) return;
			InvokeRepeating(nameof(ApplyDamage), 0f, damageIntervalSeconds);
			_autoDamageStarted = true;
		}

		private void ApplyDamage()
		{
			if (_destructible == null) return;

			_destructible.ApplyDamage(damagePerInterval);
		}
	}
}