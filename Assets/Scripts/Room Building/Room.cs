using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomBuilding
{
    public class Room : MonoBehaviour
    {
        public Vector3 dimensions;
        private Vector3 origin;

        public float wallThickness;

        private Direction entrance;
        private Direction exit;

        private List<DoorController> doors = new List<DoorController>();

        public Room(Vector3 origin, Vector3 dimensions, float wallThickness, List<DoorController> doors)
        {
            this.origin = origin;
            this.dimensions = dimensions;
            this.wallThickness = wallThickness;

            this.doors = doors;
        }
    }
}