using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public Sprite spriteDefault; // Default sprite
    //public Sprite spriteYRange; // Sprite to use when y is between 10 and 20

    private SpriteRenderer spriteRenderer;
    //private Transform playerTransform;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       // playerTransform = transform.parent; // The parent is the player

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is missing from the attached sprite.");
        }

       // if (playerTransform == null)
       // {
       //     Debug.LogError("This script must be attached to a child of the player.");
       // }
    }

    private void Update()
    {
        //if (playerTransform == null || spriteRenderer == null)
        //    return;

        //float playerY = playerTransform.position.y;

        //Check y-position and change sprite accordingly
        //if (playerY >= 10f && playerY <= 20f)
        //{
        //    spriteRenderer.sprite = spriteYRange;
        //}
        //else
        //{
        //    spriteRenderer.sprite = spriteDefault;
        //}


    }
}
