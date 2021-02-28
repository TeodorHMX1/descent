using System;
using Items;
using Map;
using TMPro;
using UnityEngine;
using ZeoFlow;

namespace Override
{
    /// <summary>
    ///     <para> ControlsPrompt </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class ControlsPrompt : MonoBehaviour
    {
        [Header("Full Controls")] public GameObject fullControls;
        public TextMeshProUGUI fullControlsContent;

        [Header("Minimized Controls")] public GameObject minimizedControls;
        public TextMeshProUGUI minimizedControlsContent;

        [Header("Options")] [Range(1, 20)] public int hideFullControls = 5;
        public AudioClip toggleSound;

        private bool _controlsVisible;
        private int timerAutoHide;

        private void Start()
        {
            fullControls.SetActive(false);
            minimizedControls.SetActive(true);
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            minimizedControlsContent.enabled = Time.timeScale != 0f;
            fullControlsContent.enabled = Time.timeScale != 0f;

            var controlsContent = "\"" + InputManager.GetKeyName("Run") + "\" - Run\n";
            controlsContent += "\"" + InputManager.GetKeyName("Jump") + "\" - Jump\n";
            controlsContent += "\"" + InputManager.GetKeyName("Interact") + "\" - Interact\n";
            if (ItemsManager.Unlocked(Item.Map))
            {
                controlsContent += "\"" + InputManager.GetKeyName("Map") + "\" - Show/Hide the Map\n";
            }
            if (ItemsManager.Unlocked(Item.Pickaxe) || ItemsManager.Unlocked(Item.Flare))
            {
                controlsContent += "\"" + InputManager.GetKeyName("InteractHUD") + "\" - Use the Tool\n";
                controlsContent += "\"" + InputManager.GetKeyName("SwitchTool") + "\" - Switch Element in Hand\n";
            }
            if (ItemsManager.Unlocked(Item.Flashlight))
            {
                controlsContent += "\"" + InputManager.GetKeyName("Flashlight") + "\" - Switch Flashlight On/Off\n";
            }
            controlsContent += "\"" + InputManager.GetKeyName("PauseMenu") + "\" - Pause Game\n";

            minimizedControlsContent.text = "Press \"" + InputManager.GetKeyName("ToggleControls") + "\" for controls";
            fullControlsContent.text = controlsContent;

            if (MapScript2.IsMapOpened())
            {
                minimizedControls.SetActive(false);
                fullControls.SetActive(false);
                return;
            }

            if (timerAutoHide == hideFullControls * 60)
            {
                _controlsVisible = false;
            }
            else
            {
                timerAutoHide++;
            }


            if (InputManager.GetButtonDown("ToggleControls"))
            {
                _controlsVisible = !_controlsVisible;
                new AudioBuilder()
                    .WithClip(toggleSound)
                    .WithName("ToggleControls")
                    .WithVolume(SoundVolume.Normal)
                    .Play();
                if (_controlsVisible) timerAutoHide = 0;
            }

            if (_controlsVisible)
            {
                minimizedControls.SetActive(false);
                fullControls.SetActive(true);
            }
            else
            {
                fullControls.SetActive(false);
                minimizedControls.SetActive(true);
            }
        }
    }
}