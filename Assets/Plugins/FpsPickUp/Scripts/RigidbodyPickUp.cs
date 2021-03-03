using Map;
using Menu;
using Override;
using TMPro;
using UnityEngine;
using static UnityEngine.Screen;

namespace ZeoFlow.Pickup
{
    [AddComponentMenu("Character/Rigidbody Pick-Up")]
    [RequireComponent(typeof(AudioSource))]
    public class RigidbodyPickUp : MonoBehaviour
    {
        public KeyCode pickupButton = KeyCode.E;
        public string mouseHorizontalAxis = "Mouse X";
        public KeyCode pauseButton = KeyCode.Escape;
        public bool togglePickUp;
        public float distance = 3f;
        public float maxDistanceHeld = 4f;
        public float maxDistanceGrab = 10f;
        public GameObject playerCam;
        public CrosshairSystem crosshairsSystem = new CrosshairSystem();
        public AudioSoundsSub audioSystem = new AudioSoundsSub();

        [Space] public bool hideCrosshair;

        private bool _objectIsToggled;
        private float _toggTime = 0.01f;
        private float _timeHeld = 0.01f;
        private bool _objectDefaultGravity = true;
        private Ray _playerAim;
        private GameObject _objectHeld;
        private bool _isObjectHeld;
        private bool _isPuzzleFocused;
        private bool _objectCan;
        private float _intTimeHeld;
        private PickableObject[] _zPickableObject;
        private GameObject[] _pickableObjs;
        private PhysicsSub _physicsMenu = new PhysicsSub();
        private PuzzleSub _puzzleSub = new PuzzleSub();
        private ThrowingSystemMenu _throwingSystem = new ThrowingSystemMenu();
        private bool _isLeftMovement;
        private bool _isRightMovement;
        private int _timeStartedMovement;
        private OutlinerSub _outlinerMenu;
        private PickableObject _playerObject;
        private bool _isGuiHolderNotNull;
        private TextMeshProUGUI _guiText;

        private void Start()
        {
            ResetPickUp(false);
            _timeHeld = 0.01f;
            _intTimeHeld = _timeHeld;
            _throwingSystem.defaultThrowTime = _throwingSystem.throwTime;

            if (crosshairsSystem.enabled)
            {
                if (crosshairsSystem.onDefault == null)
                {
                    crosshairsSystem.onDefault = Resources.Load<Texture2D>("Crosshair/crosshair_default");
                }

                if (crosshairsSystem.onAble == null)
                {
                    crosshairsSystem.onAble = Resources.Load<Texture2D>("Crosshair/crosshair_able");
                }

                if (crosshairsSystem.onGrab == null)
                {
                    crosshairsSystem.onGrab = Resources.Load<Texture2D>("Crosshair/crosshair_grab");
                }

                if (crosshairsSystem.onPuzzle == null)
                {
                    crosshairsSystem.onPuzzle = Resources.Load<Texture2D>("Crosshair/crosshair_puzzle");
                }

                _isGuiHolderNotNull = crosshairsSystem.guiHolder != null;
                if (_isGuiHolderNotNull)
                {
                    var textTemp = crosshairsSystem.guiPrefab;
                    textTemp = Instantiate(textTemp);
                    var obj = Instantiate(textTemp, new Vector3(0, 0, 0), Quaternion.identity);
                    obj.transform.SetParent(crosshairsSystem.guiHolder.transform, false);
                    obj.name = "TextHolder";
                    _guiText = obj.GetComponent<TextMeshProUGUI>();
                }
            }

            if (audioSystem.enabled)
            {
                if (audioSystem.pickedUpAudio == null)
                {
                    audioSystem.pickedUpAudio = Resources.Load<AudioClip>("Audio/Rigid_PickUp");
                }

                if (audioSystem.throwAudio == null)
                {
                    audioSystem.throwAudio = Resources.Load<AudioClip>("Audio/Rigid_Dropped");
                }
            }
        }

        private void Update()
        {
            if (Pause.IsPaused)
            {
                if (_outlinerMenu != null)
                {
                    if (_outlinerMenu.enabled)
                    {
                        _outlinerMenu.outline.enabled = false;
                    }
                }
            }

            if (!_isPuzzleFocused) return;

            var inputX = InputManager.GetAxisRaw(mouseHorizontalAxis);

            if (inputX < 0)
            {
                if (_timeStartedMovement != 0 && !_isLeftMovement)
                {
                    _timeStartedMovement = 0;
                }

                _timeStartedMovement++;
                _isLeftMovement = true;
                _isRightMovement = false;
            }
            else if (inputX > 0)
            {
                if (_timeStartedMovement != 0 && !_isRightMovement)
                {
                    _timeStartedMovement = 0;
                }

                _timeStartedMovement++;
                _isLeftMovement = false;
                _isRightMovement = true;
            }

            if (_timeStartedMovement < 10) return;

            if (_playerObject == null) return;

            var isRight = _isRightMovement && !_isLeftMovement;
            _playerObject.OnMovement(isRight);
            _isLeftMovement = false;
            _isRightMovement = false;
            _timeStartedMovement = 0;
            _isPuzzleFocused = false;
        }

        private void LateUpdate()
        {
            _zPickableObject = FindObjectsOfType<PickableObject>();

            if (_isObjectHeld && _physicsMenu.placeObjectBack)
            {
                if (Vector3.Distance(_objectHeld.GetComponent<Placeback>().pos, _objectHeld.transform.position) <
                    _physicsMenu.placeDistance)
                {
                    _physicsMenu.canPlaceBack = true;
                }
                else if (Vector3.Distance(_objectHeld.GetComponent<Placeback>().pos, _objectHeld.transform.position) >
                         _physicsMenu.placeDistance)
                {
                    _physicsMenu.canPlaceBack = false;
                }
            }

            if (_objectHeld == null)
            {
                _isObjectHeld = false;
                if (togglePickUp)
                {
                    _toggTime = 0.01f;
                    _objectIsToggled = false;
                }
            }
        }

        private void FixedUpdate()
        {
            var playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

            if (Physics.Raycast(playerAim, out var hit, maxDistanceGrab - 1.5f))
            {
                _objectCan = hit.collider.GetComponent(typeof(PickableObject)) != null;
                if (_objectCan)
                {
                    var pickableObject = hit.collider.gameObject.GetComponent<PickableObject>();
                    if (_isGuiHolderNotNull)
                    {
                        if (pickableObject != null)
                        {
                            _guiText.text = pickableObject.guiText;
                        }
                    }
                }
            }
            else
            {
                _objectCan = false;
            }

            if (_objectCan)
            {
                if (_outlinerMenu != null)
                {
                    if (_outlinerMenu != hit.collider.GetComponent<PickableObject>().outlinerMenu)
                    {
                        _outlinerMenu.outline.enabled = false;
                    }
                }
                _outlinerMenu = hit.collider.GetComponent<PickableObject>().outlinerMenu;
            }

            if (_isObjectHeld && _objectIsToggled)
            {
                HoldObject();
                _toggTime -= Time.deltaTime;
            }

            if (Input.GetKeyDown(_throwingSystem.throwButton) && !_throwingSystem.throwing && _throwingSystem.enabled &&
                _objectHeld != null)
            {
                ThrowObject();
            }

            if (_throwingSystem.throwing)
            {
                _throwingSystem.throwTime -= Time.deltaTime;
                if (_throwingSystem.throwTime < 0)
                {
                    _throwingSystem.throwing = false;
                    _throwingSystem.throwTime = _throwingSystem.defaultThrowTime;
                    _throwingSystem.obj = null;
                }
            }

            pickupButton = InputManager.GetKeyCode("Interact");

            if (Input.GetKeyUp(pickupButton))
            {
                _isPuzzleFocused = false;
                _timeStartedMovement = 0;
            }

            if (MapScript2.IsMapOpened())
            {
                if (_outlinerMenu == null) return;
                if (!_outlinerMenu.enabled) return;
                _objectHeld = null;
                _outlinerMenu.outline.enabled = false;
                _outlinerMenu = null;
                return;
            }
            switch (togglePickUp)
            {
                case true:
                {
                    if (Input.GetKeyDown(pickupButton) && !_throwingSystem.throwing && !_isObjectHeld &&
                        !_objectIsToggled &&
                        _toggTime > 0.009f)
                    {
                        TryPickObject();
                    }

                    if (Input.GetKeyDown(pickupButton) && _isObjectHeld && _objectIsToggled && _toggTime < 0)
                    {
                        if (_physicsMenu.placeObjectBack &&
                            Vector3.Distance(_objectHeld.GetComponent<Placeback>().pos, _objectHeld.transform.position) <
                            _physicsMenu.placeDistance)
                        {
                            _objectHeld.transform.position = _objectHeld.GetComponent<Placeback>().pos;
                            _objectHeld.transform.rotation = _objectHeld.GetComponent<Placeback>().rot;
                            ResetPickUp(true);
                            _toggTime = 0.01f;
                        }
                        else
                        {
                            ResetPickUp(true);
                            _toggTime = 0.01f;
                        }
                    }

                    break;
                }
                case false when Input.GetKey(pickupButton) && !_throwingSystem.throwing:
                {
                    switch (_isObjectHeld)
                    {
                        case false:
                            TryPickObject();
                            break;
                        case true:
                            HoldObject();
                            break;
                    }

                    break;
                }
                case false:
                {
                    if (!Input.GetKey(pickupButton) && _isObjectHeld)
                    {
                        if (_physicsMenu.placeObjectBack &&
                            Vector3.Distance(_objectHeld.GetComponent<Placeback>().pos, _objectHeld.transform.position) <
                            _physicsMenu.placeDistance)
                        {
                            _objectHeld.transform.position = _objectHeld.GetComponent<Placeback>().pos;
                            _objectHeld.transform.rotation = _objectHeld.GetComponent<Placeback>().rot;
                            ResetPickUp(true);
                        }

                        ResetPickUp(true);
                    }

                    break;
                }
            }
        }

        private void TryPickObject()
        {
            var playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Physics.Raycast(playerAim, out var hit);

            if (hit.collider == null) return;
            var isPickupObj = hit.collider.GetComponent(typeof(PickableObject)) != null;
            if (!isPickupObj) return;
            foreach (var pickedObject in _zPickableObject)
            {
                if (pickedObject.gameObject != hit.collider.gameObject) continue;
                if (!(Vector3.Distance(hit.collider.gameObject.transform.position,
                    playerCam.transform.position) <= maxDistanceGrab)) continue;
                if (_isPuzzleFocused && pickedObject.gameObject != _objectHeld) return;

                _playerObject = hit.collider.GetComponent<PickableObject>();

                if (_playerObject.onPickCallback)
                {
                    _playerObject.OnAttach();
                    _outlinerMenu = null;
                    _playerObject = null;
                    return;
                }
                _puzzleSub = _playerObject.puzzleSub;
                if (_puzzleSub.enabled)
                {
                    if (!_playerObject.IsPuzzleMoving())
                    {
                        _isPuzzleFocused = true;
                    }
                    else
                    {
                        _timeStartedMovement = 0;
                    }

                    return;
                }

                if (_playerObject.playerAttachMenu.attachToPlayer)
                {
                    _playerObject.OnAttach();
                    return;
                }

                _isObjectHeld = true;
                _objectHeld = pickedObject.gameObject;
                _objectDefaultGravity = _objectHeld.GetComponent<Rigidbody>().useGravity;
                _objectHeld.GetComponent<Rigidbody>().useGravity = false;
                _objectHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
                _physicsMenu = _playerObject.physicsMenu;
                _throwingSystem = _playerObject.throwingSystem;
                if (audioSystem.enabled)
                {
                    new AudioBuilder()
                        .WithClip(audioSystem.pickedUpAudio)
                        .WithName("RigidBodyPickUp_PickedUp")
                        .WithVolume(SoundVolume.Normal)
                        .Play();
                    audioSystem.letGoFired = false;
                }

                if (togglePickUp)
                {
                    _objectIsToggled = true;
                }

                if (_physicsMenu.placeObjectBack)
                {
                    if (_objectHeld.GetComponent<Placeback>() == null)
                    {
                        _objectHeld.AddComponent<Placeback>();
                    }
                }
            }
        }

        private void HoldObject()
        {
            Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            /*Finds the next position for the object held to move to, depending on the Camera's position
            ,direction, and distance the object is held between you two.*/
            Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
            //Takes the current position of the object held
            Vector3 currPos = _objectHeld.transform.position;
            _timeHeld = _timeHeld - 0.1f * Time.deltaTime;

            if (audioSystem.enabled && audioSystem.objectHeldAudio != null)
            {
                new AudioBuilder()
                    .WithClip(audioSystem.objectHeldAudio)
                    .WithName("RigidBodyPickUp_ObjectHeld")
                    .WithVolume(SoundVolume.Normal)
                    .Play();
            }

            /*Checking the distance between the player and the object held.
             * If the distance exceeds the 'maxDistanceHeld', it will let the object go. This also
             * stops a bug that forces objects through walls if you move back too far with an object held
             * maxDistanceGrab is how far you are able to grab an object, if it exceeds the amount, it won't do anything
             */
            if (Vector3.Distance(_objectHeld.transform.position, playerCam.transform.position) >
                maxDistanceGrab || _throwingSystem.throwing)
            {
                ResetPickUp(true);
            }

            //If an object is held, apply the object's placement.
            else if (_isObjectHeld)
            {
                if (Vector3.Distance(_objectHeld.transform.position, playerCam.transform.position) >
                    maxDistanceHeld && _timeHeld < 0)
                {
                    ResetPickUp(true);
                }
                else
                {
                    _objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;
                    if (_physicsMenu.keepRotation)
                    {
                        _physicsMenu.intRotation = _objectHeld.transform.rotation;
                    }

                    if (!_physicsMenu.objectRotated)
                    {
                        switch (_physicsMenu.objectDirection)
                        {
                            case PhysicsSub.objectRotation.TurnOnY:
                                _objectHeld.transform.eulerAngles =
                                    new Vector3(0, playerCam.transform.eulerAngles.y, 0);
                                break;
                            case PhysicsSub.objectRotation.FaceForward:
                                _objectHeld.transform.LookAt(playerCam.transform.position);
                                break;
                        }
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (Cursor.lockState == CursorLockMode.None || hideCrosshair)
            {
                if (_isGuiHolderNotNull)
                {
                    _guiText.enabled = false;
                }

                return;
            }

            switch (crosshairsSystem.enabled)
            {
                //Object Can Be Held Crosshair
                case true when _isPuzzleFocused:
                    GUI.DrawTexture(new Rect(
                            width / 2 - (crosshairsSystem.onPuzzle.width / 2),
                            height / 2 - (crosshairsSystem.onPuzzle.height / 2),
                            crosshairsSystem.onPuzzle.width,
                            crosshairsSystem.onPuzzle.height),
                        crosshairsSystem.onPuzzle);
                    if (_isGuiHolderNotNull)
                    {
                        _guiText.enabled = false;
                    }

                    break;
                //Object Is Being Held Crosshair
                case true when _isObjectHeld:
                    GUI.DrawTexture(new Rect(
                            width / 2 - (crosshairsSystem.onGrab.width / 2),
                            height / 2 - (crosshairsSystem.onGrab.height / 2),
                            crosshairsSystem.onGrab.width,
                            crosshairsSystem.onGrab.height),
                        crosshairsSystem.onGrab);
                    if (_isGuiHolderNotNull)
                    {
                        _guiText.enabled = false;
                    }

                    break;
                //Object Can Be Held Crosshair
                case true when _objectCan:
                    GUI.DrawTexture(new Rect(
                            width / 2 - (crosshairsSystem.onAble.width / 2),
                            height / 2 - (crosshairsSystem.onAble.height / 2),
                            crosshairsSystem.onAble.width,
                            crosshairsSystem.onAble.height),
                        crosshairsSystem.onAble);
                    if (_isGuiHolderNotNull)
                    {
                        _guiText.enabled = true;
                    }

                    if (_outlinerMenu != null)
                    {
                        if (_outlinerMenu.enabled)
                        {
                            _outlinerMenu.outline.enabled = true;
                        }
                    }

                    break;
                case true:
                {
                    if (!_isObjectHeld && !_objectCan) //Default Crosshair
                    {
                        if (crosshairsSystem.onDefault == null)
                        {
                            Debug.LogError("Crosshairs are null");
                        }
                        else
                        {
                            if (_outlinerMenu != null)
                            {
                                if (_outlinerMenu.enabled)
                                {
                                    _outlinerMenu.outline.enabled = false;
                                    _outlinerMenu = null;
                                }
                            }

                            if (_isGuiHolderNotNull)
                            {
                                _guiText.enabled = false;
                            }

                            GUI.DrawTexture(new Rect(
                                    width / 2 - (crosshairsSystem.onDefault.width / 2),
                                    height / 2 - (crosshairsSystem.onDefault.height / 2),
                                    crosshairsSystem.onDefault.width,
                                    crosshairsSystem.onDefault.height),
                                crosshairsSystem.onDefault);
                        }
                    }

                    break;
                }
            }
        }

        private void ThrowObject()
        {
            _throwingSystem.obj = _objectHeld;
            _throwingSystem.throwing = true;
            ResetPickUp(true);
            _throwingSystem.obj.GetComponent<Rigidbody>()
                .AddForce(playerCam.transform.forward * _throwingSystem.strength);
            if (audioSystem.enabled)
            {
                new AudioBuilder()
                    .WithClip(audioSystem.throwAudio)
                    .WithName("RigidBodyPickUp_ThrowAudio")
                    .WithVolume(SoundVolume.Normal)
                    .Play();
            }
        }

        private void ResetPickUp(bool disableGravity)
        {
            if (disableGravity && _isObjectHeld && _objectDefaultGravity)
            {
                _objectHeld.GetComponent<Rigidbody>().useGravity = true;
            }

            if (_objectHeld != null)
            {
                _objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
            }

            _isObjectHeld = false;
            _isPuzzleFocused = false;
            _timeStartedMovement = 0;
            _physicsMenu.canPlaceBack = false;
            _objectHeld = null;
            _timeHeld = _intTimeHeld;
        }
    }
}