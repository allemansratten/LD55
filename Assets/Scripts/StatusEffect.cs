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

// A value that can be modified and reset to its base value.
// Meant to be modified by status effects.
public class Modifiable<T>
{
    public T BaseValue
    {
        get;
        set;
    }

    public T Value
    {
        get;
        set;
    }

    public Modifiable(T baseValue)
    {
        BaseValue = baseValue;
        Value = baseValue;
    }

    public void Reset()
    {
        Value = BaseValue;
    }
}