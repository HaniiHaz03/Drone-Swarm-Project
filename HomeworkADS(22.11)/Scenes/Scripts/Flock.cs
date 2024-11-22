using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class Flock : MonoBehaviour
{
    public List<CommunicatingDrone> drones = new List<CommunicatingDrone>();
    public float avoidanceRadius = 1f;
    public float maxSpeed = 2f;

    private string csvFilePath = "Assets/Logs/partition_timing.csv";

    // Dictionary to manage partitions based on drone tags
    public Dictionary<string, List<CommunicatingDrone>> partitions = new Dictionary<string, List<CommunicatingDrone>>();


    void Start()
    {
        InitializeCsvFile();

        // Initialize partitions dictionary
        partitions["Red"] = new List<CommunicatingDrone>();
        partitions["Blue"] = new List<CommunicatingDrone>();
        partitions["Neutral"] = new List<CommunicatingDrone>();

        // Add all drones found in the scene
        foreach (CommunicatingDrone drone in FindObjectsOfType<CommunicatingDrone>())
        {
            AddDrone(drone);
        }

        foreach (var drone in drones)
        {
            Debug.Log($"Drone in scene: {drone.name} with key: {drone.droneKey} and tag: {drone.tag}");
        }
    }

    void InitializeCsvFile()
    {
        if (!File.Exists(csvFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(csvFilePath));
            File.WriteAllText(csvFilePath, "Frame,PartitionTime (ms)\n");
        }
    }

    public void AddDrone(CommunicatingDrone drone)
    {
        if (!drones.Contains(drone))
        {
            drones.Add(drone);
            drone.flock = this;
            AssignDroneToPartition(drone);
            Debug.Log($"Drone {drone.name} (Key: {drone.droneKey}) added to the flock.");
        }
    }


    public void RemoveDrone(CommunicatingDrone drone)
    {
        if (drones.Contains(drone))
        {
            drones.Remove(drone);
            RemoveDroneFromPartition(drone);
            Debug.Log($"Drone {drone.name} removed from the flock.");
        }
    }

    public CommunicatingDrone FindDroneByName(string name)
    {
        return drones.Find(drone => string.Equals(drone.name, name, StringComparison.OrdinalIgnoreCase));
    }

    public CommunicatingDrone FindDroneByKey(string key)
    {
        CommunicatingDrone foundDrone = drones.Find(drone => string.Equals(drone.droneKey, key, StringComparison.OrdinalIgnoreCase));
        if (foundDrone == null)
        {
            Debug.Log($"Drone with key '{key}' not found in the network.");
        }
        return foundDrone;
    }

    void Update()
    {
        CleanupDestroyedDrones(); // Removes null references
        float startTime = Time.realtimeSinceStartup;
        PartitionDrones();
        float partitionTime = (Time.realtimeSinceStartup - startTime) * 1000f;
        LogPartitionTime(partitionTime);
    }
    void CleanupDestroyedDrones()
    {
        drones.RemoveAll(drone => drone == null);
    }

    void PartitionDrones()
    {
        // Clear existing partitions
        foreach (var partition in partitions.Values)
        {
            partition.Clear();
        }

        foreach (var drone in drones)
        {
            if (drone != null)
            {
                AssignDroneToPartition(drone);
            }
        }

        // Debug log to show partition contents
        foreach (var partition in partitions)
        {
            Debug.Log($"Partition {partition.Key}: {partition.Value.Count} drones");
            foreach (var drone in partition.Value)
            {
                Debug.Log($"  - {drone.name} ({drone.droneKey})");
            }
        }

    }

    void AssignDroneToPartition(CommunicatingDrone drone)
    {
        // Assign drones based on their tag
        if (drone.CompareTag("Red"))
        {
            partitions["Red"].Add(drone);
            drone.GetComponent<SpriteRenderer>().color = Color.red;
            Debug.Log($"Drone {drone.name} added to Red partition.");
        }
        else if (drone.CompareTag("Blue"))
        {
            partitions["Blue"].Add(drone);
            drone.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            partitions["Neutral"].Add(drone);
            drone.GetComponent<SpriteRenderer>().color = Color.gray;
        }

        // Update communication network after assigning to a partition
        UpdateDroneCommunication(drone);
    }

    void RemoveDroneFromPartition(CommunicatingDrone drone)
    {
        if (drone.CompareTag("Red"))
        {
            partitions["Red"].Remove(drone);
        }
        else if (drone.CompareTag("Blue"))
        {
            partitions["Blue"].Remove(drone);
        }
        else
        {
            partitions["Neutral"].Remove(drone);
        }
    }

    void UpdateDroneCommunication(CommunicatingDrone drone)
    {
        if (drone == null) return;
        string partitionTag = drone.tag;

        // Update communication only within the same partition
        if (partitions.ContainsKey(partitionTag))
        {
            List<CommunicatingDrone> samePartitionDrones = partitions[partitionTag];

            // Update neighbors for communication
            foreach (var otherDrone in samePartitionDrones)
            {
                if (otherDrone != drone)
                {
                    drone.AddNeighbor(otherDrone);
                }
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
