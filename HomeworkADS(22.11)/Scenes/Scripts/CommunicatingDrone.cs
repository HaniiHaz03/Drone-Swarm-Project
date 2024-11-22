using UnityEngine;
using System; // For Guid to generate unique drone key
using System.Collections.Generic;

public class CommunicatingDrone : MonoBehaviour
{
    public CommunicatingDrone nextDrone;  // Reference to the next drone in the linked list
    public string droneKey; // Unique key for each drone
    public Flock flock; // Reference to the flock this drone belongs to
    public float speed = 2f;
    public float neighborRadius = 3f; // Radius to detect nearby drones
    public float avoidanceRadius = 1f; // Radius to avoid crowding

    public Vector2 targetPosition;
    private List<CommunicatingDrone> neighbors = new List<CommunicatingDrone>(); // List of neighboring drones

    void Start()
    {
        GetNewTargetPosition();
        droneKey = this.name;

        // Ensure each drone gets a unique name if not already set
        if (string.IsNullOrEmpty(this.name))
        {
            this.name = "Drone_" + UnityEngine.Random.Range(1000, 9999);
        }

        // Ensure each drone gets a unique key if not already set
        if (string.IsNullOrEmpty(droneKey))
        {
            droneKey = Guid.NewGuid().ToString(); // Generate a unique key using GUID
        }

        // Log the drone's name and key for debugging
        Debug.Log($"{name} (Key: {droneKey}) is part of flock: {flock != null}");
    }

    void Update()
    {
        MoveToTarget();
    }

    void GetNewTargetPosition()
    {
        // Set a random target position for the drone to move towards
        targetPosition = new Vector2(
            UnityEngine.Random.Range(-10f, 10f),
            UnityEngine.Random.Range(-5f, 5f)
        );
    }

    void MoveToTarget()
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;

        // Move towards the target considering the flocking direction
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // When reaching the target, get a new one
        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            GetNewTargetPosition();
        }
    }

    public void SelfDestruct()
    {
        // Deactivate the drone (it will disappear from the scene)
        this.gameObject.SetActive(false);
    }

    // Add a neighboring drone
    public void AddNeighbor(CommunicatingDrone neighbor)
    {
        if (neighbor != null && neighbor != this && !neighbors.Contains(neighbor))
        {
            neighbors.Add(neighbor);
            Debug.Log($"{name} added neighbor {neighbor.name}");
        }
    }

    // Remove a neighboring drone
    public void RemoveNeighbor(CommunicatingDrone neighbor)
    {
        if (neighbors.Contains(neighbor))
        {
            neighbors.Remove(neighbor);
            Debug.Log($"{name} removed neighbor {neighbor.name}");
        }
    }

    // Get the list of current neighbors
    public List<CommunicatingDrone> GetNeighbors()
    {
        return neighbors;
    }
}
