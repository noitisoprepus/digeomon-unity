using UnityEngine;

public class KeepLookingAtScreen : MonoBehaviour
{
    void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    }
}
