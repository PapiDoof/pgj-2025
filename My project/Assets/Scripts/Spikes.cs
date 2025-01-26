using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Vector3 playerStartPosition;

    private void Start()
    {
        // Store the player's starting position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStartPosition = player.transform.position;
        }
        else
        {
            Debug.LogError("No GameObject tagged 'Player' found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is tagged as "Player"
        if (collision.CompareTag("Player"))
        {
            RespawnPlayer(collision.gameObject);
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        // Reset the player's position to the starting position
        player.transform.position = playerStartPosition;
        Debug.Log("Player respawned at the starting position.");
    }
}
