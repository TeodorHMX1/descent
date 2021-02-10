using UnityEngine;

/// <summary>
///     <para> SceneGameOptions </para>
///     <author> @TeodorHMX1 </author>
/// </summary>
public class SyncModel : MonoBehaviour
{
	public GameObject objectAttached;

	/// <summary>
	///     <para> Update </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Update()
	{
		transform.rotation = objectAttached.transform.rotation;
	}
}