using UnityEngine;

public class KeepRotating : MonoBehaviour
{
    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;
    [SerializeField] private float rotationRate;

    
    private void Update()
    {
        if (x || y || z)
        {
            transform.Rotate(x ? rotationRate * Time.deltaTime: 0f, y ? rotationRate * Time.deltaTime : 0f, z ? rotationRate * Time.deltaTime : 0f);
        }
    }
}
