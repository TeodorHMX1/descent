using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Destructible
{
	/// <summary>
	/// This script allows you to assign particle system hit effects for each weapon type.
	/// PlayEffect() will attempt to play the hit effect for the specified weapon type.
	/// </summary>
	[DisallowMultipleComponent]
	public class HitEffects : MonoBehaviour
	{
		public List<HitEffect> effects;

		public void PlayEffect(HitBy weaponType, Vector3 hitPoint, Vector3 hitNormal)
		{
			var effect = (from eff in effects where (eff.hitBy & weaponType) > 0 select eff.effect).FirstOrDefault();
			if (effect != null)
				ObjectPool.Instance.Spawn(effect, hitPoint, Quaternion.LookRotation(hitNormal));
		}
	}
}