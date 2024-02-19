using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public string databaseUri { get; } = "https://digeomon-default-rtdb.asia-southeast1.firebasedatabase.app/";
        public string userID { get; set; } = null;

        private DigeomonList digeomonData;
        private JournalManager journalManager;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            digeomonData = GetComponent<DigeomonList>();
            journalManager = GetComponent<JournalManager>();
        }

        public void GoToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public List<DigeomonData> GetDigeomonList()
        {
            return digeomonData.digeomons;
        }

        public void SyncCaptureData()
        {
            journalManager.InitializeCaptureData();
        }

        public List<string> GetDigeomonCaptures()
        {
            return journalManager.capturedDigeomons;
        }

        public void CaptureDigeomon(DigeomonData digeomon)
        {
            journalManager.AddDigeomon(digeomon.name);
        }
    }
}