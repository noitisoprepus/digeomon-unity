using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Data")]
    public List<Dialogue> dialogues;
    public float textSpeed = 0.2f;
    public bool startOnReady;
    public UnityEvent onDialogueFinished;

    [Header("Dialogue GUI")]
    public GameObject dialoguePanel;
    public RectTransform dialogueCharImageRT;
    public RectTransform dialogueArrowRT;
    public TextMeshProUGUI charNameText;
    public TextMeshProUGUI dialogueText;

    private Image dialogueCharImage;
    private Sequence arrowSequence;
    private Coroutine dialogueCoroutine;
    private Dialogue currDialogue;
    private List<string> currLines;
    private int currDialogueIndex = 0;
    private int currLineIndex = 0;
    private bool isRunning = false;
    private bool isPlaying = false;

    private void Awake()
    {
        dialogueCharImage = dialogueCharImageRT.gameObject.GetComponent<Image>();
        dialoguePanel.GetComponentInChildren<DialogueInput>().onClick.AddListener(OnDialogueBoxTapped);
    }

    private void Start()
    {
        arrowSequence = DOTween.Sequence();
        arrowSequence.Append(dialogueArrowRT.DOAnchorPosX(-80f, 0.5f));
        arrowSequence.Append(dialogueArrowRT.DOAnchorPosX(-100f, 0.5f));
        arrowSequence.SetLoops(-1, LoopType.Yoyo).SetSpeedBased();

        if (startOnReady)
            StartDialogue();
        else
            dialoguePanel.SetActive(false);
    }

    private void OnDialogueBoxTapped()
    {
        if (isRunning)
            NextLine();
    }

    public void StartDialogue()
    {
        currDialogueIndex = 0;
        SetupDialogueUI(dialogues[currDialogueIndex]);
        currDialogue = dialogues[currDialogueIndex];
        RunDialogue(currDialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        SetupDialogueUI(dialogue);
        RunDialogue(dialogue);
    }

    private void SetupDialogueUI(Dialogue dialogue)
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
            onDialogueFinished?.Invoke();
            isRunning = false;
            dialoguePanel.SetActive(false);
            return;
        }

        currLineIndex++;
        dialogueCoroutine = StartCoroutine(PlayLine(currLines[currLineIndex]));
    }

    private void RunDialogue(Dialogue dialogue)
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
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(textSpeed);
        }
        CheckFuncToken(line);
        OnLineFinished();
    }

    private void CheckFuncToken(string line)
    {
        if (line.Contains("[") && line.Contains("]"))
        {
            int startIndex = line.IndexOf("[") + "[".Length;
            int endIndex = line.IndexOf("]");

            string functionName = line.Substring(startIndex, endIndex - startIndex);

            if (!string.IsNullOrEmpty(functionName))
            {
                switch (functionName)
                {
                    case "InputGradeLevel":
                        //InputGradeLevel();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnLineFinished()
    {
        dialogueText.maxVisibleCharacters = currLines[currLineIndex].Length;
        dialogueArrowRT.gameObject.SetActive(true);
        arrowSequence.Play();
        isPlaying = false;
    }
}
