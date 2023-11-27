using UnityEngine;

public class Sandbox : MonoBehaviour
{
    public Transform digeomonModelT;

    private void Start()
    {
        Digeomon digeomon = PersistentData.targetDigeomon;
        Instantiate(digeomon.modelPrefab, digeomonModelT);
    }
}
