using System.Collections.Generic;
using UnityEngine;

public class DroneNetworkCommunication
{
    private List<CommunicatingDrone> drones = new List<CommunicatingDrone>();

    public void AddDrone(CommunicatingDrone drone)
    {
        if (!drones.Contains(drone))
        {
            drones.Add(drone);
        }
    }

    public void RemoveDrone(CommunicatingDrone drone)
    {
        drones.Remove(drone);
    }

    // Method to send a message to a specific drone within the network
    public void SendMessage(string message, CommunicatingDrone targetDrone)
    {
        if (drones.Contains(targetDrone))
        {
            Debug.Log($"Message to {targetDrone.name}: {message}");
        }
        else
        {
            Debug.Log("Target drone not found in the network.");
        }
    }

    public CommunicatingDrone SearchByKey(string key)
    {
        return drones.Find(drone => drone.droneKey.Equals(key));
    }
}

