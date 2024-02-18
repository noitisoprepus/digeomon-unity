using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class JournalManager : MonoBehaviour
    {
        GameManager gameManager;
        DatabaseReference databaseReference;

        public Dictionary<DigeomonData, bool> caughtDigeomons;

        private void Start()
        {
            gameManager = GameManager.Instance;

            databaseReference = FirebaseDatabase.GetInstance(gameManager.databaseUri)
                .GetReference("users").Child(gameManager.userID).Child("captureData");

            GetCaughtDigeomonData();
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
                        // Convert the snapshot to a List<string>
                        List<string> captureData = snapshot.Children
                            .Select(childSnapshot => childSnapshot.Value.ToString())
                            .ToList();

                        // Use the captureData list as needed
                        foreach (string capture in captureData) Debug.Log(capture); 
                    }
                    else
                    {
                        Debug.LogError("Snapshot does not exist or has no children.");
                    }

                    //caughtDigeomons = new Dictionary<DigeomonData, bool>();
                    //List<DigeomonData> availableDigeomons = gameManager.GetDigeomonList();
                    //foreach (DigeomonData digeomon in availableDigeomons)
                    //{
                    //    if (captureData.Contains(digeomon.name))
                    //    {
                    //        caughtDigeomons.Add(digeomon, true);
                    //        continue;
                    //    }
                    //    caughtDigeomons.Add(digeomon, false);
                    //}
                }
            });
        }

        public void TestCapture()
        {
            // Testing
            List<string> testCapture = new List<string>
            {
                "Tableh"
            };
            databaseReference.SetValueAsync(testCapture).ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("An error has occurred: " + task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Test Capture Success");
                }
            });
        }
    }
}