using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;

public class DroneUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField searchInputField; // Input field for drone name or key
    public Button searchButton;
    public Button searchByKeyButton;
    public Button selfDestructButton;
    public TMP_InputField pathStartInputField; // Input field for start drone key
    public TMP_InputField pathEndInputField; // Input field for end drone key
    public Button findShortestPathButton; // Button to find shortest path
    public TMP_Text resultText; // Result text for search
    public TMP_Text pathResultText; // Result text for pathfinding
    public TMP_Text simulationTimeText; // Simulation time text
    public TMP_Text frameRateText; // Frame rate text
    public TMP_Text droneCountText; // Drone count text

    [Header("Drone Flock")]
    public Flock droneFlock; // Reference to the Flock script

    private Dictionary<string, List<CommunicatingDrone>> partitions;

    private float elapsedTime = 0f;
    private string csvFilePath = "Assets/Logs/destruction_timing.csv"; // CSV file path

    void Start()
    {
        if (droneFlock == null)
        {
            // Attempt to find the Flock component in the scene
            droneFlock = FindObjectOfType<Flock>();
        }

        if (droneFlock == null)
        {
            Debug.LogError("Flock component not found in the scene. Please add a Flock component.");
            return;
        }

        InitializeCsvFile();

        // Access partitions from the Flock script and ensure it's not null
        if (droneFlock.partitions != null)
        {
            partitions = droneFlock.partitions;
        }
        else
        {
            Debug.LogError("Partitions not found in the Flock component.");
            return;
        }

        // Debug: Log the contents of partitions
        LogPartitionContents();

        // Attach button click events
        searchButton.onClick.AddListener(OnSearchDrone);
        searchByKeyButton.onClick.AddListener(OnSearchDroneByKey);
        selfDestructButton.onClick.AddListener(OnSelfDestructDrone);
        findShortestPathButton.onClick.AddListener(OnFindShortestPath);
    }

    void InitializeCsvFile()
    {
        // Create CSV file if it doesn't exist
        if (!File.Exists(csvFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(csvFilePath));
            File.WriteAllText(csvFilePath, "DroneID,TimeToDestroy (ms)\n");
        }
    }

    void Update()
    {
        UpdateFrameRate();
        UpdateDroneCount();
        UpdateSimulationTime();
    }

    // Update frame rate text
    void UpdateFrameRate()
    {
        float frameRate = 1.0f / Time.deltaTime;
        frameRateText.text = $"Framerate: {frameRate:F2} FPS";
    }

    // Update the drone count displayed on the UI
    void UpdateDroneCount()
    {
        if (droneFlock != null)
        {
            droneCountText.text = $"Drone Count: {droneFlock.drones.Count}";
        }
        else
        {
            droneCountText.text = "Drone Count: N/A";
        }
    }

    // Update simulation time text
    void UpdateSimulationTime()
    {
        if (simulationTimeText != null)
        {
            float simulationTime = Time.realtimeSinceStartup;
            simulationTimeText.text = $"Simulation Time: {simulationTime:F2} seconds";
        }
    }

    // Handle searching a drone by name
    void OnSearchDrone()
    {
        string droneId = searchInputField.text.Trim();

        if (string.IsNullOrEmpty(droneId))
        {
            resultText.text = "Please enter a drone name!";
            return;
        }

        CommunicatingDrone drone = droneFlock.FindDroneByName(droneId);

        if (drone != null && drone.gameObject.activeSelf)
        {
            resultText.text = $"Drone {droneId} Position: {drone.transform.position}\n" +
                          $"Target Position: {drone.targetPosition}\n" +
                          $"Speed: {drone.speed} units/s";

            float distance = CalculateDistanceToDrone(drone);
            elapsedTime = Time.realtimeSinceStartup;

            if (drone.tag == "Red")
            {
                resultText.text += "\nDrone is part of Red partition!";
            }
        }
        else
        {
            resultText.text = "Drone not found or inactive.";
        }
    }

    // Handle searching a drone by key
    void OnSearchDroneByKey()
    {
        string droneKey = searchInputField.text.Trim();

        if (string.IsNullOrEmpty(droneKey))
        {
            resultText.text = "Please enter a drone key!";
            return;
        }

        CommunicatingDrone drone = droneFlock.FindDroneByKey(droneKey);

        if (drone != null && drone.gameObject.activeSelf)
        {
            resultText.text = $"Drone {droneKey} Position: {drone.transform.position}\n" +
                          $"Target Position: {drone.targetPosition}\n" +
                          $"Speed: {drone.speed} units/s";

            float distance = CalculateDistanceToDrone(drone);
            elapsedTime = Time.realtimeSinceStartup;

            if (drone.tag == "Blue")
            {
                resultText.text += "\nDrone is part of Blue partition!";
            }
        }
        else
        {
            resultText.text = "Drone not found or inactive.";
        }
    }

    // Handle self-destruct of a drone
    void OnSelfDestructDrone()
    {
        string droneKey = searchInputField.text.Trim();

        if (string.IsNullOrEmpty(droneKey))
        {
            resultText.text = "Please enter a drone key!";
            return;
        }

        CommunicatingDrone drone = droneFlock.FindDroneByKey(droneKey);
        if (drone != null && drone.gameObject != null)
        {
            float selfDestructionTime = (Time.realtimeSinceStartup - elapsedTime) * 1000f;
            LogSelfDestruction(drone, selfDestructionTime);
            resultText.text = $"Drone {droneKey} destroyed.";

            // First, remove the drone from the flock
            droneFlock.RemoveDrone(drone);

            // Then destroy the GameObject
            Destroy(drone.gameObject);
        }
        else
        {
            resultText.text = "Drone not found or already destroyed.";
        }
    }

    // Record self-destruction time to CSV
    void LogSelfDestruction(CommunicatingDrone drone, float timeToDestroy)
    {
        string logEntry = $"{drone.droneKey},{timeToDestroy:F3}\n";
        File.AppendAllText(csvFilePath, logEntry);
        Debug.Log($"Drone {drone.droneKey} destroyed in {timeToDestroy:F3} ms.");
    }

    // Pathfinding: Find shortest path between two drones
    void OnFindShortestPath()
    {
        string startDroneKey = pathStartInputField.text.Trim();
        string endDroneKey = pathEndInputField.text.Trim();

        CommunicatingDrone startDrone = droneFlock.FindDroneByKey(startDroneKey);
        CommunicatingDrone endDrone = droneFlock.FindDroneByKey(endDroneKey);

        if (startDrone == null || endDrone == null)
        {
            pathResultText.text = "Invalid drone keys!";
            return;
        }

        // Call the new CalculateShortestPath method
        float shortestPath = CalculateShortestPath(startDrone, endDrone);
        pathResultText.text = $"Shortest Path: {shortestPath:F2} units.";
    }

    // Calculate the distance to a specific drone
    float CalculateDistanceToDrone(CommunicatingDrone drone)
    {
        return Vector3.Distance(transform.position, drone.transform.position);
    }

    // Basic placeholder for shortest path calculation
    // Here you can implement a more sophisticated pathfinding algorithm (e.g., A*)
    float CalculateShortestPath(CommunicatingDrone start, CommunicatingDrone end)
    {
        // For now, just return the straight-line distance between the two drones
        return Vector3.Distance(start.transform.position, end.transform.position);
    }

    // Log the contents of the partitions to the console
    void LogPartitionContents()
    {
        foreach (var partition in partitions)
        {
            Debug.Log($"Partition {partition.Key} has {partition.Value.Count} drones.");
        }
    }
}
