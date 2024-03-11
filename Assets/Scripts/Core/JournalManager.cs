using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class JournalManager : MonoBehaviour
    {
        public delegate void SummonDelegate();
        public static event SummonDelegate OnSummonAction;

        public delegate void FetchSuccessDelegate();
        public static event FetchSuccessDelegate OnFetchSuccessAction;

        public delegate void CaptureSuccessDelegate();
        public static event CaptureSuccessDelegate OnCaptureSuccessAction;

        [SerializeField] DigeomonCaptureData digeomonCaptureData;

        private DatabaseReference databaseReference;

        public void InitializeCaptureData()
        {
            // For guest mode
            if (PlayerPrefs.GetInt("guest") == 1)
            {
                Dictionary<string, bool> captureData = new Dictionary<string, bool>(digeomonCaptureData.captureData);
                foreach (KeyValuePair<string, bool> data in captureData)
                {
                    if (PlayerPrefs.HasKey(data.Key))
                        digeomonCaptureData.captureData[data.Key] = true;
                }
                OnFetchSuccessAction?.Invoke();
                return;
            }

            databaseReference = FirebaseDatabase.GetInstance(GameManager.Instance.databaseUri)
                .GetReference("users").Child(PlayerPrefs.GetString("userID")).Child("captureData");

            GetCaughtDigeomonData();
        }

        public void AddDigeomon(string digeomonName)
        {
            if (PlayerPrefs.GetInt("guest") == 1)
            {
                OnCaptureSuccessAction?.Invoke();
                return;
            }

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

        public void SummonDigeomon(DigeomonData digeomonData)
        {
            PersistentData.targetDigeomon = digeomonData;
            PersistentData.toSummon = true;

            if (SceneManager.GetActiveScene().name.Equals("Main Menu"))
                GameManager.Instance.GoToScene("Scanner");
            else
                OnSummonAction?.Invoke();
        }

        public void EvolveDigeomon(DigeomonData digeomonData)
        {
            if (digeomonCaptureData.captureData[digeomonData.name])
            {
                GameManager.Instance.ShowDialog("The evolution for this \n digeomon has already been \ncaptured");
                return;
            }

            PersistentData.targetDigeomon = digeomonData;
            PersistentData.toSummon = false;
            PersistentData.toEvolve = true;
            GameManager.Instance.GoToScene("Sandbox");
        }
    }
}