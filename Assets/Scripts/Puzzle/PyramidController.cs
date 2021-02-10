using System;
using UnityEngine;
using ZeoFlow.Pickup.Interfaces;

namespace Puzzle
{
	public class PyramidController : MonoBehaviour, IOnPuzzle
	{
		public PyramidState startSide = PyramidState.Side1;
		public PyramidState winState = PyramidState.Side1;
		public float rotateBy = 1.0f;
		private int _currentProgress;

		private PyramidState _currentState;
		private bool _isMoving;
		private float _rotateByCurrent;
		private Vector3 _startAngle;

		private void Start()
		{
			_startAngle = transform.rotation.eulerAngles;
			switch (startSide)
			{
				case PyramidState.Side1:
					RotatePyramid(0);
					break;
				case PyramidState.Side2:
					RotatePyramid(90);
					break;
				case PyramidState.Side3:
					RotatePyramid(180);
					break;
				case PyramidState.Side4:
					RotatePyramid(270);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			_currentState = startSide;
		}

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

		public void ONMovement(bool toRight)
		{
			if (_isMoving) return;
			_isMoving = true;

			_rotateByCurrent = !toRight ? rotateBy : rotateBy * -1;
		}

		public bool ONIsMoving()
		{
			return _isMoving;
		}

		private void RotatePyramid(int angle)
		{
			transform.Rotate(0, 0, angle);
		}

		private void RotatePyramidBy(float angle)
		{
			transform.Rotate(0, 0, angle);
			_currentProgress++;
		}

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

		public bool IsWinState()
		{
			if (_isMoving) return false;
			return winState == _currentState;
		}
	}
}