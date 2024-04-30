using System.Text.RegularExpressions;
using XResults;

namespace App;

public class Email 
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail("Email should not be empty");

        email = email.Trim();

        if (email.Length > 200)
            return Result.Fail("Email is too long");

        if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
            return Result.Fail("Email is invalid");

        return new Email(email);
    }

    public static implicit operator string(Email email)
    {
        return email.Value;
    }
}
