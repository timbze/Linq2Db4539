namespace Linq2Db4539;

public struct TenderId : IEquatable<TenderId>
{
    public Guid Value { get; set; }
    public static TenderId From(Guid value) => new TenderId {Value = value};
    public static TenderId? From(Guid? value) => value.HasValue ? new TenderId {Value = value.Value} : null;
    
    public static bool operator ==(TenderId a, TenderId b) => a.Value == b.Value;
    public static bool operator !=(TenderId a, TenderId b) => !(a == b);
    public static bool operator ==(TenderId a, Guid b) => a.Value == b;
    public static bool operator !=(TenderId a, Guid b) => !(a == b);
    public static bool operator ==(Guid a, TenderId b) => a == b.Value;
    public static bool operator !=(Guid a, TenderId b) => !(a == b);

    public static explicit operator TenderId(Guid value) => new TenderId {Value = value};
    public static explicit operator Guid(TenderId value) => value.Value;

    public bool Equals(TenderId other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => obj is TenderId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    
    internal static void LinqToDbMapping(LinqToDB.Mapping.MappingSchema ms)
    {
        ms.SetConverter<TenderId, Guid>(id => (Guid) id);
        ms.SetConverter<TenderId, Guid?>(id => (Guid?) id);
        ms.SetConverter<TenderId?, Guid>(id => (Guid?) id ?? default);
        ms.SetConverter<TenderId?, Guid?>(id => (Guid?) id);
        ms.SetConverter<Guid, TenderId>(From);
        ms.SetConverter<Guid, TenderId?>(g => From(g));
        ms.SetConverter<Guid?, TenderId>(g => g == null ? default : From((Guid) g));
        ms.SetConverter<Guid?, TenderId?>(From);

        ms.SetConverter<TenderId, LinqToDB.Data.DataParameter>(id => new LinqToDB.Data.DataParameter {DataType = LinqToDB.DataType.Guid, Value = (Guid) id});
        ms.SetConverter<TenderId?, LinqToDB.Data.DataParameter>(id => new LinqToDB.Data.DataParameter {DataType = LinqToDB.DataType.Guid, Value = (Guid?) id});

        ms.AddScalarType(typeof(TenderId), LinqToDB.DataType.Guid);
    }
}