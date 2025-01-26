using UnityEngine;
using UnityEngine.Rendering;

public class PostProcess : MonoBehaviour
{
    public Shader shader;
    public Material grayscaleMaterial;  // The material with the custom grayscale shader
    public Texture2D noiseTexture;  // The noise texture used to control the grayscale effect
    private Camera mainCamera;  // Reference to the camera

    void OnEnable()
    {
        // Subscribe to the render pipeline events
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    // Called when the camera starts rendering
    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        mainCamera = camera;
        ApplyGrayscaleEffect();
    }

    // Called when the camera finishes rendering
    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        // Reset the material after rendering
        if (grayscaleMaterial != null)
        {
            // You can reset properties or clean up if needed here
        }
    }

    private void ApplyGrayscaleEffect()
    {
        if (grayscaleMaterial == null || mainCamera == null)
            return;

        // Set properties for the material (pass Y position from player or other logic)
        grayscaleMaterial.SetFloat("_YPosition", mainCamera.transform.position.y);
        grayscaleMaterial.SetTexture("_NoiseTex", noiseTexture);

        // Use OnRenderImage to apply the effect during the final render step
        mainCamera.AddCommandBuffer(CameraEvent.AfterEverything, CreateCommandBuffer());
    }

    private CommandBuffer CreateCommandBuffer()
    {
        CommandBuffer commandBuffer = new CommandBuffer { name = "Apply Grayscale Effect" };

        // Apply grayscale material to the screen
        commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CurrentActive, grayscaleMaterial);

        return commandBuffer;
    }

    // This function can be used for debugging and verifying if the effect is being applied
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (grayscaleMaterial != null)
        {
            Graphics.Blit(src, dest, grayscaleMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
