using UnityEngine;

namespace Puzzle
{
    public class SlidingDoor : MonoBehaviour
    {

        [Header("Options")]
        public float closedY;
        public float openedY;

        private Vector3 openedPos;
        private Vector3 closedPos;

        private void Start()
        {
            var position = transform.position;
            openedPos = new Vector3(
                position.x,
                openedY,
                position.z
            );
            closedPos = new Vector3(
                position.x,
                closedY,
                position.z
            );
        }
        
        public void Opened()
        {
            transform.position = openedPos;
        }

        public void Closed()
        {
            transform.position = closedPos;
        }
        
    }
}