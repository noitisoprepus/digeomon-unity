using DG.Tweening;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Core
{
    public class SandboxManager : MonoBehaviour
    {
        [SerializeField] private Transform digeomonModelT;

        [Header("TESTING HOLOGRAM")]
        [SerializeField] private HologramCanvas hologramCanvas;
        [SerializeField] private List<InformationalData> informationalData;

        // TESTING HOLOGRAM
        public void OnHologramInitialized()
        {
            hologramCanvas.InitializeInformationalData(informationalData);
        }

        // TESTING SHAKE
        public void ShakeObject(GameObject obj)
        {
            GameObject go = Instantiate(obj, digeomonModelT);
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);
        }
    }
}