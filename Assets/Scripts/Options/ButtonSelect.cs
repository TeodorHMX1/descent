using UnityEngine;
using UnityEngine.UI;

namespace Options
{
	/// <summary>
	///     <para> ButtonSelect </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[RequireComponent(typeof(Button))]
	[AddComponentMenu("Options/Button Select")]
	public class ButtonSelect : MonoBehaviour
	{
		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			GetComponent<Button>().Select();
		}
	}
}