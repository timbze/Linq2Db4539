namespace Linq2Db4539;

/// <summary>
/// Not a Guid, but a shorter base64 representation minus the final 2 '=' signs (22 chars total).
/// This type is converted to string or Guid implicitly, or you can get the ShortValue or GuidValue property.
/// </summary>
/// <remarks>https://www.madskristensen.net/blog/A-shorter-and-URL-friendly-GUID</remarks>
public readonly struct ShortId : IEquatable<ShortId>
{
    private ShortId(Guid value)
    {
        ShortValue = Encode(value);
        GuidValue = value;
    }
    private ShortId(string value)
    {
        ShortValue = value;
        GuidValue = Decode(value);
    }

    public string ShortValue { get; }
    public Guid GuidValue { get; }

    public static implicit operator string(ShortId shortId) => shortId.ShortValue;
    public static implicit operator Guid(ShortId shortId) => shortId.GuidValue;
    public static implicit operator ShortId(Guid guid) => new (guid);
    public static implicit operator ShortId(string shortGuid) => new (shortGuid);

    private static Guid Decode(string encoded)
    {
        var work = encoded.Replace("_", "/");
        work = work.Replace("-", "+");
        try
        {
            byte[] buffer = Convert.FromBase64String(work + "==");
            return new Guid(buffer);
        }
        catch (Exception e) when (e is ArgumentException or ArgumentNullException or FormatException)
        {
            throw new ArgumentException($"The Id supplied ('{encoded}') is not valid", e);
        }
    }

    private static string Encode(Guid guid)
    {
        string enc = Convert.ToBase64String(guid.ToByteArray());
        enc = enc.Replace("/", "_");
        enc = enc.Replace("+", "-");
        return enc[..22];
    }

    /// <summary>
    /// Try to parse string into ShortId. For FastEndpoints model binding
    /// </summary>
    /// <param name="input">The string to parse</param>
    /// <param name="output">The parsed string in ShortId format</param>
    /// <returns>True if parsing succeeded</returns>
    public static bool TryParse(string? input, out ShortId output)
    {
        if (string.IsNullOrEmpty(input))
        {
            output = default;
            // input null = fine, but blank should return false
            return input is null;
        }

        try
        {
            output = new ShortId(input);
            return true;
        }
        catch (ArgumentException)
        {
            output = default;
            return false;
        }
    }

    public override string ToString() => ShortValue;

    public bool Equals(ShortId other)
    {
        return ShortValue == other.ShortValue && GuidValue.Equals(other.GuidValue);
    }

    public override bool Equals(object? obj)
    {
        return obj is ShortId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GuidValue);
    }

    public static bool operator ==(ShortId left, ShortId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShortId left, ShortId right)
    {
        return !(left == right);
    }

    public static bool operator ==(ShortId? left, ShortId? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(ShortId? left, ShortId? right)
    {
        return !(left == right);
    }
}