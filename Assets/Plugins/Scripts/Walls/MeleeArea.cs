using System.Collections.Generic;
using Destructible;
using Puzzle;
using UnityEngine;

namespace Walls
{
        
    /// <summary>
    ///     <para> MeleeArea </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class MeleeArea : MonoBehaviour
    {
        public int damageAmount = 30;
        public float meleeRadius = 1.3f;
        public float additionalForceAmount = 150f;
        public float additionalForceRadius = 2f;
        public bool requirePuzzle = false;
        public PyramidPuzzle pyramidPuzzle;
        
        /// <summary>
        ///     <para> OnMeleeDamage </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public void OnMeleeDamage()
        {

            if (requirePuzzle)
            {
                if (pyramidPuzzle == null)
                {
                    return;
                }

                if (!pyramidPuzzle.IsCompleted())
                {
                    return;
                }
            }
            
            Collider[] objectsInRange = Physics.OverlapSphere(transform.position, meleeRadius);
            List<Destructible.Destructible> damagedObjects = new List<Destructible.Destructible>(); // Keep track of what objects have been damaged so we don't do damage multiple times per collider.
            bool hasPlayedHitEffect = false;

            foreach (Collider col in objectsInRange)
            {
                // Ignore terrain colliders
                if (col is TerrainCollider) continue;

                // Ignore trigger colliders
                if (col.isTrigger) continue;

                // Ignore the player's character controller (ie, don't allow hitting yourself)
                if (col is CharacterController && col.CompareTag("Player")) continue;

                if (!hasPlayedHitEffect) // Only play the hit effect once per melee attack.
                {
                    // Play hit effects
                    HitEffects hitEffects = col.gameObject.GetComponentInParent<HitEffects>();
                    if (hitEffects != null && hitEffects.effects.Count > 0)
                        hitEffects.PlayEffect(HitBy.Axe, transform.position, transform.forward * -1);

                    hasPlayedHitEffect = true;
                }

                // Apply impact force to rigidbody hit
                Rigidbody rbody = col.attachedRigidbody;
                if (rbody != null)
                    rbody.AddForceAtPosition(transform.forward * 3f, transform.position, ForceMode.Impulse);

                // Apply damage if object hit was Destructible
                // Only do this for active and enabled Destructible scripts found in parent objects
                // Special Note: Destructible scripts are turned off on terrain trees by default (to save resources), so we will make an exception for them and process the hit anyway
                Destructible.Destructible[] destObjs = col.gameObject.GetComponentsInParent<Destructible.Destructible>(false);
                foreach (Destructible.Destructible destObj in destObjs)
                {
                    if (damagedObjects.Contains(destObj)) continue;
                    if (!destObj.isActiveAndEnabled && !destObj.isTerrainTree) continue;

                    damagedObjects.Add(destObj);
                    ImpactDamage meleeImpact = new ImpactDamage() { DamageAmount = damageAmount, AdditionalForce = additionalForceAmount,
                        AdditionalForcePosition = transform.position, AdditionalForceRadius = additionalForceRadius };
                    destObj.ApplyDamage(meleeImpact);
                }
            }
        }
        
        /// <summary>
        ///     <para> OnDrawGizmos </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, meleeRadius);
        }
    }
}