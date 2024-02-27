using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class SandboxManager : MonoBehaviour
    {
        [SerializeField] private Transform digeomonModelT;

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