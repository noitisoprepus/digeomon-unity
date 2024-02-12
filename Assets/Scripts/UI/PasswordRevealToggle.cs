using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class PasswordRevealToggle : MonoBehaviour
{
    private TMP_InputField inputField;
    private bool toggle = false;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void TogglePassword()
    {
        toggle = !toggle;
        inputField.contentType = toggle ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        inputField.ForceLabelUpdate();
    }
}
