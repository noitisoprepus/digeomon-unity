using UnityEngine;

namespace Core
{
    public class FTUEManager : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private DialogueData ftueDialogue;

        private void OnEnable()
        {
            //Debug.Log("FTUE: " + key);
            if (PlayerPrefs.HasKey(key)) return;

            dialogueManager.StartDialogue(ftueDialogue);
            PlayerPrefs.SetInt(key, 1);
        }
    }
}