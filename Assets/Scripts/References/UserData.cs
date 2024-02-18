using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public string username;
    public List<string> captureData;

    public UserData(string username, List<string> captureData)
    {
        this.username = username;
        this.captureData = captureData;
    }
}
