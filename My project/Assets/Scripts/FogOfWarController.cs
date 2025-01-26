using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class FogOfWarController : MonoBehaviour
{
    public Transform player; // Reference to the player
    public UnityEngine.UI.Image fogImage; // Reference to the UI Image component
    public float maxFogDistance = 1.0f; // Maximum distance of the fog from the player
    public float fogSpeed = 0.01f; // Speed at which the fog builds up

    private Material fogMaterial;

    void Start()
    {
        // Get the material from the Image component
        fogMaterial = fogImage.material;
    }

    void Update()
    {
        // Update the fog distance based on the player's Y position
        float playerY = player.position.y;
        float fogDistance = Mathf.Clamp01(playerY * fogSpeed / maxFogDistance); // Adjust fog distance based on Y position
        fogMaterial.SetFloat("_FogDistance", fogDistance);
    }
}