using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Core
{
    public class JournalManager : MonoBehaviour
    {
        public delegate void FetchSuccessDelegate();
        public static event FetchSuccessDelegate OnFetchSuccessAction;

        public delegate void CaptureSuccessDelegate();
        public static event CaptureSuccessDelegate OnCaptureSuccessAction;

        [SerializeField] DigeomonCaptureData digeomonCaptureData;

        private DatabaseReference databaseReference;

        public void InitializeCaptureData()
        {
            databaseReference = FirebaseDatabase.GetInstance(GameManager.Instance.databaseUri)
                .GetReference("users").Child(PlayerPrefs.GetString("userID")).Child("captureData");

            GetCaughtDigeomonData();
        }

        public void AddDigeomon(string digeomonName)
        {
            databaseReference.Push().SetValueAsync(digeomonName).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("An error has occurred: " + task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    OnCaptureSuccessAction?.Invoke();
                }
            });
        }

        private void GetCaughtDigeomonData()
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
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            digeomonCaptureData.SyncDigeomonData(childSnapshot.Value.ToString());
                        }
                    }
                    OnFetchSuccessAction?.Invoke();
                }
            });
        }
    }
}