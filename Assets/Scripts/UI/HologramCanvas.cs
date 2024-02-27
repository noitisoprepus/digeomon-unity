using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class HologramCanvas : MonoBehaviour
    {
        private void Start()
        {
            if (SceneManager.GetActiveScene().name.Equals("Sandbox"))
            {
                gameObject.SetActive(false);
                return;
            }

            Camera eventCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Canvas canvas = GetComponent<Canvas>();
            canvas.worldCamera = eventCamera;
        }
    }
}