using XResults;

namespace App;

public class Name : ValueObject
{
    public string First { get; }
    public string Last { get; }
    public virtual Suffix Suffix { get; }

    protected Name() { }

    private Name(string first, string last, Suffix suffix)
        : this()
    {
        First = first;
        Last = last;
        Suffix = suffix;
    }

    public static Result<Name> Create(string firstName, string lastName, Suffix suffix)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Fail("First name should not be empty");
        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Fail("Last name should not be empty");

        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (firstName.Length > 200)
            return Result.Fail("First name is too long");
        if (lastName.Length > 200)
            return Result.Fail("Last name is too long");

        return new Name(firstName, lastName, suffix);
    }

    protected override IEnumerable<object> GetPropertiesForComparison()
    {
        yield return First;
        yield return Last;
        yield return Suffix;
    }
}
