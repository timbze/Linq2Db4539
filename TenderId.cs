namespace Linq2Db4539;

public struct TenderId
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
    
    // public static implicit operator string(TenderId tenderId) => tenderId.Value.ToString();
    
    internal static void LinqToDbMapping(LinqToDB.Mapping.MappingSchema ms)
    {
        ms.SetConverter<TenderId, Guid>(id => id.Value);
        ms.SetConverter<TenderId, Guid?>(id => id.Value);
        ms.SetConverter<TenderId?, Guid>(id => id?.Value ?? default);
        ms.SetConverter<TenderId?, Guid?>(id => id?.Value);
        ms.SetConverter<Guid, TenderId>(From);
        ms.SetConverter<Guid, TenderId?>(g => From(g));
        ms.SetConverter<Guid?, TenderId>(g => g == null ? default : From((Guid) g));
        ms.SetConverter<Guid?, TenderId?>(From);

        ms.SetConverter<TenderId, LinqToDB.Data.DataParameter>(id => new LinqToDB.Data.DataParameter {DataType = LinqToDB.DataType.Guid, Value = id.Value});
        ms.SetConverter<TenderId?, LinqToDB.Data.DataParameter>(id => new LinqToDB.Data.DataParameter {DataType = LinqToDB.DataType.Guid, Value = id?.Value});
    }
}