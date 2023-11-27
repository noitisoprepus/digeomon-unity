using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadFTUE()
    {
        GameManager.Instance.GoToScene("FTUE");
    }
}
