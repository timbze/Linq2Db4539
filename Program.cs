using Linq2Db4539;
using LinqToDB;
using LinqToDB.Async;
using LinqToDB.Data;
using LinqToDB.Mapping;

var ms = new MappingSchema();
TenderId.LinqToDbMapping(ms);

// get connection string from .env file
var env = File.ReadAllLines(".env");
var connectionString = env[0].Substring("connection=".Length);

DataConnection.TurnTraceSwitchOn();
DataConnection.WriteTraceLine = (s1, s2, _) => Console.WriteLine($"{s1}{s2}");

var db = new DataConnection(
    new DataOptions()
        .UseMappingSchema(ms)
        .UsePostgreSQL(connectionString));

// works in 5.4.1.9 but not in 6.1.0
await db
    .GetTable<Tender>()
    .Select(i => new Output { Id = (Guid) i.Id })
    .FirstOrDefaultAsync();

class Output
{
    public ShortId Id { get; set; }
}

// works both 5.4.1 and 6.0.0-rc.2 (comparing Guid to TenderId)
// var tenderIds = new List<TenderId> {(TenderId) Guid.NewGuid(), (TenderId) Guid.NewGuid()};
// var tender = await db
//     .GetTable<Tender>()
//     .Where(i => tenderIds.Any(id => id == i.Id))
//     .FirstOrDefaultAsync();

// generates sql in 5.4.1 but not fails in 6.0.0-rc.2 (Comparing TenderId to TenderId) (both versions crash but for different reasons)
// List<TenderId> tenderIds2 = new List<TenderId> {TenderId.From(Guid.NewGuid()), TenderId.From(Guid.NewGuid())};
// var tender2 = await db
//     .GetTable<Tender>()
//     .Where(i => tenderIds2.Any(id => id == i.Id))
//     .FirstOrDefaultAsync();
