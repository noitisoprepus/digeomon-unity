using UnityEngine;

public class KeepScaling : MonoBehaviour
{
    [SerializeField] private bool active = false;
    [SerializeField] private float scaleSpeed = 2.0f;
    [SerializeField] private float scaleVariation = 0.5f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (active)
        {
            float scale = Mathf.Sin(Time.time * scaleSpeed) * scaleVariation + 1;
            transform.localScale = originalScale * scale;
        }
    }
}
