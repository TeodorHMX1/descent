using UnityEngine;
using UnityEngine.UI;

namespace Options
{
	[RequireComponent(typeof(Button))]
	[AddComponentMenu("Options/Button Select")]
	public class ButtonSelect : MonoBehaviour
	{
		private void Start()
		{
			GetComponent<Button>().Select();
		}
	}
}