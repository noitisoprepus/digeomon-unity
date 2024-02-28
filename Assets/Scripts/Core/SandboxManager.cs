using System.Collections.Generic;
using DG.Tweening;
using UI;
using UnityEngine;

namespace Core
{
    public class SandboxManager : MonoBehaviour
    {
        [SerializeField] private Transform digeomonModelT;
        [SerializeField] private InformationalData placeholderInfo;

        private GameObject digeomonObj;

        private void OnEnable()
        {
            QuizManager.OnAnswerCorrectAction += GentleShakeDigeomon;
            QuizManager.OnAnswerIncorrectAction += ViolentShakeDigeomon;
        }

        private void OnDisable()
        {
            QuizManager.OnAnswerCorrectAction -= GentleShakeDigeomon;
            QuizManager.OnAnswerIncorrectAction -= ViolentShakeDigeomon;
        }

        private void Start()
        {
            DigeomonData digeomon = PersistentData.targetDigeomon;
            digeomonObj = Instantiate(digeomon.modelPrefab, digeomonModelT);
            
            HologramCanvas hologram = digeomonObj.GetComponentInChildren<HologramCanvas>();
            if (digeomon.relevantInfos != null)
                hologram.informationalContent = digeomon.relevantInfos;
            else
                hologram.informationalContent = new List<InformationalData>() { placeholderInfo };

            hologram.StartPresentation();
        }

        private void ViolentShakeDigeomon()
        {
            digeomonObj.transform.DOShakePosition(1f, 0.25f, 12, 90);
        }

        private void GentleShakeDigeomon()
        {
            digeomonObj.transform.DOShakePosition(1f, 0.1f, 1, 10);
        }
    }
}