using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
	/// <summary>
	///     <para> PyramidPuzzle </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class PyramidPuzzle : MonoBehaviour
	{
		public List<PyramidController> pyramids = new List<PyramidController>();

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			if (pyramids.Count(pyramid => pyramid.IsWinState()) < pyramids.Count) return;

			Debug.Log("puzzleCompleted");
		}
	}
}