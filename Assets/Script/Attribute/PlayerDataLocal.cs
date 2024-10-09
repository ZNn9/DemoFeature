[System.Serializable]
public class PlayerDataLocal
{
    private int health = 200;
    private int shield = 200;
    private int attack = 20;
    private int defense = 20;
    private int accuracy = 10;

    public int Health
    {
        get { return health;}
        set { health = value;}
    }
    public int Shield
    {
        get { return shield; }
        set { shield = value; }
    }

    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public int Accuracy
    {
        get { return accuracy; }
        set { accuracy = value; }
    }
}