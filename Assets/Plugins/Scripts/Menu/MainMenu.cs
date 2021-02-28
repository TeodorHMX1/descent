using System;
using UnityEngine;
using ZeoFlow;

namespace Menu
{
	/// <summary>
	///     <para> MainMenu </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class MainMenu : MonoBehaviour
	{
		public GameObject mainMenu;
		public GameObject optionsMenu;
		public GameObject creditsScreen;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			mainMenu.SetActive(true);
			optionsMenu.SetActive(false);
			creditsScreen.SetActive(false);
		}

		/// <summary>
		///     <para> Update </para>
		/// </summary>
		private void Update()
		{
			if (!creditsScreen.activeSelf || !InputManager.GetButtonDown("PauseMenu")) return;
			
			mainMenu.SetActive(true);
			optionsMenu.SetActive(false);
			creditsScreen.SetActive(false);
		}

		/// <summary>
		///     <para> IOnMainMenu </para>
		/// </summary>
		public void IOnMainMenu()
		{
			mainMenu.SetActive(true);
			optionsMenu.SetActive(false);
			creditsScreen.SetActive(false);
		}

		/// <summary>
		///     <para> IOnOptions </para>
		/// </summary>
		public void IOnOptions()
		{
			mainMenu.SetActive(false);
			optionsMenu.SetActive(true);
			creditsScreen.SetActive(false);
		}

		/// <summary>
		///     <para> IOnPlay </para>
		/// </summary>
		public void IOnPlay()
		{
			mainMenu.SetActive(false);
			optionsMenu.SetActive(false);
			creditsScreen.SetActive(false);
		}

		/// <summary>
		///     <para> IOnCreditsScreen </para>
		/// </summary>
		public void IOnCreditsScreen()
		{
			mainMenu.SetActive(false);
			optionsMenu.SetActive(false);
			creditsScreen.SetActive(true);
		}
	}
}