using System.Collections.Generic;

[System.Serializable]
public class CaptureData
{
    public List<CaptureEntry> entries = new List<CaptureEntry>();
}

[System.Serializable]
public class CaptureEntry
{
    public string name;
    public bool captured;

    public CaptureEntry(string name, bool captured)
    {
        this.name = name;
        this.captured = captured;
    }
}
