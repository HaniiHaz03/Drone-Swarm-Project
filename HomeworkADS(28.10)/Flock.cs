using UnityEngine;
using System.Collections.Generic;
using System.IO;  // Import to handle file operations
using System;

public class Flock : MonoBehaviour
{
    public List<CommunicatingDrone> drones = new List<CommunicatingDrone>(); // Initialize the list of drones
    public float avoidanceRadius = 1f; // Radius to avoid crowding
    public float maxSpeed = 2f; // Maximum speed for the drones

    private string csvFilePath = "Assets/Logs/partition_timing.csv"; // CSV file path

    void Start()
    {
        InitializeCsvFile();  // Create CSV header
        // Automatically add all drones in the scene to the flock
        foreach (CommunicatingDrone drone in FindObjectsOfType<CommunicatingDrone>())
        {
            AddDrone(drone); // Add each drone to the Flock
        }
    }

    void InitializeCsvFile()
    {
        if (!File.Exists(csvFilePath))
        {
            // Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(csvFilePath));

            // Write header to CSV
            File.WriteAllText(csvFilePath, "Frame,PartitionTime (ms)\n");
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

    public void RemoveDrone(CommunicatingDrone drone)
    {
        if (drones.Contains(drone))
        {
            drones.Remove(drone); // Remove the drone from the list
            Debug.Log($"Drone {drone.name} removed from the flock.");
        }
    }

    public CommunicatingDrone FindDroneByName(string name)
    {
        return drones.Find(drone => string.Equals(drone.name, name, StringComparison.OrdinalIgnoreCase));
    }

    void Update()
    {
        // Measure the partitioning time
        float startTime = Time.realtimeSinceStartup;

        PartitionDrones();  // Call the partition function

        float partitionTime = (Time.realtimeSinceStartup - startTime) * 1000f; // Convert to milliseconds
        LogPartitionTime(partitionTime);  // Log the partitioning time
    }

    void PartitionDrones()
    {
        foreach (var drone in drones)
        {
            // Check the drone's tag for color identification
            if (drone.CompareTag("Red"))
            {
                drone.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (drone.CompareTag("Blue"))
            {
                drone.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            else
            {
                Debug.LogWarning($"{drone.name} has an unrecognized tag and was not colored.");
            }
        }
    }

    void LogPartitionTime(float partitionTime)
    {
        int frameNumber = Time.frameCount;
        string logEntry = $"{frameNumber},{partitionTime:F3}\n";
        File.AppendAllText(csvFilePath, logEntry);
        Debug.Log($"Frame {frameNumber}: Partitioning time {partitionTime:F3} ms");
    }
}


