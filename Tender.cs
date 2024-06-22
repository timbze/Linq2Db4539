using LinqToDB.Mapping;

namespace Linq2Db4539;

[Table("tender")]
public sealed class Tender
{
    [Column("id")]
    public TenderId Id { get; set; }
}