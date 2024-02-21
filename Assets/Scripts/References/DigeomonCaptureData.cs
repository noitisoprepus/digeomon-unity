using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DigeomonCaptureData", menuName = "ScriptableObject/DigeomonCaptureData")]
public class DigeomonCaptureData : ScriptableObject
{
    public Dictionary<string, bool> captureData = new Dictionary<string, bool>();
    public Dictionary<string, DigeomonData> digeomonData = new Dictionary<string, DigeomonData>(); // YUCK

    public static event Action<string> OnDigeomonCapture;

    public void AddDigeomonData(DigeomonData digeomonData)
    {
        captureData.Add(digeomonData.name, false);
        this.digeomonData.Add(digeomonData.name, digeomonData);
    }

    public void SyncDigeomonData(string digeomonName)
    {
        captureData[digeomonName] = true;
    }

    public void CaptureDigeomon(DigeomonData digeomonData)
    {
        SyncDigeomonData(digeomonData.name);
        OnDigeomonCapture?.Invoke(digeomonData.name);
    }
}
