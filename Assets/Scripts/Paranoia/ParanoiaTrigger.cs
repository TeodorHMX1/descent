using UnityEngine;

namespace Paranoia
{
	public class ParanoiaTrigger : MonoBehaviour
	{
		private Collider _collider;
		private bool _wasCollided;

		private void Start()
		{
			_collider = GetComponent<Collider>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			Physics.IgnoreCollision(collision.collider, _collider);
		}

		private void OnTriggerEnter(Collider collision)
		{
			if (collision.gameObject.name != "Player") return;
			_wasCollided = !_wasCollided;
		}

		public bool GETWasCollided()
		{
			return _wasCollided;
		}
	}
}