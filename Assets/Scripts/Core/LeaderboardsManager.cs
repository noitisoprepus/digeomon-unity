using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class LeaderboardsManager : MonoBehaviour
    {
        public delegate void FetchSuccessDelegate(List<KeyValuePair<string, int>> userCaptureData);
        public static event FetchSuccessDelegate OnFetchSuccessAction;

        public delegate void FetchCancelDelegate();
        public static event FetchCancelDelegate OnFetchCancelAction;

        private DatabaseReference databaseReference;
        private Dictionary<string, int> userCaptureData;

        public void InitializeLeaderboardsData()
        {
            // For guest mode
            if (PlayerPrefs.GetInt("guest") == 1)
            {
                OnFetchCancelAction?.Invoke();
                return;
            }

            databaseReference = FirebaseDatabase
                .GetInstance(GameManager.Instance.databaseUri)
                .GetReference("users");
            
            GetLeaderboardsData();
        }

        private void GetLeaderboardsData()
        {
            databaseReference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("An error has occurred: " + task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists && snapshot.HasChildren)
                    {
                        userCaptureData = new Dictionary<string, int>();
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            string username = childSnapshot.Child("username").Value.ToString();
                            int captureCount = (int)childSnapshot.Child("captureData").ChildrenCount;
                            userCaptureData.Add(username, captureCount);
                        }
                        List<KeyValuePair<string, int>> sortedUsers = userCaptureData
                            .OrderByDescending(user => user.Value).ToList();
                        OnFetchSuccessAction?.Invoke(sortedUsers);
                    }
                }
            });
        }
    }
}