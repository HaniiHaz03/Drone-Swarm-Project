using UnityEngine;
using System.Collections.Generic;

public class DroneSpawner2D : MonoBehaviour
{
    public GameObject RedDronePrefab;
    public GameObject BlueDronePrefab;

    public int dronesPerColor = 50; // Number of drones per partition

    private List<CommunicatingDrone> redDrones = new List<CommunicatingDrone>();
    private List<CommunicatingDrone> blueDrones = new List<CommunicatingDrone>();

    void Start()
    {
        // Ensure the Flock object is present
        Flock flock = FindObjectOfType<Flock>();
        if (flock != null)
        {
            // Spawn and setup drones for each partition
            SpawnDrones(RedDronePrefab, redDrones, "Red", flock);
            SpawnDrones(BlueDronePrefab, blueDrones, "Blue", flock);
        }
        else
        {
            Debug.LogError("Flock not found in the scene. Please add a Flock component.");
        }
    }

    // Function to spawn drones and add them to the appropriate list
    void SpawnDrones(GameObject prefab, List<CommunicatingDrone> droneList, string colorTag, Flock flock)
    {
        for (int i = 0; i < dronesPerColor; i++)
        {
            GameObject droneObject = Instantiate(prefab, Vector2.zero, Quaternion.identity);
            CommunicatingDrone drone = droneObject.GetComponent<CommunicatingDrone>();

            if (drone == null)
            {
                Debug.LogError($"CommunicatingDrone component is missing on the {colorTag} drone prefab.");
                continue;
            }

            droneObject.name = $"{colorTag}Drone{i}"; // Assign unique names for debugging
            droneObject.tag = colorTag; // Use tag for easier identification

            droneList.Add(drone); // Add to respective list
            flock.AddDrone(drone); // Add the drone to the flock

            // Add this debug log to verify that the drone is part of the flock
            Debug.Log($"Drone {droneObject.name} added to flock: {drone.flock != null}");
        }
    }
}
