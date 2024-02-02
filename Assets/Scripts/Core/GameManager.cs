using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private DigeomonList digeomonData;

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
        }

        public void GoToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public List<DigeomonData> GetDigeomonList()
        {
            return digeomonData.digeomons;
        }
    }
}