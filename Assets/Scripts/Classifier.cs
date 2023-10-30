using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Classification : MonoBehaviour
{
    [Header("Classification Model")]
    public NNModel modelFile;
    public TextAsset classesTxt;
    public Preprocessor preprocess;

    [Header("UI")]
    public TextMeshProUGUI detectedObjText;
    public RenderTexture cameraRenderTexture;
    public ARCameraBackground m_ARCameraBackground;
    public RawImage cameraOutput;

    const int IMAGE_SIZE = 224;
    const string INPUT_NAME = "images";
    const string OUTPUT_NAME = "Softmax";

    private IWorker worker;
    private Texture2D cameraTexture;
    private string[] classLabels;

    private void Start()
    {
        Model model = ModelLoader.Load(modelFile);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        LoadClasses();
        cameraTexture = new Texture2D(1080, 1920, TextureFormat.RGB24, false);
    }

    private void Update()
    {
        // Blit the camera image to the Render Texture
        Graphics.Blit(null, cameraRenderTexture, m_ARCameraBackground.material);

        // Read the Render Texture into a Texture2D for processing
        RenderTexture.active = cameraRenderTexture;
        cameraTexture.ReadPixels(new Rect(0, 0, cameraRenderTexture.width, cameraRenderTexture.height), 0, 0);
        cameraTexture.Apply();
        RenderTexture.active = null;

        // Re-outputs the Camera image texture, so it can be seen in-game
        cameraOutput.texture = cameraTexture;
        preprocess.PreprocessImage(cameraTexture, IMAGE_SIZE, RunModel);
    }

    void RunModel(byte[] pixels)
    {
        StartCoroutine(RunModelRoutine(pixels));
    }

    IEnumerator RunModelRoutine(byte[] pixels)
    {
        Tensor tensor = TransformInput(pixels);

        var inputs = new Dictionary<string, Tensor> {
            { INPUT_NAME, tensor }
        };

        worker.Execute(inputs);
        Tensor outputTensor = worker.PeekOutput(OUTPUT_NAME);

        //get largest output
        List<float> temp = outputTensor.ToReadOnlyArray().ToList();
        float max = temp.Max();
        int index = temp.IndexOf(max);

        //set UI text
        detectedObjText.text = classLabels[index];

        //dispose tensors
        tensor.Dispose();
        outputTensor.Dispose();
        yield return null;
    }

    //transform from 0-255 to -1 to 1
    Tensor TransformInput(byte[] pixels)
    {
        float[] transformedPixels = new float[pixels.Length];

        for (int i = 0; i < pixels.Length; i++)
        {
            transformedPixels[i] = (pixels[i] - 127f) / 128f;
        }
        return new Tensor(1, IMAGE_SIZE, IMAGE_SIZE, 3, transformedPixels);
    }

    private void OnDestroy()
    {
        worker?.Dispose();
    }

    void LoadClasses()
    {
        if (classesTxt != null)
        {
            string fileContents = classesTxt.text;
            classLabels = fileContents.Split('\n');
        }
    }
}