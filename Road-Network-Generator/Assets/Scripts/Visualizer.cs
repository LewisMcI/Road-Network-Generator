using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    // Length of road.
    [SerializeField]
    private int length;

    // Angle of road.
    [SerializeField]
    private int angle;

    // Road to place.
    [SerializeField]
    private GameObject roadDiagonal, roadStraight, roadCorner, roadThreeway, roadFourway, roadEnd;

    // C# Version of a HashMap.
    private Dictionary<Vector3Int, GameObject> roadsPlaced = new Dictionary<Vector3Int, GameObject>();


    private void PlaceRoad(Vector3 start, Vector3 dir, int distance)
    {
        // Direction in converted to Vecor3Int
        Vector3Int newDirection = Vector3Int.RoundToInt(dir);
        // Base Rotation
        var rotation = Quaternion.identity;
        // Rotation set if facing separate direction
        if (newDirection.x == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        // For each road to be placed.
        for (int i = 0; i < length; i++)
        {
            // New position of road.
            var position = Vector3Int.RoundToInt(start + newDirection * i);

            // If road is pointing towards the Z axis.
            if (rotation.y != 0)
            {
                // If a road has not been placed to the right or the left of this current road previously.
                if (!roadsPlaced.ContainsKey(position + Vector3Int.left) && !roadsPlaced.ContainsKey(position + Vector3Int.right))
                {
                    // Place road at position.
                    PlaceRoadAtPosition(roadStraight, position, rotation);

                }
            }
            // If road is not pointing towards the Z axis (Pointing towards X axis).
            else
            {
                // If a road has not been placed infront or behind of this current road previously.
                if(!roadsPlaced.ContainsKey(position + Vector3Int.forward) && !roadsPlaced.ContainsKey(position + Vector3Int.back))
                {
                    // Place road at position.
                    PlaceRoadAtPosition(roadStraight, position, rotation);

                }
            }
        }
    }

    private void PlaceRoadAtPosition(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        // Creates a new road at position and adds it to the road dictionary.
        if (roadsPlaced.ContainsKey(position))
        {
            Destroy(roadsPlaced[position]);
        }
        roadsPlaced[position] = Instantiate(prefab, position, rotation, transform);
    }
    private void FixRoads()
    {
        HashSet<Vector3Int> positions = new HashSet<Vector3Int>();
        foreach (var key in roadsPlaced.Keys)
        {
            positions.Add(key);
        }
        foreach (var position in positions)
        {
            List<Direction> neighbourDirections = NeighbourHelper.FindNeighbourRoads(position, roadsPlaced.Keys);
            Quaternion rotation = Quaternion.identity;

            if (neighbourDirections.Count == 0)
            {
                Destroy(roadsPlaced[position]);
            }
            // If there is only one neighbour, we must convert it to a 'RoadEnd' depending on its neighbours direction from our road.
            else if (neighbourDirections.Count == 1)
            {
                if (neighbourDirections.Contains(Direction.down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }

                PlaceRoadAtPosition(roadEnd, position, rotation);
            }
            // If there are two neighbours, we must first check if it is parallel. If not then we will convert it to a 'RoadCorner' depending on its neighbours direction from our road.
            else if (neighbourDirections.Count == 2)
            {

                Quaternion newQuat = new Quaternion();
                if (neighbourDirections.Contains(Direction.up) && neighbourDirections.Contains(Direction.down))
                {
                    newQuat = Quaternion.Euler(0, 90, 0);
                    roadsPlaced[position].transform.rotation = newQuat;
                    continue;
                }
                else if (neighbourDirections.Contains(Direction.left) && neighbourDirections.Contains(Direction.right))
                {
                    newQuat = Quaternion.Euler(0, 0, 0);
                    roadsPlaced[position].transform.rotation = newQuat;
                    continue;
                }


                if (neighbourDirections.Contains(Direction.up) && neighbourDirections.Contains(Direction.right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.right) && neighbourDirections.Contains(Direction.down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.down) && neighbourDirections.Contains(Direction.left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }

                PlaceRoadAtPosition(roadCorner, position, rotation);
            }
            // If there are three neighbours, we must convert it to a 'RoadThreeway' depending on its neighbours direction from our road.
            else if (neighbourDirections.Count == 3)
            {
                if (neighbourDirections.Contains(Direction.right) && neighbourDirections.Contains(Direction.down) && neighbourDirections.Contains(Direction.left))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.down) && neighbourDirections.Contains(Direction.left) && neighbourDirections.Contains(Direction.up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.left) && neighbourDirections.Contains(Direction.up) && neighbourDirections.Contains(Direction.right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }

                PlaceRoadAtPosition(roadThreeway, position, rotation);
            }
            // If there are four neighbours, we will convert it to a 'RoadFourway'. We do not need to check the orientation for this.
            else
            {
                PlaceRoadAtPosition(roadFourway, position, rotation);
            }
        }
    }

    public void DrawRoads(string finishedString)
    {
        // If the Generator has a MeshRenderer and a MeshFilter attached, they are destroyed.
        try
        {
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());
        }
        catch { }
        Debug.Log("Road to Draw: '" + finishedString + "'");

        // Creates a stack holding the position, direction and length of each point.
        Stack<RoadParameters> savePoints = new Stack<RoadParameters>();
        // Instantiate variables.
        Vector3 currentPosition = transform.position;
        Vector3 direction = Vector3.forward;

        // For each action
        foreach (var action in finishedString)
        {
            // Converts action to predetermined alphabet
            Alphabet alphabet = (Alphabet)action;
            switch (alphabet)
            {
                // If case is to draw, a line is drawn from the current position to the end position.
                case Alphabet.Draw:
                    PlaceRoad(currentPosition, direction, length);
                    currentPosition += direction * length;
                    length -= 2;
                    break;
                // If case is to save, the current position, direction and length are saved into the savePoints stack.
                case Alphabet.Save:
                    savePoints.Push(new RoadParameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = length
                    });
                    break;
                // If case is to load, the previous positions are reloaded.
                case Alphabet.Load:
                    var roadParameters = savePoints.Pop();
                    currentPosition = roadParameters.position;
                    direction = roadParameters.direction;
                    length = roadParameters.length;
                    break;
                // If case is to turn left, direction is adjusted with angle set in inspector
                case Alphabet.TurnLeft:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                // If case is to turn right, direction is adjusted with negative of angle set in inspector
                case Alphabet.TurnRight:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
                default:
                    Debug.Log("Invalid Character");
                    break;

            }
        }
        FixRoads();
        roadsPlaced = new Dictionary<Vector3Int, GameObject>(); ;
    }
}
