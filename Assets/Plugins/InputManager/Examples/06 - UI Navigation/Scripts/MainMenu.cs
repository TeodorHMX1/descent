using UnityEngine;
using UnityEngine.EventSystems;

namespace ZeoFlow.Examples
{
	public class MainMenu : MonoBehaviour 
	{
		[SerializeField]
		private MenuPage m_startPage = null;
		[SerializeField]
		private MenuPage[] m_pages = null;

		private MenuPage m_currentPage;

		private void Start()
		{
			ChangePage(m_startPage.ID);
		}

		public void ChangePage(string id)
		{
			if(m_currentPage != null)
				m_currentPage.gameObject.SetActive(false);

			m_currentPage = FindPage(id);
			if(m_currentPage != null)
			{
				m_currentPage.gameObject.SetActive(true);
				EventSystem.current.SetSelectedGameObject(m_currentPage.FirstSelected);
			}
		}

		public void Quit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		private MenuPage FindPage(string id)
		{
			foreach(MenuPage page in m_pages)
			{
				if(page.ID == id)
					return page;
			}
			return null;
		}
	}
}
