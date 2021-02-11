using System;
using UnityEngine;

namespace Destructible
{
    /// <summary>
    /// This is a helper script that listens to a Destructible object's DestroyedEvent and runs additional code when the object is destroyed.
    /// Put this script on a GameObject that has the Destructible script.
    /// </summary>
    [RequireComponent(typeof(Destructible))]
    public class WhenDestroyedResetTree : MonoBehaviour
    {
        private Destructible _destObj;

        private void Start()
        {
            // Try to get the Destructible script on the object. If found, attach the OnDestroyed event listener to the DestroyedEvent.
            _destObj = gameObject.GetComponent<Destructible>();
            if (_destObj != null)
                _destObj.DestroyedEvent += OnDestroyed;
        }

        private void OnDisable()
        {
            // Unregister the event listener when disabled/destroyed. Very important to prevent memory leaks due to orphaned event listeners!
            if (_destObj == null) return;
            _destObj.DestroyedEvent -= OnDestroyed;
        }

        /// <summary>When the Destructible object is destroyed, the code in this method will run.</summary>
        private void OnDestroyed()
        {
            Debug.Log($"{_destObj.name} was destroyed at world coordinates: {_destObj.transform.position}");
            TerrainTree terrainTree = Terrain.activeTerrain.ClosestTreeToPoint(transform.position);
            TreeReset tree = new TreeReset()
            {
                prototypeIndex = terrainTree.TreeInstance.prototypeIndex,
                position = terrainTree.TreeInstance.position,
                color = terrainTree.TreeInstance.color,
                heightScale = terrainTree.TreeInstance.heightScale,
                widthScale = terrainTree.TreeInstance.widthScale,
                resetTime = DateTime.Now.AddSeconds(3)
            };
            TreeManager.Instance.treesToReset.Add(tree);
        }
    }
}