using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class SandboxManager : MonoBehaviour
    {
        [SerializeField] private Transform digeomonModelT;

        [Header("Feedback VFX")]
        [SerializeField] private ParticleSystem successParticleSystem;
        [SerializeField] private ParticleSystem failParticleSystem;

        [Header("Evolution")]
        [SerializeField] private Material evolutionMat;
        [SerializeField] private ParticleSystem evolutionVFX;

        private DialogueManager dialogueManager;
        private GameObject digeomonObj;
        private GameObject evolutionObj;

        private void Awake()
        {
            dialogueManager = GetComponent<DialogueManager>();
        }

        private void OnEnable()
        {
            QuizManager.OnAnswerCorrectAction += OnDigeomonExcited;
            QuizManager.OnAnswerIncorrectAction += OnDigeomonInfuriated;
            QuizManager.OnEvolutionSuccessAction += OnEvolutionSuccessful;
        }

        private void OnDisable()
        {
            QuizManager.OnAnswerCorrectAction -= OnDigeomonExcited;
            QuizManager.OnAnswerIncorrectAction -= OnDigeomonInfuriated;
            QuizManager.OnEvolutionSuccessAction -= OnEvolutionSuccessful;
        }

        private void Start()
        {
            DigeomonData digeomon = PersistentData.targetDigeomon;

            if (PersistentData.toEvolve)
            {
                digeomonObj = Instantiate(digeomon.preEvolution.modelPrefab, digeomonModelT);
                evolutionObj = Instantiate(digeomon.modelPrefab, digeomonModelT);
                evolutionObj.transform.localScale = Vector3.zero;
                return;
            }

            digeomonObj = Instantiate(digeomon.modelPrefab, digeomonModelT);
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

        private void OnEvolutionSuccessful()
        {
            StartCoroutine(EvolveDigeomon());
            PersistentData.toEvolve = false;
        }

        private IEnumerator EvolveDigeomon()
        {
            digeomonObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = evolutionMat;
            digeomonObj.transform.DOShakeScale(3f, 0.4f, 5, 90, true, ShakeRandomnessMode.Harmonic);

            yield return new WaitForSeconds(3f);

            evolutionObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);
            digeomonObj.SetActive(false);
            evolutionVFX.Play();
            dialogueManager.StartDialogue(PersistentData.targetDigeomon.introDialogue);
        }

        // Testing
        public void ShakeObject(GameObject obj)
        {
            GameObject go = Instantiate(obj, digeomonModelT);
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuint);
        }
    }
}