using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Data")]
    public List<Dialogue> dialogues;
    public float textSpeed = 0.2f;

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
    }

    private void Start()
    {
        arrowSequence = DOTween.Sequence();
        arrowSequence.Append(dialogueArrowRT.DOAnchorPosX(-80f, 0.5f));
        arrowSequence.Append(dialogueArrowRT.DOAnchorPosX(-100f, 0.5f));
        arrowSequence.SetLoops(-1, LoopType.Yoyo).SetSpeedBased();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                NextDialogue();
            }
        }
        else
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                StartDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        if (dialogues.Count.Equals(currDialogueIndex)) return;
        isRunning = true;

        dialogueCharImage.sprite = dialogues[currDialogueIndex].charSprite;
        dialogueCharImageRT.anchoredPosition = new Vector2(0, -1030f);
        dialoguePanel.SetActive(true);
        dialogueCharImageRT.DOAnchorPosY(0, 1.5f).SetEase(Ease.OutQuint);

        currDialogue = dialogues[currDialogueIndex];
        RunDialogue(currDialogue);
        currDialogueIndex++;
    }

    public void NextDialogue()
    {
        if (isPlaying)
        {
            StopCoroutine(dialogueCoroutine);
            OnDialogueFinished();
            return;
        }

        if (currLineIndex.Equals(currLines.Count - 1))
        {
            isRunning = false;
            dialoguePanel.SetActive(false);
            return;
        }

        currLineIndex++;
        dialogueCoroutine = StartCoroutine(PlayDialogue(currLines[currLineIndex]));
    }

    private void RunDialogue(Dialogue dialogue)
    {
        charNameText.SetText(dialogue.charName);

        currLines = new List<string>(dialogue.dialogues);
        dialogueCoroutine = StartCoroutine(PlayDialogue(currLines[0]));
        currLineIndex = 0;
    }

    private IEnumerator PlayDialogue(string line)
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

        OnDialogueFinished();
    }

    private void OnDialogueFinished()
    {
        dialogueText.maxVisibleCharacters = currLines[currLineIndex].Length;
        dialogueArrowRT.gameObject.SetActive(true);
        arrowSequence.Play();
        isPlaying = false;
    }
}
