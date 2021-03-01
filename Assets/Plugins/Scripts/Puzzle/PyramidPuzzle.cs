﻿using System.Collections.Generic;
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
		[Header("Pyramids")]
		public List<PyramidController> pyramids = new List<PyramidController>();

		[Header("On Win")]
		public GameObject pickableObject;

		private bool _onWinCreated = false;
		private bool _isPickableObjectNotNull;

		private void Start()
		{
			_isPickableObjectNotNull = pickableObject != null;
		}

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			if (!_onWinCreated)
			{
				if (_isPickableObjectNotNull)
				{
					
				}
			}
			if (pyramids.Count(pyramid => pyramid.IsWinState()) < pyramids.Count) return;

			if (_onWinCreated || !IsCompleted()) return;
			_onWinCreated = true;
			if (!_isPickableObjectNotNull) return;
			if (pickableObject.GetComponent<IOnComplete>() != null)
				pickableObject.GetComponent<IOnComplete>().ONCompleted();
		}
		
		/// <summary>
		///     <para> IsCompleted </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <returns param="isCompleted"></returns>
		public bool IsCompleted()
		{
			return !(pyramids.Count(pyramid => pyramid.IsWinState()) < pyramids.Count);
		}
	}
}