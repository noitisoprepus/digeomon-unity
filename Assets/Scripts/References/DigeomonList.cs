using System.Collections.Generic;
using UnityEngine;

public class DigeomonList : MonoBehaviour
{
    [Header("Capture Data")]
    [SerializeField] private DigeomonCaptureData digeomonCaptureData;

    [Header("Digeomon Repository")]
    public List<DigeomonData> digeomons;

    private void Awake()
    {
        foreach(DigeomonData digeomon in digeomons)
        {
            if (!digeomonCaptureData.captureData.ContainsKey(digeomon.name))
                digeomonCaptureData.AddDigeomonData(digeomon);
        }
    }
}
