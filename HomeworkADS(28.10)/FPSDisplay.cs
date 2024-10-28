using UnityEngine;
using TMPro; // Include the TextMeshPro namespace

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Reference to the TextMeshPro UI element
    private float deltaTime;

    void Update()
    {
        // Calculate the time it takes to render the frame
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate the FPS
        float fps = 1.0f / deltaTime;

        // Update the UI text with the current FPS
        fpsText.text = string.Format("FPS: {0:0.} ", fps);
    }
}
