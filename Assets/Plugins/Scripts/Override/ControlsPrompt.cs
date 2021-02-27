using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZeoFlow;
using ZeoFlow.PlayerMovement;

namespace Override
{
	/// <summary>
	///     <para> ControlsPrompt </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class ControlsPrompt : MonoBehaviour
	{

		public Text fullControls;
		public Text minimizedControls;
		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			minimizedControls.enabled = Time.timeScale != 0f;
			fullControls.enabled = Time.timeScale != 0f;
			
			var controlsContent = "\"" + InputManager.GetKeyCode("Jump") + "\" - Jump\n";
			controlsContent += "\"" + InputManager.GetKeyCode("Run") + "\" - Run\n";
			controlsContent += "\"" + InputManager.GetKeyCode("Interact") + "\" - Interact\n";
			controlsContent += "\"" + InputManager.GetKeyCode("Map") + "\" - Show/Hide the Map\n";
			controlsContent += "\"" + InputManager.GetKeyCode("Flashlight") + "\" - Switch Flashlight On/Off\n";
			controlsContent += "\"" + InputManager.GetKeyCode("SwitchTool") + "\" - Switch Element in Hand\n";
			controlsContent += "\"" + InputManager.GetKeyCode("PauseMenu") + "\" - Pause Game\n";
			
			minimizedControls.text = "Press \"" + InputManager.GetKeyCode("ToggleControls") + "\" for controls";
			fullControls.text = controlsContent;
		}
	}
}