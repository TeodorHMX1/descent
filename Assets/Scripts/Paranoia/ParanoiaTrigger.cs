using UnityEngine;

namespace Paranoia
{
	/// <summary>
	///     <para> ParanoiaTrigger </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class ParanoiaTrigger : MonoBehaviour
	{
		private Collider _collider;
		private bool _wasCollided;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			_collider = GetComponent<Collider>();
		}

		/// <summary>
		///     <para> OnCollisionEnter </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="collision"></param>
		private void OnCollisionEnter(Collision collision)
		{
			Physics.IgnoreCollision(collision.collider, _collider);
		}

		/// <summary>
		///     <para> OnTriggerEnter </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="collision"></param>
		private void OnTriggerEnter(Collider collision)
		{
			if (collision.gameObject.name != "Player") return;
			_wasCollided = !_wasCollided;
		}

		/// <summary>
		///     <para> GETWasCollided </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <returns></returns>
		public bool GETWasCollided()
		{
			return _wasCollided;
		}
	}
}