using UnityEngine;
using System.Collections.Generic;

public class DroneSpawner2D : MonoBehaviour
{
    public GameObject RedDronePrefab;
    public GameObject BlueDronePrefab;

    public int dronesPerColor = 50;

    private List<CommunicatingDrone> redDrones = new List<CommunicatingDrone>();
    private List<CommunicatingDrone> blueDrones = new List<CommunicatingDrone>();
    private DroneNetworkCommunication redNetwork = new DroneNetworkCommunication();
    private DroneNetworkCommunication blueNetwork = new DroneNetworkCommunication();

    void Start()
    {
        Flock flock = FindObjectOfType<Flock>();
        if (flock != null)
        {
            // Force initialization of partitions
            flock.partitions["Red"] = new List<CommunicatingDrone>();
            flock.partitions["Blue"] = new List<CommunicatingDrone>();
            flock.partitions["Neutral"] = new List<CommunicatingDrone>();

            SpawnDrones(RedDronePrefab, redDrones, "Red", flock, redNetwork);
            SpawnDrones(BlueDronePrefab, blueDrones, "Blue", flock, blueNetwork);
        }
        else
        {
            Debug.LogError("Flock not found in the scene. Please add a Flock component.");
        }
    }

    void SpawnDrones(GameObject prefab, List<CommunicatingDrone> droneList, string colorTag, Flock flock, DroneNetworkCommunication network)
    {
        for (int i = 0; i < dronesPerColor; i++)
        {
            GameObject droneObject = Instantiate(prefab, Vector2.zero, Quaternion.identity);
            CommunicatingDrone drone = droneObject.GetComponent<CommunicatingDrone>();

            droneObject.name = $"{colorTag}Drone{i}";
            droneObject.tag = colorTag;
            drone.droneKey = droneObject.name; // Ensure keys match drone names

            if (drone != null)
            {
                droneList.Add(drone);
                flock.AddDrone(drone);

                if (flock.partitions.ContainsKey(colorTag))
                {
                    flock.partitions[colorTag].Add(drone); // Add to partition
                }
                else
                {
                    Debug.LogError($"Partition '{colorTag}' does not exist in the Flock component.");
                }

                network.AddDrone(drone);
                Debug.Log($"Spawned drone with key: {drone.droneKey}");
            }
        }
    }

}
