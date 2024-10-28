using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DroneUIManager : MonoBehaviour
{
    public TMP_InputField searchInputField; // Use TMP_InputField for TextMeshPro
    public Button searchButton; // Use Button for the search button
    public Button selfDestructButton; // Button for self-destruct
    public TMP_Text resultText; // Use TMP_Text for TextMeshPro text
    public TMP_Text simulationTimeText; // Use TMP_Text for simulation time
    public TMP_Text frameRateText; // Use TMP_Text for frame rate display

    public Flock droneFlock; // Reference to the Flock
    private float elapsedTime = 0f;

    void Start()
    {
        // Assign the button's onClick event to the search function
        searchButton.onClick.AddListener(OnSearchDrone);
        selfDestructButton.onClick.AddListener(OnSelfDestructDrone);
    }

    void Update()
    {
        UpdateFrameRate();
    }

    void UpdateFrameRate()
    {
        float frameRate = 1.0f / Time.deltaTime;
        frameRateText.text = "Framerate: " + frameRate.ToString("F2") + " FPS";
    }

    void OnSearchDrone()
    {
        string droneId = searchInputField.text.Trim(); // Trim any leading/trailing spaces
        Debug.Log("Searching for drone: " + droneId); // Debug log for the search query

        CommunicatingDrone drone = droneFlock.FindDroneByName(droneId);

        if (drone != null && drone.gameObject.activeSelf)
        {
            resultText.text = "Drone " + droneId + " Position: " + drone.transform.position;
            Debug.Log($"Found drone: {droneId} at position {drone.transform.position}");
            float distance = CalculateDistanceToDrone(drone);
            elapsedTime = distance / droneFlock.maxSpeed;
            simulationTimeText.text = "Simulated Time: " + elapsedTime.ToString("F2") + " s";
        }
        else
        {
            resultText.text = "Drone not found!";
            Debug.Log("Drone not found: " + droneId); // Debug log for not found
        }
    }

    void OnSelfDestructDrone()
    {
        string droneId = searchInputField.text.Trim(); // Get the ID from the input field
        Debug.Log("Self-destructing drone: " + droneId); // Debug log for self-destruct

        CommunicatingDrone selfDestructDrone = droneFlock.FindDroneByName(droneId);

        if (selfDestructDrone != null && selfDestructDrone.gameObject.activeSelf)
        {
            // Call the self-destruct method (deactivate the drone)
            selfDestructDrone.SelfDestruct();

            // Remove the drone from the drone list in the Flock
            droneFlock.RemoveDrone(selfDestructDrone);

            resultText.text = "Drone " + droneId + " has self-destructed.";
            Debug.Log($"Drone {droneId} self-destructed and removed from the flock.");
        }
        else
        {
            resultText.text = "Drone not found for self-destruct!";
            Debug.Log("Drone not found for self-destruct: " + droneId);
        }
    }

    float CalculateDistanceToDrone(CommunicatingDrone drone)
    {
        return Vector2.Distance(drone.transform.position, transform.position); // Calculate distance
    }
}







