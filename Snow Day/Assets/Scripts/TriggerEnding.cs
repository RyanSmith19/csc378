using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEnding : MonoBehaviour
{
    // Name of the end credits scene
    public string endCreditsSceneName = "EndCredits";

    // This method is called when another collider enters the trigger collider attached to the object this script is attached to
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name); // Log the name of the object entering the trigger

        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger"); // Log when the player enters the trigger

            // Load the end credits scene
            SceneManager.LoadScene(endCreditsSceneName);
        }
    }
}
