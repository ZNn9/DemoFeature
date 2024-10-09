
[System.Serializable]
public class SettingData
{
    private int volume;
    private bool isEdit;

    public int Volume
    {
        get { return volume;}
        set { volume = value;}
    }

    public bool IsEdit
    { 
        get { return isEdit;}
        set { isEdit = value;}
    }
}