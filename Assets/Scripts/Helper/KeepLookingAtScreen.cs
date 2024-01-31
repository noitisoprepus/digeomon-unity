using UnityEngine;

namespace Helper
{
    public class KeepLookingAtScreen : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}