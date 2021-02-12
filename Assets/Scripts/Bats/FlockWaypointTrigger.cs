using UnityEngine;

namespace Bats
{
	
	/// <summary>
	///     <para> FlockWaypointTrigger </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class FlockWaypointTrigger : MonoBehaviour
	{
		public float timer = 1.0f;
		public FlockChild flockChild;
		
		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Start()
		{
			if (flockChild == null)
				flockChild = transform.parent.GetComponent<FlockChild>();
			var triggerTime = Random.Range(this.timer, this.timer * 3);
			InvokeRepeating(nameof(Trigger), triggerTime, triggerTime);
		}
		
		/// <summary>
		///     <para> Trigger </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Trigger()
		{
			flockChild.Wander(0.0f);
		}
	}
}