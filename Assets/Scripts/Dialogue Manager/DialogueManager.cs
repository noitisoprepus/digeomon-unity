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
    private int currLineIndex = 0;
    private bool isRunning = false;

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
        if (dialogues.Count.Equals(0)) return;
        isRunning = true;
        dialoguePanel.SetActive(true);

        currDialogue = dialogues[0];
        RunDialogue(currDialogue);
        dialogues.RemoveAt(0);
    }

    public void NextDialogue()
    {
        if (currLineIndex.Equals(currLines.Count - 1))
        {
            isRunning = false;
            dialoguePanel.SetActive(false);
            return;
        }

        currLineIndex++;
        StartCoroutine(PlayDialogue(currLines[currLineIndex]));
    }

    private void RunDialogue(Dialogue dialogue)
    {
        charNameText.SetText(dialogue.charName);

        currLines = new List<string>(dialogue.dialogues);
        StartCoroutine(PlayDialogue(currLines[0]));
        currLineIndex = 0;
    }

    private IEnumerator PlayDialogue(string line)
    {
        dialogueText.SetText(line);
        dialogueText.maxVisibleCharacters = 0;

        while (dialogueText.maxVisibleCharacters != line.Length)
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(textSpeed);
        }

        
    }
}
