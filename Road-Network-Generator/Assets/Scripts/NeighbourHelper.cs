using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourHelper : MonoBehaviour
{
    // Finds neighbours roads.
    public static List<Direction> FindNeighbourRoads(Vector3Int position, ICollection<Vector3Int> collection)
    {
        // Creates new list of directions.
        List<Direction> neighbourDirections = new List<Direction>();
        // If the collection contains the position to the right of the current road.
        if (collection.Contains(position + Vector3Int.right))
        {
            // Adds this road to the neighbours.
            neighbourDirections.Add(Direction.right);
        }
        // If the collection contains the position to the left of the current road.
        if (collection.Contains(position - Vector3Int.right))
        {
            // Adds this road to the neighbours.
            neighbourDirections.Add(Direction.left);
        }
        // If the collection contains the position above the current road.
        if (collection.Contains(position + new Vector3Int(0, 0, 1)))
        {
            // Adds this road to the neighbours.
            neighbourDirections.Add(Direction.up);
        }
        // If the collection contains the position below  the current road.
        if (collection.Contains(position - new Vector3Int(0, 0, 1)))
        {
            // Add this road to the neighbours.
            neighbourDirections.Add(Direction.down);
        }
        // Returns the neighbours of the objects.
        return neighbourDirections;
    }
}
public enum Direction
{
    left,
    right,
    up,
    down,
    invalid
}
