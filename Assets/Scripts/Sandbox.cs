using UnityEngine;

public class Sandbox : MonoBehaviour
{
    public Transform digeomonModelT;

    private void Start()
    {
        DigeomonData digeomon = PersistentData.targetDigeomon;
        Instantiate(digeomon.modelPrefab, digeomonModelT);
    }
}
