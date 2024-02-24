using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaderboardsEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI captureCountText;

        [Header("Top Three Tint")]
        [SerializeField] private Color rank1Color;
        [SerializeField] private Color rank2Color;
        [SerializeField] private Color rank3Color;

        public void InitializeLeaderboardsEntry(int rank, string username, int captureCount)
        {
            rankText.text = rank.ToString();
            usernameText.text = username;
            captureCountText.text = captureCount.ToString();

            switch (rank)
            {
                case 1:
                    GetComponent<Image>().color = rank1Color;
                    break;
                case 2:
                    GetComponent<Image>().color = rank2Color;
                    break;
                case 3:
                    GetComponent<Image>().color = rank3Color;
                    break;
                default:
                    break;
            }
        }
    }
}