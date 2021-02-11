using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Destructible
{
	public class SetupDestructible
	{
		[MenuItem("Window/Destructible/Setup - Minimal")]
		public static void SetupMinimalMenuOption()
		{
			GameObject Destructible;
			DestructionManager destructionManager = Object.FindObjectOfType<DestructionManager>();
			if (destructionManager != null)
				Destructible = destructionManager.gameObject;
			else
				Destructible = new GameObject("Destructible");

			Destructible.AddComponent<DestructionManager>();
			Destructible.AddComponent<ParticleManager>();
			ObjectPool pool = Destructible.AddComponent<ObjectPool>();

			DestructionTest destructionTest = Object.FindObjectOfType<DestructionTest>();
			if (destructionTest == null)
			{
				GameObject DestructibleTest = new GameObject("Destructible-InputTest");
				DestructibleTest.AddComponent<DestructionTest>();
			}

			if (pool != null)
			{
				GameObject defaultLargeParticle = Resources.Load<GameObject>("Default_Particles/DefaultLargeParticle");
				GameObject defaultSmallParticle = Resources.Load<GameObject>("Default_Particles/DefaultSmallParticle");
				pool.prefabsToPool = new List<PoolEntry>();
				pool.prefabsToPool.Add(new PoolEntry() {Count = 10, Prefab = defaultLargeParticle});
				pool.prefabsToPool.Add(new PoolEntry() {Count = 10, Prefab = defaultSmallParticle});
			}
		}

		[MenuItem("Window/Destructible/Setup - Destructible Trees")]
		public static void SetupDestructibleTreesMenuOption()
		{
			EditorUtility.DisplayDialog("A Note About Destructible Trees",
				"NOTE: You will need to uncheck Enable Tree Colliders on your terrain in order to use destructible trees.\n\n" +
				"Once you've added your trees to the terrain, click the \"Update Trees\" button on the TreeManager, and Destructible will " +
				"create game objects with colliders and place them over the terrain tree instances so they can be destroyed.",
				"Ok");

			DestructionManager destructionManager = Object.FindObjectOfType<DestructionManager>();
			if (destructionManager == null)
				SetupMinimalMenuOption();

			destructionManager = Object.FindObjectOfType<DestructionManager>();
			destructionManager.gameObject.AddComponent<TreeManager>();
		}
	}
}