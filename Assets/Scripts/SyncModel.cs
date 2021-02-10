using UnityEngine;

public class SyncModel : MonoBehaviour
{
	public GameObject objectAttached;

	/// <summary>
	/// Update()
	/// </summary>
	private void Update()
	{
		transform.rotation = objectAttached.transform.rotation;
	}
}