using Core;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadFTUE()
        {
            GameManager.Instance.GoToScene("FTUE");
        }
    }
}