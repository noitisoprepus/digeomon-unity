using UnityEngine;

namespace Helper
{
    public class KeepLookingAtScreen : MonoBehaviour
    {
        Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            transform.LookAt(mainCamera.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}