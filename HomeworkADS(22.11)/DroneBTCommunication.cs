using UnityEngine;
using System;

public class DroneBTCommunication : MonoBehaviour
{
    public CommunicatingDrone Drone { get; private set; } // Reference to the drone
    public DroneBTCommunication Left { get; set; } // Left child in the BST
    public DroneBTCommunication Right { get; set; } // Right child in the BST
    public float Key; // Key used for BST comparisons (e.g., speed or some custom attribute)

    public DroneBTCommunication(CommunicatingDrone drone, float key)
    {
        Drone = drone;
        Key = key;
    }

    // Insert a new drone into the Binary Tree
    public void Insert(CommunicatingDrone newDrone, float newKey)
    {
        if (newKey < Key)
        {
            if (Left == null)
                Left = new DroneBTCommunication(newDrone, newKey);
            else
                Left.Insert(newDrone, newKey);
        }
        else
        {
            if (Right == null)
                Right = new DroneBTCommunication(newDrone, newKey);
            else
                Right.Insert(newDrone, newKey);
        }
    }

    // Search for a drone by key
    public CommunicatingDrone SearchByKey(float searchKey)
    {
        if (searchKey == Key)
            return Drone;
        else if (searchKey < Key && Left != null)
            return Left.SearchByKey(searchKey);
        else if (searchKey > Key && Right != null)
            return Right.SearchByKey(searchKey);
        else
            return null; // Drone not found
    }

    // Example exhaustive search by some attribute (e.g., search by name)
    public CommunicatingDrone SearchByName(string name)
    {
        if (Drone.name.Equals(name, StringComparison.OrdinalIgnoreCase))
            return Drone;

        CommunicatingDrone found = null;

        if (Left != null)
            found = Left.SearchByName(name);

        if (found == null && Right != null)
            found = Right.SearchByName(name);

        return found;
    }
}
