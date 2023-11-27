using UnityEngine;

public class FTUE : MonoBehaviour
{
    private DialogueManager dialogueManager;

    private void Awake()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }

    private void Start()
    {
        dialogueManager.onDialogueFinished.AddListener(OnFinished);
    }

    private void OnFinished()
    {
        GameManager.Instance.GoToScene("Scanner");
    }
}
