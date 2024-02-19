using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class JournalManager : MonoBehaviour
    {
        public List<string> capturedDigeomons { get; set; }

        private GameManager gameManager;
        private DatabaseReference databaseReference;

        private void Start()
        {
            gameManager = GameManager.Instance;

            InitializeCaptureData();
        }

        public void InitializeCaptureData()
        {
            if (gameManager.userID == null)
                return;

            if (databaseReference == null)
            {
                databaseReference = FirebaseDatabase.GetInstance(gameManager.databaseUri)
                    .GetReference("users").Child(gameManager.userID).Child("captureData");

                GetCaughtDigeomonData();
            }
        }

        public void AddDigeomon(string newDigeomon)
        {
            capturedDigeomons.Add(newDigeomon);
            databaseReference.SetValueAsync(newDigeomon).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("An error has occurred: " + task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    // Upload success
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
                        capturedDigeomons = new List<string>();
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            capturedDigeomons.Add(childSnapshot.Value.ToString());
                        }
                    }
                    else
                    {
                        Debug.LogError("'captureData' does not exist or has no children.");
                    }
                }
            });
        }
    }
}