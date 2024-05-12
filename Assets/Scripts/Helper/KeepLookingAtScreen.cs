using UnityEngine;

namespace Helper
{
    public class KeepLookingAtScreen : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}