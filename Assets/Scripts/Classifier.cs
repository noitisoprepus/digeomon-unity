﻿using System.Collections;
using System.Collections.Generic;
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
    public Material preprocessMaterial;

    [Header("UI")]
    public Button scanButton;
    public List<Image> scanFrameSprites;
    public Color normalColor;
    public Color disabledColor;
    public TextMeshProUGUI detectedObjText;
    public RenderTexture cameraRenderTexture;
    public ARCameraBackground m_ARCameraBackground;

    private Capture capture;
    private IWorker worker;
    private Model model;
    private Dictionary<string, Tensor> inputs = new Dictionary<string, Tensor>();
    private RenderTexture targetRT;
    private Texture2D cameraTexture;
    private string[] classLabels;

    private void Awake()
    {
        capture = GetComponent<Capture>();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.Portrait;

        string fileContents = classesTxt.text;
        classLabels = fileContents.Split('\n');
        model = ModelLoader.Load(modelFile);

        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, model);
        targetRT = RenderTexture.GetTemporary(224, 224, 0, RenderTextureFormat.ARGBHalf);
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
    }

    private void OnDestroy()
    {
        worker?.Dispose();

        foreach (var key in inputs.Keys)
        {
            inputs[key].Dispose();
        }

        inputs.Clear();
    }

    private void ExecuteML()
    {
        // Preprocessing
        var input = new Tensor(PrepareTextureForInput(cameraTexture), 3);

        // Executing
        worker.Execute(input);

        // Reading Output
        var output = worker.PeekOutput();
        var res = output.ArgMax()[0];
        var label = classLabels[res];
        var accuracy = output[res];
        capture.SearchDigeomon(label, accuracy);

        StartCoroutine(ScanDelay());
        // FOR DEBUGGING
        // detectedObjText.text = $"{label}\n{Mathf.Round(accuracy * 100)}%";
    }

    Texture PrepareTextureForInput(Texture2D src)
    {
        RenderTexture.active = targetRT;
        Graphics.Blit(src, targetRT, preprocessMaterial);

        var result = new Texture2D(targetRT.width, targetRT.height, TextureFormat.RGBAHalf, false);
        result.ReadPixels(new Rect(0, 0, targetRT.width, targetRT.height), 0, 0);
        result.Apply();
        return result;
    }

    public void OnCaptureButtonPressed()
    {
        scanButton.interactable = false;
        scanButton.GetComponentInChildren<TextMeshProUGUI>().text = ". . .";
        foreach (Image img in scanFrameSprites)
            img.color = disabledColor;
        ExecuteML();
    }

    private IEnumerator ScanDelay()
    {
        yield return new WaitForSeconds(3f);
        scanButton.interactable = true;
        scanButton.GetComponentInChildren<TextMeshProUGUI>().text = "SCAN";
        foreach (Image img in scanFrameSprites)
            img.color = normalColor;
    }
}
