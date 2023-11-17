using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Data")]
    public List<Dialogue> dialogues;
    public float textSpeed = 0.2f;

    [Header("Dialogue GUI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI charNameText;
    public TextMeshProUGUI dialogueText;

    private Coroutine dialogueCoroutine;
    private Dialogue currDialogue;
    private List<string> currLines;
    private int currDialogueIndex = 0;
    private int currLineIndex = 0;
    private bool isRunning = false;
    private bool isPlaying = false;

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
        dialoguePanel.SetActive(true);

        currDialogue = dialogues[currDialogueIndex];
        RunDialogue(currDialogue);
        currDialogueIndex++;
    }

    public void NextDialogue()
    {
        if (isPlaying)
        {
            isPlaying = false;
            StopCoroutine(dialogueCoroutine);
            dialogueText.maxVisibleCharacters = currLines[currLineIndex].Length;
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
        isPlaying = true;
        dialogueText.SetText(line);
        dialogueText.maxVisibleCharacters = 0;

        while (dialogueText.maxVisibleCharacters != line.Length)
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(textSpeed);
        }
        isPlaying = false;
    }
}
