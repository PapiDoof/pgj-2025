using UnityEngine;

public class Ending : MonoBehaviour
{
    public Animator playerAnimator; // Reference to the player's Animator
    private Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D
    private Movement playerMovementScript; // Reference to the player's movement script

    // Conditions for endings
    private bool NPC1 = false; // Set to true when NPC1 condition is met
    private bool NPC2 = false; // Set to true when NPC2 condition is met
    private bool MountainTop = false; // To track if player reaches the MountainTop

    private void Start()
    {
        // Find the player GameObject in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            playerMovementScript = player.GetComponent<Movement>();
        }

        // Error handling if references are not found
        if (playerAnimator == null || playerRigidbody == null || playerMovementScript == null)
        {
            Debug.LogError("Ensure the player has Animator, Rigidbody2D, and Movement components.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            // Set the MountainTop bool to true
            MountainTop = true;
            playerAnimator.SetBool("MountainTop", true);

            // Stop the player's movement
            if (playerRigidbody != null)
            {
                playerRigidbody.linearVelocity = Vector2.zero; // Stop all velocity
                playerMovementScript.enabled = false; // Disable movement script
            }

            // Check for ending conditions
            if (MountainTop && NPC1 && NPC2)
            {
                TriggerGoodEnding();
            }
            else if (MountainTop)
            {
                TriggerBadEnding();
            }
        }
    }

    private void TriggerGoodEnding()
    {
        Debug.Log("Good Ending triggered!");
        // Add logic here for the "Good Ending"
        // E.g., trigger a cutscene, load a scene, or show a message
    }

    private void TriggerBadEnding()
    {
        Debug.Log("Bad Ending triggered!");
        // Add logic here for the "Bad Ending"
        // E.g., trigger a cutscene, load a scene, or show a message
    }

    // Public methods to set NPC conditions from other scripts
    public void SetNPC1(bool value)
    {
        NPC1 = value;
    }

    public void SetNPC2(bool value)
    {
        NPC2 = value;
    }
}
