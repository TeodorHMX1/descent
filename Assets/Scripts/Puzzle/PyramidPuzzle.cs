using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
	public class PyramidPuzzle : MonoBehaviour
	{
		public List<PyramidController> pyramids = new List<PyramidController>();

		private void Update()
		{
			var winStates = pyramids.Count(pyramid => pyramid.IsWinState());
			if (winStates < pyramids.Count) return;

			Debug.Log("puzzleCompleted");
		}
	}
}