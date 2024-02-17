using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public string username;
    public CaptureData captureData;

    public UserData(string username, List<DigeomonData> digeomonList)
    {
        this.username = username;
        this.captureData = new CaptureData();
        foreach (DigeomonData digeomon in digeomonList)
        {
            captureData.entries.Add(new CaptureEntry(digeomon.name, false));
        }
    }
}
