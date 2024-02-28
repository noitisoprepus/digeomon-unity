using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LeaderboardsUI : MonoBehaviour
    {
        [Header("GUI")]
        [SerializeField] private GameObject leaderboardsContent;
        [SerializeField] private GameObject entryBox;

        public void PopulateLeaderboards(List<KeyValuePair<string, int>> sortedUsersCaptureData)
        {
            if (leaderboardsContent.transform.childCount == 0)
            {
                int rank = 1;
                foreach (KeyValuePair<string, int> user in sortedUsersCaptureData)
                {
                    GameObject entry = Instantiate(entryBox, leaderboardsContent.transform);
                    LeaderboardsEntry leaderboardsEntry = entry.GetComponent<LeaderboardsEntry>();
                    leaderboardsEntry.InitializeLeaderboardsEntry(rank, user.Key, user.Value);
                    rank++;
                }
            }
        }

        public void ResetLeaderboards()
        {
            foreach (Transform entry in leaderboardsContent.transform)
                Destroy(entry.gameObject);
            leaderboardsContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}