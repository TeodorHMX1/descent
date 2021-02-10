// ReSharper disable once CheckNamespace

namespace ZeoFlow.Pickup.Interfaces
{
	public interface IOnPuzzle
	{
		void ONMovement(bool toRight);
		bool ONIsMoving();
	}
}