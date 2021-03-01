using Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZeoFlow;
using ZeoFlow.PlayerMovement;

namespace Menu
{
    /// <summary>
    ///     <para> Pause </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class Pause : MonoBehaviour
    {
        public static Pause instance;
        public bool isPaused;
        public GameObject pauseMenu;
        public GameObject optionsMenu;
        public MouseCursorLock mouseCursorLock;

        private bool _mainMenu;

        /// <summary>
        ///     <para> Start </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        /// <summary>
        ///     <para> Start </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Start()
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            if (_mainMenu) return;

            var pausePressed = InputManager.GetButtonDown("PauseMenu");
            switch (pausePressed)
            {
                case true when !isPaused:
                    isPaused = true;
                    break;
                case true when pauseMenu.activeSelf && isPaused:
                    isPaused = false;
                    break;
            }

            mouseCursorLock.SetPaused(isPaused);

            if (isPaused)
                Paused();
            else
                Resume();
        }

        /// <summary>
        ///     <para> OnResume </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void OnResume()
        {
            isPaused = false;
            pauseMenu.SetActive(false);
        }

        /// <summary>
        ///     <para> IOnPause </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void IOnPause()
        {
            isPaused = true;
        }

        /// <summary>
        ///     <para> OnOptions </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void OnOptions()
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }

        /// <summary>
        ///     <para> OnOptionsClosed </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void OnOptionsClosed()
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }

        /// <summary>
        ///     <para> Resume </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Resume()
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
            }

            Time.timeScale = 1f;
        }

        /// <summary>
        ///     <para> Paused </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Paused()
        {
            if (!optionsMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
            }

            Time.timeScale = 0f;
        }

        /// <summary>
        ///     <para> IOnMainMenu </para>
        /// </summary>
        public void IOnMainMenu()
        {
            _mainMenu = true;
            ItemsManager.Clean();
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
        }
    }
}