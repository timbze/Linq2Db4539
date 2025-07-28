using System.Linq.Expressions;
using LinqToDB;
using LinqToDB.Async;
using LinqToDB.Data;
using LinqToDB.Mapping;

[Table("test_person")]
public sealed class Person
{
    [Column("id"), PrimaryKey, Identity]
    public int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    [Column("phone_id")]
    public int PhoneId { get; set; }
    
    [Association(QueryExpressionMethod = nameof(MapExpression))]
    public List<MapWithInfo> MapLocations { get; set; } = new();
    
    public static Expression<Func<Person, IDataContext, IQueryable<MapWithInfo>>> MapExpression =>
        (p, db) => from m in db.GetTable<Map>()
            where m.PersonId == p.Id
            select new MapWithInfo { Map = m };
}

public sealed class Phone
{
    [Column("id"), PrimaryKey, Identity]
    public int Id { get; set; }

    [Column("number")]
    public required string Number { get; set; }
}

public sealed class MapWithInfo
{
    public Map Map { get; set; }
}

[Table("test_map")]
public sealed class Map
{
    [Column("person_id")]
    public int PersonId { get; set; }

    [Column("location")]
    public required string Location { get; set; }

    [Column("map_info_id")]
    public int MapInfoId { get; set; }
}

public class Run5052
{
    public static async Task Main(DataConnection db)
    {
        // Data
        await db.DropTableAsync<Person>(throwExceptionIfNotExists: false);
        await db.DropTableAsync<Map>(throwExceptionIfNotExists: false);
        await db.DropTableAsync<Phone>(throwExceptionIfNotExists: false);
        await db.CreateTableAsync<Person>();
        await db.CreateTableAsync<Map>();
        await db.CreateTableAsync<Phone>();

        var phoneId = await db.InsertWithInt32IdentityAsync(new Phone { Number = "1234567890" });
        var id = await db.InsertWithInt32IdentityAsync(new Person { Name = "John", PhoneId = phoneId });

        await db.InsertAsync(new Map { PersonId = id, Location = "Here" });
        await db.InsertAsync(new Map { PersonId = id, Location = "There" });

        // Query
        var result = await (from p in db.GetTable<Person>()
            from ph in db.GetTable<Phone>().LeftJoin(i => i.Id == p.PhoneId)
            select p)
            .LoadWith(i => i.MapLocations)
            .ToListAsync();
        
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));
    }
}