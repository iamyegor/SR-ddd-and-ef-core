namespace App;

public abstract class Entity
{
    public long Id { get; }

    protected Entity() { }

    protected Entity(long id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }
        else if (GetRealType() != other.GetRealType())
        {
            return false;
        }
        else if (Id == 0 || other.Id == 0)
        {
            return false;
        }

        return Id == other.Id;
    }

    private Type? GetRealType()
    {
        Type type = GetType();

        if (type.ToString().Contains("Castle.Proxies."))
        {
            return type.BaseType;
        }
        else
        {
            return type;
        }
    }

    public static bool operator ==(Entity? a, Entity? b)
    {
        return Equals(a, b);
    }

    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
