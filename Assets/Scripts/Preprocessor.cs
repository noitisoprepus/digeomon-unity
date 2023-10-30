using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Preprocessor : MonoBehaviour
{

    RenderTexture renderTexture;
    Vector2 scale = new Vector2(1, 1);
    Vector2 offset = Vector2.zero;

    UnityAction<byte[]> callback;

    public void PreprocessImage(Texture2D camTexture, int desiredSize, UnityAction<byte[]> callback)
    {

        this.callback = callback;

        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(desiredSize, desiredSize, 0, RenderTextureFormat.ARGB32);
        }

        scale.x = (float)camTexture.height / (float)camTexture.width;
        offset.x = (1 - scale.x) / 2f;
        Graphics.Blit(camTexture, renderTexture, scale, offset);
        AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGB24, OnCompleteReadback);
    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {

        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
            return;
        }

        callback.Invoke(request.GetData<byte>().ToArray());
    }
}