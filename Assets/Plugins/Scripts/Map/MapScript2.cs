using Items;
using Menu;
using Override;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Pickup;

namespace Map
{
    /// <summary>
    ///     <para> MapScript2 </para>
    /// </summary>
    public class MapScript2 : MonoBehaviour
    {
        public static bool ColArea1;
        public static bool ColArea2;
        public static bool ColArea3;
        public bool mapOpen;
        public Pause pause;
        public RigidbodyPickUp rigidbodyPickUp;
        public GameObject map;
        public GameObject area1;
        public GameObject area2;
        public GameObject area3;
        public AudioClip scribble;
        private bool _isArea1Null;
        private bool _isArea2Null;
        private bool _isArea3Null;
        private bool _isRigidbodyPickUpNotNull;
        private bool _isPauseNotNull;

        private static MapScript2 _mInstance;

        private void Awake()
        {
            if (_mInstance == null)
            {
                _mInstance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public static bool IsMapOpened()
        {
            return _mInstance != null && _mInstance.mapOpen;
        }

        /// <summary>
        ///     <para> Start </para>
        /// </summary>
        private void Start()
        {
            _isPauseNotNull = pause != null;
            _isRigidbodyPickUpNotNull = rigidbodyPickUp != null;
            _isArea1Null = area1 == null;
            _isArea2Null = area2 == null;
            _isArea3Null = area3 == null;
            mapOpen = false;
            ColArea1 = false;
            ColArea2 = false;
            ColArea3 = false;
            map.SetActive(false);

            if (!_isArea1Null)
            {
                area1.SetActive(false);
            }

            if (!_isArea2Null)
            {
                area2.SetActive(false);
            }

            if (!_isArea3Null)
            {
                area3.SetActive(false);
            }
        }

        /// <summary>
        ///     <para> Update </para>
        /// </summary>
        private void Update()
        {
            if (Time.timeScale == 0f) return;
            if (!ItemsManager.Unlocked(Item.Map)) return;
            if (_isArea1Null || _isArea2Null || _isArea3Null) return;

            area1.SetActive(ColArea1);
            area2.SetActive(ColArea2);
            area3.SetActive(ColArea3);

            if (InputManager.GetButtonDown("Map"))
            {
                mapOpen = !mapOpen;
                new AudioBuilder()
                    .WithClip(scribble)
                    .WithName("ToggleMap_Sound")
                    .WithVolume(SoundVolume.Normal)
                    .Play();
            }

            if (_isRigidbodyPickUpNotNull) rigidbodyPickUp.hideCrosshair = mapOpen;

            if (_isPauseNotNull)
            {
                if (pause.isPaused)
                {
                    area1.SetActive(false);
                    area2.SetActive(false);
                    area3.SetActive(false);
                    map.SetActive(false);
                }
                else if (mapOpen)
                {
                    Open_Map();
                }
                else
                {
                    area1.SetActive(false);
                    area2.SetActive(false);
                    area3.SetActive(false);
                    Exit_Map();
                }
            }
            else
            {
                if (mapOpen)
                {
                    Open_Map();
                }
                else
                {
                    area1.SetActive(false);
                    area2.SetActive(false);
                    area3.SetActive(false);
                    Exit_Map();
                }
            }
        }

        /// <summary>
        ///     <para> Exit_Map </para>
        /// </summary>
        private void Exit_Map()
        {
            map.SetActive(false);
            mapOpen = false;
        }

        /// <summary>
        ///     <para> Open_Map </para>
        /// </summary>
        private void Open_Map()
        {
            map.SetActive(true);
            mapOpen = true;
        }
    }
}