using Menu;
using Override;
using UnityEngine;
using UnityEngine.Rendering;
using ZeoFlow;
using ZeoFlow.Pickup;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
    /// <summary>
    ///     <para> HelmetManage </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class HelmetManage : MonoBehaviour, IOnAttached
    {
        public Light helmetLight;
        public float lightIntensity = 1f;
        public bool attached;
        public AudioClip torchOn;
        public AudioClip torchOff;
        public MeshRenderer objMeshRenderer;

        // unity 1 = 1frame
        private readonly FlashPattern[] _lightPattern =
        {
            // initial battery capacity
            new FlashPattern {IsDark = false, Time = 400, Intensity = 1f},

            // light pattern (first light then dark and so on)
            new FlashPattern {IsDark = false, Time = 120, Intensity = 1f},
            new FlashPattern {IsDark = true, Time = 10, Intensity = 0f},
            new FlashPattern {IsDark = false, Time = 120, Intensity = 0.8f},
            new FlashPattern {IsDark = true, Time = 35, Intensity = 0f},
            new FlashPattern {IsDark = false, Time = 100, Intensity = 0.6f},
            new FlashPattern {IsDark = true, Time = 50, Intensity = 0f},
            new FlashPattern {IsDark = false, Time = 60, Intensity = 0.4f},

            // end pattern
            new FlashPattern {IsDark = true, Time = 0}
        };

        private BoxCollider _boxCollider;
        private FlashlightState _flashlightState = FlashlightState.None;
        private int _index;
        private bool _isBoxColliderNotNull;
        private bool _outOfBattery;
        private int _timer;
        private bool FlashlightOn { get; set; }

        /// <summary>
        ///     <para> Start </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _isBoxColliderNotNull = _boxCollider != null;

            _lightPattern[0].Intensity = lightIntensity;
            FlashlightOn = helmetLight.enabled;
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            if (Pause.IsPaused || !attached) return;

            if (_isBoxColliderNotNull) _boxCollider.enabled = false;

            if (InputManager.GetButtonDown("Flashlight"))
            {
                if (!FlashlightOn && _flashlightState <= FlashlightState.Triggered)
                {
                    helmetLight.intensity = _lightPattern[0].Intensity;
                    new AudioBuilder()
                        .WithClip(torchOn)
                        .WithName("Torch_Toggle")
                        .WithVolume(SoundVolume.Normal)
                        .Play();
                    helmetLight.enabled = true;
                    FlashlightOn = true;
                    if (_flashlightState == FlashlightState.Triggered)
                    {
                        _flashlightState = FlashlightState.AnimationStarted;
                        _index = 0;
                        _timer = 0;
                    }
                }
                else
                {
                    new AudioBuilder()
                        .WithClip(torchOff)
                        .WithName("Torch_Toggle")
                        .WithVolume(SoundVolume.Normal)
                        .Play();
                    helmetLight.enabled = false;
                    FlashlightOn = false;
                    if (_flashlightState == FlashlightState.Triggered ||
                        _flashlightState == FlashlightState.AnimationStarted)
                        _flashlightState = FlashlightState.OutOfBattery;
                }
            }

            if (_flashlightState == FlashlightState.AnimationStarted) FlashlightEffect();
        }

        /// <summary>
        ///     <para> ONUpdate </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="playerAttachMenu"></param>
        public void ONUpdate(PlayerAttachSub playerAttachMenu)
        {
            if (attached) return;
            attached = true;
            objMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        /// <summary>
        ///     <para> FlashlightEffect </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void FlashlightEffect()
        {
            _timer++;
            if (_timer < _lightPattern[_index].Time) return;
            _timer = 0;

            _index++;
            if (_index < _lightPattern.Length)
            {
                helmetLight.enabled = !_lightPattern[_index].IsDark;
                FlashlightOn = !_lightPattern[_index].IsDark;
                helmetLight.intensity = _lightPattern[_index].Intensity;
                return;
            }

            _index = 0;
            helmetLight.enabled = false;
            FlashlightOn = false;
            helmetLight.intensity = 0;
            _flashlightState = FlashlightState.OutOfBattery;
        }

        /// <summary>
        ///     <para> CanApplyEffect </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <returns param="_outOfBattery"></returns>
        public bool CanApplyEffect()
        {
            return _flashlightState == FlashlightState.OutOfBattery
                   || _flashlightState == FlashlightState.None && !FlashlightOn;
        }

        /// <summary>
        ///     <para> IsHelmetLightOn </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <returns param="FlashlightOn"></returns>
        public bool IsHelmetLightOn()
        {
            return FlashlightOn;
        }

        /// <summary>
        ///     <para> SetParanoiaTriggered </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void SetParanoiaTriggered()
        {
            if (_flashlightState >= FlashlightState.Triggered) return;
            _flashlightState = FlashlightOn ? FlashlightState.AnimationStarted : FlashlightState.Triggered;
            if (_flashlightState != FlashlightState.AnimationStarted) return;
            _index = 0;
            _timer = 0;
        }

        /// <summary>
        ///     <para> DisableParanoiaTriggered </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void DisableParanoiaTriggered()
        {
            _flashlightState = FlashlightState.None;
            helmetLight.intensity = _lightPattern[0].Intensity;
        }

        private enum FlashlightState
        {
            None = 0,
            Triggered = 1,
            AnimationStarted = 2,
            OutOfBattery = 3
        }
    }
}