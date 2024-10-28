using UnityEngine;

public class CommunicatingDrone : MonoBehaviour
{
    public CommunicatingDrone nextDrone;  // Reference to the next drone in the linked list
    public Flock flock; // Reference to the flock this drone belongs to
    public float speed = 2f;
    public float neighborRadius = 3f; // Radius to detect nearby drones
    public float avoidanceRadius = 1f; // Radius to avoid crowding

    private Vector2 targetPosition;

    void Start()
    {
        GetNewTargetPosition();

        // Ensure each drone gets a unique name
        if (string.IsNullOrEmpty(this.name))
        {
            this.name = "Drone_" + Random.Range(1000, 9999); // Assign a unique name to each drone
        }

        Debug.Log($"{name} is part of flock: {flock != null}");
    }

    void Update()
    {
        MoveToTarget();
    }

    void GetNewTargetPosition()
    {
        // Set a random target position for the drone to move towards
        targetPosition = new Vector2(
            Random.Range(-10f, 10f), // Adjust range as needed
            Random.Range(-5f, 5f)   // Adjust range as needed
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
}








