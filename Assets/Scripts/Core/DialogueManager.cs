using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dialogue;

namespace Core
{
    public class DialogueManager : MonoBehaviour
    {
        public delegate void DialogueStartDelegate();
        public static event DialogueStartDelegate OnDialogueStartAction;

        public delegate void DialogueEndDelegate();
        public static event DialogueEndDelegate OnDialogueEndAction;

        [Header("Dialogue Data")]
        [SerializeField] private List<DialogueData> dialogues;
        [SerializeField] private float textSpeed = 0.2f;

        [Header("Dialogue GUI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Image dialogueCharImage;
        [SerializeField] private RectTransform dialogueCharImageRT;
        [SerializeField] private RectTransform dialogueArrowRT;
        [SerializeField] private TextMeshProUGUI charNameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        private AudioSource dialogueTextAudio;
        private Sequence arrowSequence;
        private Coroutine dialogueCoroutine;
        private DialogueData currDialogue;
        private List<string> currLines;
        private int currDialogueIndex = 0;
        private int currLineIndex = 0;
        private bool isRunning = false;
        private bool isPlaying = false;

        private void Awake()
        {
            dialoguePanel.GetComponentInChildren<DialogueInput>().onClick.AddListener(OnDialogueBoxTapped);
            dialogueTextAudio = dialogueText.gameObject.GetComponent<AudioSource>();
        }

        private void Start()
        {
            arrowSequence = DOTween.Sequence();
            arrowSequence.Append(dialogueArrowRT.DOAnchorPosX(-80f, 0.5f));
            arrowSequence.Append(dialogueArrowRT.DOAnchorPosX(-100f, 0.5f));
            arrowSequence.SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
        }

        private void OnDialogueBoxTapped()
        {
            if (isRunning)
                NextLine();
        }

        public void StartDialogue()
        {
            OnDialogueStartAction?.Invoke();
            currDialogueIndex = 0;
            SetupDialogueUI(dialogues[currDialogueIndex]);
            currDialogue = dialogues[currDialogueIndex];
            RunDialogue(currDialogue);
        }

        public void StartDialogue(DialogueData dialogue)
        {
            OnDialogueStartAction?.Invoke();
            SetupDialogueUI(dialogue);
            RunDialogue(dialogue);
        }

        private void SetupDialogueUI(DialogueData dialogue)
        {
            dialogueCharImage.sprite = dialogue.charSprite;
            dialogueCharImageRT.anchoredPosition = new Vector2(0, -1030f);
            dialoguePanel.SetActive(true);
            dialogueCharImageRT.DOAnchorPosY(0, 1.5f).SetEase(Ease.OutQuint);

            isRunning = true;
        }

        private void NextLine()
        {
            if (isPlaying)
            {
                StopCoroutine(dialogueCoroutine);
                OnLineFinished();
                return;
            }

            if (currLineIndex.Equals(currLines.Count - 1))
            {
                currDialogueIndex++;
                isRunning = false;
                dialoguePanel.SetActive(false);
                OnDialogueEndAction?.Invoke();
                return;
            }

            currLineIndex++;
            dialogueCoroutine = StartCoroutine(PlayLine(currLines[currLineIndex]));
        }

        private void RunDialogue(DialogueData dialogue)
        {
            charNameText.SetText(dialogue.charName);

            currLines = new List<string>(dialogue.dialogues);
            dialogueCoroutine = StartCoroutine(PlayLine(currLines[0]));
            currLineIndex = 0;
        }

        private IEnumerator PlayLine(string line)
        {
            dialogueArrowRT.gameObject.SetActive(false);
            isPlaying = true;
            dialogueText.SetText(line);
            dialogueText.maxVisibleCharacters = 0;

            while (dialogueText.maxVisibleCharacters != line.Length)
            {
                dialogueTextAudio.Play();
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(textSpeed);
            }
            //CheckFuncToken(line);
            OnLineFinished();
        }

        // Disabled for now. Not being used.
        //private void CheckFuncToken(string line)
        //{
        //    if (line.Contains("[") && line.Contains("]"))
        //    {
        //        int startIndex = line.IndexOf("[") + "[".Length;
        //        int endIndex = line.IndexOf("]");

        //        string functionName = line.Substring(startIndex, endIndex - startIndex);

        //        if (!string.IsNullOrEmpty(functionName))
        //        {
        //            switch (functionName)
        //            {
        //                case "InputGradeLevel":
        //                    //InputGradeLevel();
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //}

        private void OnLineFinished()
        {
            dialogueText.maxVisibleCharacters = currLines[currLineIndex].Length;
            dialogueArrowRT.gameObject.SetActive(true);
            arrowSequence.Play();
            isPlaying = false;
        }
    }
}