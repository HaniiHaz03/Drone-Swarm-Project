using System;
using UnityEngine;  // Ensure UnityEngine is used for logging

public class DroneCommunication : MonoBehaviour
{
    public CommunicatingDrone Drone { get; private set; } // Use CommunicatingDrone to align with your previous code
    public DroneCommunication Next { get; set; } // Reference to the next communication node

    public DroneCommunication(CommunicatingDrone drone)
    {
        Drone = drone;
    }
    
    // Method to send a message to a specific drone by traversing the linked listA
    public void SendMessage(string message, CommunicatingDrone targetDrone)
    {
        DroneCommunication current = this;
        while (current != null)
        {
            if (current.Drone == targetDrone)
            {
                Debug.Log($"Message to {targetDrone.name}: {message}");
                return; // Stop once the message is sent
            }
            current = current.Next;
        }

        Debug.Log("Target drone not found in the communication chain.");
    }
}
