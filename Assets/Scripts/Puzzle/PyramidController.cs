using System;
using UnityEngine;
using ZeoFlow.Pickup.Interfaces;

namespace Puzzle
{
	/// <summary>
	///     <para> PyramidController </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class PyramidController : MonoBehaviour, IOnPuzzle
	{
		[Range(3, 8)] public int pyramidSides = 4;
		public PyramidState startSide = PyramidState.Side1;
		public PyramidState winState = PyramidState.Side1;
		[Range(0.5f, 10.0f)] public float rotateBy = 1.0f;

		private int _currentProgress;
		private PyramidState _currentState;
		private bool _isMoving;
		private float _rotateByCurrent;
		private Vector3 _startAngle;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		private void Start()
		{
			_startAngle = transform.rotation.eulerAngles;
			switch (startSide)
			{
				case PyramidState.Side1:
					RotatePyramid(360 / pyramidSides * 0);
					break;
				case PyramidState.Side2:
					RotatePyramid(360 / pyramidSides * 1);
					break;
				case PyramidState.Side3:
					RotatePyramid(360 / pyramidSides * 2);
					break;
				case PyramidState.Side4:
					RotatePyramid(360 / pyramidSides * 3);
					break;
				case PyramidState.Side5:
					RotatePyramid(360 / pyramidSides * 4);
					break;
				case PyramidState.Side6:
					RotatePyramid(360 / pyramidSides * 5);
					break;
				case PyramidState.Side7:
					RotatePyramid(360 / pyramidSides * 6);
					break;
				case PyramidState.Side8:
					RotatePyramid(360 / pyramidSides * 7);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			_currentState = startSide;
		}

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			if (!_isMoving) return;

			if (_currentProgress < 90)
			{
				RotatePyramidBy(_rotateByCurrent);
			}
			else
			{
				_currentProgress = 0;
				_isMoving = false;
				CheckState();
			}
		}

		/// <summary>
		///     <para> ONMovement </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="toRight"></param>
		public void ONMovement(bool toRight)
		{
			if (_isMoving) return;
			_isMoving = true;

			_rotateByCurrent = !toRight ? rotateBy : rotateBy * -1;
		}

		/// <summary>
		///     <para> ONIsMoving </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <returns param="_isMoving"></returns>
		public bool ONIsMoving()
		{
			return _isMoving;
		}

		/// <summary>
		///     <para> RotatePyramid </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="angle"></param>
		private void RotatePyramid(int angle)
		{
			transform.Rotate(0, 0, angle);
		}

		/// <summary>
		///     <para> RotatePyramidBy </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="angle"></param>
		private void RotatePyramidBy(float angle)
		{
			transform.Rotate(0, 0, angle);
			_currentProgress++;
		}

		/// <summary>
		///     <para> CheckState </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void CheckState()
		{
			var currentRotation = transform.rotation.eulerAngles.y;
			currentRotation = Math.Abs((int) currentRotation) - (int) _startAngle.y - 1;
			switch (currentRotation)
			{
				case 0:
					_currentState = PyramidState.Side1;
					break;
				case 90:
					_currentState = PyramidState.Side2;
					break;
				case 180:
					_currentState = PyramidState.Side3;
					break;
				case 270:
					_currentState = PyramidState.Side4;
					break;
			}
		}

		/// <summary>
		///     <para> IsWinState </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <returns name="winState"></returns>
		public bool IsWinState()
		{
			if (_isMoving) return false;
			return winState == _currentState;
		}
	}
}