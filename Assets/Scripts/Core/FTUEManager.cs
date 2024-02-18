using UnityEngine;

namespace Core
{
    public class FTUEManager : MonoBehaviour
    {
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private DialogueData ftueDialogue;

        private void Start()
        {
            // Check if FTUE is true
            dialogueManager.StartDialogue(ftueDialogue);
        }
    }
}