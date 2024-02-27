using UnityEngine;

namespace UI
{
    public class HologramCanvas : MonoBehaviour
    {
        void Start()
        {
            Camera eventCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Canvas canvas = GetComponent<Canvas>();
            canvas.worldCamera = eventCamera;
        }
    }
}