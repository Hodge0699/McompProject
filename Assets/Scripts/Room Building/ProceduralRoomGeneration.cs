using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomBuilding
{
    public class ProceduralRoomGeneration : MonoBehaviour
    {
        public int rooms = 5;

        public Vector2 widthBoundaries = new Vector2(25.0f, 40.0f);
        public Vector2 heightBoundaries = new Vector2(20.0f, 40.0f);

        private RoomBuilder rb;

        private Direction nextEntrance;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<RoomBuilder>();

            createStartingRoom();

            for (int i = 1; i < rooms - 1; i++)
                createRoom(nextEntrance);

            createEndRoom(nextEntrance);
        }

        void createStartingRoom()
        {
            rb.init();
            rb.dimensions = new Vector3(Random.Range(widthBoundaries.x, widthBoundaries.y), 5.0f, Random.Range(heightBoundaries.x, heightBoundaries.y));

            Direction exit = getRandomDirection();

            switch (exit)
            {
                case Direction.NORTH:
                    rb.northWall = RoomBuilder.wallType.DOOR;
                    break;
                case Direction.EAST:
                    rb.eastWall = RoomBuilder.wallType.DOOR;
                    break;
                case Direction.SOUTH:
                    rb.southWall = RoomBuilder.wallType.DOOR;
                    break;
                case Direction.WEST:
                    rb.westWall = RoomBuilder.wallType.DOOR;
                    break;
            }

            nextEntrance = flipDirection(exit);

            rb.buildRoom();
        }

        void createRoom(Direction entrance)
        {
            Vector3 oldDimensions = rb.dimensions;
            rb.dimensions = new Vector3(Random.Range(widthBoundaries.x, widthBoundaries.y), 5.0f, Random.Range(heightBoundaries.x, heightBoundaries.y));

            Vector3 newOrigin = rb.transform.position;

            rb.init();


            switch (entrance)
            {
                case Direction.NORTH:
                    rb.northWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.z = newOrigin.z - (oldDimensions.z / 2) - (rb.dimensions.z / 2) + rb.wallThickness;
                    break;
                case Direction.EAST:
                    rb.eastWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.x = newOrigin.x - (oldDimensions.x / 2) - (rb.dimensions.x / 2) + rb.wallThickness;
                    break;
                case Direction.SOUTH:
                    rb.southWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.z = newOrigin.z + (oldDimensions.z / 2) + (rb.dimensions.z / 2) - rb.wallThickness;
                    break;
                case Direction.WEST:
                    rb.westWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.x = newOrigin.x + (oldDimensions.x / 2) + (rb.dimensions.x / 2) - rb.wallThickness;
                    break;
            }

            rb.transform.position = newOrigin;

            Direction exit = entrance;

            while (exit == entrance)
                exit = getRandomDirection();

            switch (exit)
            {
                case Direction.NORTH:
                    rb.northWall = RoomBuilder.wallType.DOOR;
                    break;
                case Direction.EAST:
                    rb.eastWall = RoomBuilder.wallType.DOOR;
                    break;
                case Direction.SOUTH:
                    rb.southWall = RoomBuilder.wallType.DOOR;
                    break;
                case Direction.WEST:
                    rb.westWall = RoomBuilder.wallType.DOOR;
                    break;
            }

            nextEntrance = flipDirection(exit);

            rb.buildRoom();
        }

        void createEndRoom(Direction entrance)
        {
            Vector3 oldDimensions = rb.dimensions;
            rb.dimensions = new Vector3(Random.Range(widthBoundaries.x, widthBoundaries.y), 5.0f, Random.Range(heightBoundaries.x, heightBoundaries.y));

            Vector3 newOrigin = rb.transform.position;

            rb.init();


            switch (entrance)
            {
                case Direction.NORTH:
                    rb.northWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.z = newOrigin.z - (oldDimensions.z / 2) - (rb.dimensions.z / 2) + rb.wallThickness;
                    break;
                case Direction.EAST:
                    rb.eastWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.x = newOrigin.x - (oldDimensions.x / 2) - (rb.dimensions.x / 2) + rb.wallThickness;
                    break;
                case Direction.SOUTH:
                    rb.southWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.z = newOrigin.z + (oldDimensions.z / 2) + (rb.dimensions.z / 2) - rb.wallThickness;
                    break;
                case Direction.WEST:
                    rb.westWall = RoomBuilder.wallType.DOORWAY;
                    newOrigin.x = newOrigin.x + (oldDimensions.x / 2) + (rb.dimensions.x / 2) - rb.wallThickness;
                    break;
            }

            rb.transform.position = newOrigin;

            rb.buildRoom();
        }

        Direction getRandomDirection()
        {
            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    return Direction.NORTH;
                case 1:
                    return Direction.EAST;
                case 2:
                    return Direction.SOUTH;
                case 3:
                    return Direction.WEST;
                default:
                    return Direction.ERROR;
            }
        }

        Direction flipDirection(Direction input)
        {
            switch (input)
            {
                case Direction.NORTH:
                    return Direction.SOUTH;
                case Direction.EAST:
                    return Direction.WEST;
                case Direction.SOUTH:
                    return Direction.NORTH;
                case Direction.WEST:
                    return Direction.EAST;
                default:
                    return Direction.ERROR;
            }

        }
    }
}