using UnityEngine;

namespace Override
{
	/// <summary>
	///     <para> ClipManager </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[RequireComponent(typeof(AudioClip))]
	public class ClipManager : MonoBehaviour
	{
		private void Update()
		{
			Debug.Log(AudioManager.GetSoundVolume());
		}
	}
}