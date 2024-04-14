public class StatusEffect
{
    public string Name
    {
        get;
        protected set;
    }

    public float MaxDuration
    {
        get;
        protected set;
    }

    public float Duration
    {
        get;
        protected set;
    }

    public StatusEffect(string name, float duration)
    {
        Name = name;
        MaxDuration = duration;
        Duration = duration;
    }

    public bool Update(float deltaTime)
    {
        Duration -= deltaTime;
        if (Duration <= 0)
        {
            return false;
        }
        return true;
    }
}