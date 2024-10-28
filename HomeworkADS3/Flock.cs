using UnityEngine;
using System.Collections.Generic;
using System;

public class Flock : MonoBehaviour
{
    public List<CommunicatingDrone> drones = new List<CommunicatingDrone>(); // Initialize the list of drones

    // Define avoidance radius and max speed
    public float avoidanceRadius = 1f; // Radius to avoid crowding
    public float maxSpeed = 2f; // Maximum speed for the drones

    // Property to return the square of the avoidance radius
    public float SquareAvoidanceRadius => avoidanceRadius * avoidanceRadius;

    void Start()
    {
        // Automatically add all drones in the scene to the flock
        foreach (CommunicatingDrone drone in FindObjectsOfType<CommunicatingDrone>())
        {
            AddDrone(drone); // Add each drone to the Flock
        }
    }

    public void AddDrone(CommunicatingDrone drone)
    {
        if (!drones.Contains(drone))
        {
            drones.Add(drone);
            drone.flock = this; // Link the drone to this flock
            Debug.Log($"Drone {drone.name} added to the flock.");
        }
    }

    // Method to remove a drone from the flock
    public void RemoveDrone(CommunicatingDrone drone)
    {
        if (drones.Contains(drone))
        {
            drones.Remove(drone); // Remove the drone from the list
            Debug.Log($"Drone {drone.name} removed from the flock.");
        }
    }

    // Method to find a drone by name (used for searching)
    public CommunicatingDrone FindDroneByName(string name)
    {
        return drones.Find(drone => string.Equals(drone.name, name, StringComparison.OrdinalIgnoreCase));
    }

    // Method to get the list of drones
    public List<CommunicatingDrone> GetDrones()
    {
        return drones;
    }

    void Update()
    {
        // Measure the time taken to partition drones
        float startTime = Time.realtimeSinceStartup;

        // Call the partition function
        PartitionDrones();

        // Measure the elapsed time
        float partitionTime = Time.realtimeSinceStartup - startTime;

        // Calculate FPS
        float elapsedTime = 0f;
        int frameCount = 0;
        elapsedTime += Time.deltaTime;
        frameCount++;

        if (elapsedTime >= 1f) // Update FPS every second
        {
            Debug.Log($"FPS: {frameCount}");
            Debug.Log($"Partitioning Time: {partitionTime * 1000} ms"); // Convert to milliseconds
            elapsedTime = 0f;
            frameCount = 0;
        }
    }

    // Method to partition drones based on their tags
    void PartitionDrones()
    {
        foreach (var drone in drones)
        {
            // Check the drone's tag for color identification
            if (drone.CompareTag("Red"))
            {
                Debug.Log($"{drone.name} colored Red");
                drone.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (drone.CompareTag("Blue"))
            {
                Debug.Log($"{drone.name} colored Blue");
                drone.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            else
            {
                Debug.LogWarning($"{drone.name} has an unrecognized tag and was not colored.");
            }
        }
    }
}

