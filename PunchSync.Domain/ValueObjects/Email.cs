using System.Text.RegularExpressions;

namespace PunchSync.Domain.ValueObjects;

public sealed partial class Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("E-mail não pode ser vazio.", nameof(value));

        value = value.Trim().ToLowerInvariant();

        if (!EmailRegex().IsMatch(value))
            throw new ArgumentException("E-mail em formato inválido.", nameof(value));

        return new Email(value);
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
