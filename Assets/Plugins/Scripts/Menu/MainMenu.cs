using UnityEngine;

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

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			mainMenu.SetActive(true);
			optionsMenu.SetActive(false);
		}

		/// <summary>
		///     <para> IOnMainMenu </para>
		/// </summary>
		public void IOnMainMenu()
		{
			mainMenu.SetActive(true);
			optionsMenu.SetActive(false);
		}

		/// <summary>
		///     <para> IOnOptions </para>
		/// </summary>
		public void IOnOptions()
		{
			mainMenu.SetActive(false);
			optionsMenu.SetActive(true);
		}

		/// <summary>
		///     <para> IOnPlay </para>
		/// </summary>
		public void IOnPlay()
		{
			mainMenu.SetActive(false);
			optionsMenu.SetActive(false);
		}
	}
}