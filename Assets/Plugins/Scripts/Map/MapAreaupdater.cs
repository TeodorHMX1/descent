using Override;
using UnityEngine;

namespace Map
{
	/// <summary>
	///     <para> MapAreaupdater </para>
	/// </summary>
	public class MapAreaupdater : MonoBehaviour
	{
		private const float StartTime = 10f;
		public GameObject area1;
		public GameObject area2;
		public GameObject area3;
		public GameObject updatetext;
		public AudioClip scribble;
		private float _currentTime;
		private bool _isupdatetextNull;

		/// <summary>
		///     <para> Start </para>
		/// </summary>
		private void Start() //sets timer
		{
			_isupdatetextNull = updatetext == null;
			_currentTime = 0;
		}

		/// <summary>
		///     <para> Update </para>
		/// </summary>
		private void Update() //starts and finishes timer
		{
			if (_isupdatetextNull) return;
			
			_currentTime -= 1 * Time.deltaTime;
			
			if (_currentTime <= 0)
			{
				_currentTime = 0;
				updatetext.SetActive(false);
			}

			if (_currentTime >= 1) updatetext.SetActive(true);
		}

		/// <summary>
		///     <para> OnTriggerEnter </para>
		/// </summary>
		/// <param name="collisionInfo"></param>
		private void OnTriggerEnter(Collider collisionInfo) //Creates popup baised on area entered
		{
			switch (collisionInfo.name)
			{
				//Debug.Log(collisionInfo.collider.name);
				case "Update area 1":
					MapScript2.ColArea1 = true;
					_currentTime = StartTime;
					Debug.Log("Area one update");
					new AudioBuilder()
						.WithClip(scribble)
						.WithName("MapArea_Updater")
						.WithVolume(SoundVolume.Normal)
						.Play();
					Destroy(area1);
					break;
				case "Update area 2":
					MapScript2.ColArea2 = true;
					_currentTime = StartTime;
					Debug.Log("Area two update");
					new AudioBuilder()
						.WithClip(scribble)
						.WithName("MapArea_Updater")
						.WithVolume(SoundVolume.Normal)
						.Play();
					Destroy(area2);
					break;
				case "Update area 3":
					MapScript2.ColArea3 = true;
					_currentTime = StartTime;
					Debug.Log("Area three update");
					new AudioBuilder()
						.WithClip(scribble)
						.WithName("MapArea_Updater")
						.WithVolume(SoundVolume.Normal)
						.Play();
					Destroy(area3);
					break;
			}
		}
	}
}