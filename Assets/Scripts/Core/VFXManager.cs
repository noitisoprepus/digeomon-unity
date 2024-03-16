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

        [Header("Capture Feedback VFX")]
        [SerializeField] private ParticleSystem confettiParticleSystem;
        [SerializeField] private ParticleSystem disintegrateVFX;

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
            QuizUI.OnQuizConcludeAction += OnDigeomonCapture;
        }

        private void OnDisable()
        {
            QuizManager.OnAnswerCorrectAction -= OnDigeomonExcited;
            QuizManager.OnAnswerIncorrectAction -= OnDigeomonInfuriated;
            QuizUI.OnQuizConcludeAction -= OnDigeomonCapture;
        }

        private void OnDigeomonInfuriated()
        {
            digeomonObj.transform.DOShakePosition(1f, 0.25f, 12, 90);
            failParticleSystem.Play();
        }

        private void OnDigeomonExcited()
        {
            digeomonObj.transform.DOShakePosition(1f, 0.1f, 1, 10);
            successParticleSystem.Play();
        }

        private void OnDigeomonCapture()
        {
            if (PersistentData.toEvolve)
            {
                if (digeomonCaptureData.captureData[PersistentData.targetDigeomon.name])
                    OnEvolutionSuccessful();
                PersistentData.toEvolve = false;
                return;
            }

            if (digeomonCaptureData.captureData[PersistentData.targetDigeomon.name])
                confettiParticleSystem.Play();
            else
            {
                Instantiate(disintegrateVFX, digeomonObj.transform.position, digeomonObj.transform.rotation);
                digeomonObj.transform.parent.gameObject.SetActive(false);
            }
        }

        private void SetupHologram(GameObject digeomon, DigeomonData digeomonData)
        {
            NameScreen nameScreen = digeomon.GetComponentInChildren<NameScreen>();
            nameScreen.SetName(digeomonData.name);

            HologramDisplay hologramDisplay = digeomon.GetComponentInChildren<HologramDisplay>();
            hologramDisplay.digeomon = digeomonData;
            hologramDisplay.SetupEvolveButton();

            HologramCanvas hologramCanvas = digeomon.GetComponentInChildren<HologramCanvas>();
            List<InformationalData> infoList = new List<InformationalData>(digeomonData.relevantInfos);
            hologramCanvas.InitializeInformationalData(infoList);
        }

        public void InitializeDigeomon(GameObject digeomon, DigeomonData digeomonData)
        {
            digeomonObj = digeomon.transform.GetChild(0).gameObject;
            quizManager.quizUI = digeomon.GetComponentInChildren<QuizUI>();
            SetupHologram(digeomon, digeomonData);
        }

        public void InitializeEvolution(GameObject digeomon, DigeomonData evolutionData)
        {
            digeomonObj = digeomon;
            quizManager.quizUI = digeomonObj.transform.parent.gameObject.GetComponentInChildren<QuizUI>();
            evolutionObj = Instantiate(evolutionData.modelPrefab, digeomon.transform.position, digeomon.transform.rotation);
            evolutionObj.transform.localScale = Vector3.zero;
            SetupHologram(evolutionObj, evolutionData);

            quizManager.StartQuiz();
        }

        private void OnEvolutionSuccessful()
        {
            StartCoroutine(EvolveDigeomon());
        }

        private IEnumerator EvolveDigeomon()
        {
            digeomonObj.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = evolutionMat;
            digeomonObj.transform.DOShakeScale(3f, 0.4f, 5, 90, true, ShakeRandomnessMode.Harmonic);

            yield return new WaitForSeconds(3f);

            evolutionObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);
            digeomonObj.transform.parent.gameObject.SetActive(false);
            Instantiate(evolutionVFX, evolutionObj.transform.position, evolutionObj.transform.rotation);
            confettiParticleSystem.Play();

            audioSource.clip = evolutionSFX;
            audioSource.Play();

            dialogueManager.StartDialogue(PersistentData.targetDigeomon.introDialogue);
        }
    }
}