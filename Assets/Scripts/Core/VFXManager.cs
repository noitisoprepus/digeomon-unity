using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Core
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private DigeomonCaptureData digeomonCaptureData;

        [Header("Quiz Feedback VFX")]
        [SerializeField] private ParticleSystem successParticleSystem;
        [SerializeField] private ParticleSystem failParticleSystem;

        [Header("Evolution")]
        [SerializeField] private QuizManager quizManager;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Material evolutionMat;
        [SerializeField] private GameObject evolutionVFX;
        [SerializeField] private AudioClip evolutionSFX;

        private AudioSource audioSource;
        private GameObject digeomonObj;
        private GameObject evolutionObj;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            QuizManager.OnAnswerCorrectAction += OnDigeomonExcited;
            QuizManager.OnAnswerIncorrectAction += OnDigeomonInfuriated;
            QuizUI.OnEvolutionSuccessAction += OnEvolutionSuccessful;
        }

        private void OnDisable()
        {
            QuizManager.OnAnswerCorrectAction -= OnDigeomonExcited;
            QuizManager.OnAnswerIncorrectAction -= OnDigeomonInfuriated;
            QuizUI.OnEvolutionSuccessAction -= OnEvolutionSuccessful;
        }

        private void OnDigeomonInfuriated()
        {
            //digeomonObj.transform.DOShakePosition(1f, 0.25f, 12, 90);
            failParticleSystem.Play();
        }

        private void OnDigeomonExcited()
        {
            //digeomonObj.transform.DOShakePosition(1f, 0.1f, 1, 10);
            successParticleSystem.Play();
        }

        public void InitializeEvolution(GameObject digeomon, DigeomonData evolution)
        {
            digeomonObj = digeomon;
            evolutionObj = Instantiate(evolution.modelPrefab, digeomon.transform.position, digeomon.transform.rotation);
            evolutionObj.transform.localScale = Vector3.zero;

            HologramDisplay hologramDisplay = evolutionObj.GetComponentInChildren<HologramDisplay>();
            hologramDisplay.digeomon = evolution;
            hologramDisplay.SetupEvolveButton();

            HologramCanvas hologramCanvas = evolutionObj.GetComponentInChildren<HologramCanvas>();
            List<InformationalData> infoList = new List<InformationalData>(evolution.relevantInfos);
            hologramCanvas.InitializeInformationalData(infoList);

            quizManager.StartQuiz();
        }

        private void OnEvolutionSuccessful()
        {
            StartCoroutine(EvolveDigeomon());
            PersistentData.toEvolve = false;
        }

        private IEnumerator EvolveDigeomon()
        {
            digeomonObj.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = evolutionMat;
            digeomonObj.transform.DOShakeScale(3f, 0.4f, 5, 90, true, ShakeRandomnessMode.Harmonic);

            yield return new WaitForSeconds(3f);

            evolutionObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);
            digeomonObj.transform.parent.gameObject.SetActive(false);
            Instantiate(evolutionVFX, evolutionObj.transform.position, evolutionObj.transform.rotation);

            audioSource.clip = evolutionSFX;
            audioSource.Play();

            dialogueManager.StartDialogue(PersistentData.targetDigeomon.introDialogue);
        }
    }
}