using UnityEngine;

namespace Puzzle
{
    public class SlidingDoor : MonoBehaviour
    {

        [Header("Options")]
        public float closedY;
        public float openedY;

        public void Opened()
        {
            var o = gameObject;
            var position = o.transform.position;
            position = new Vector3(
                position.x,
                openedY,
                position.z
            );
            o.transform.position = position;
        }

        public void Closed()
        {
            var o = gameObject;
            var position = o.transform.position;
            position = new Vector3(
                position.x,
                closedY,
                position.z
            );
            o.transform.position = position;
        }
        
    }
}