using Linq2Db4539;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

var ms = new MappingSchema();
TenderId.LinqToDbMapping(ms);

// get connection string from .env file
var env = File.ReadAllLines(".env");
var connectionString = env[0].Substring("connection=".Length);

DataConnection.TurnTraceSwitchOn();
DataConnection.WriteTraceLine = (s1,s2,_) => Console.WriteLine(s1, s2);

var db = new DataConnection(
    new DataOptions().UseMappingSchema(ms)
        .UsePostgreSQL(connectionString));

// issue #4539 (works!)
// var tenderIdsGuid = new List<TenderId> {TenderId.From(Guid.NewGuid()), TenderId.From(Guid.NewGuid())};
// await db.GetTable<Tender>().Where(i => tenderIdsGuid.Contains(i.Id)).AnyAsync();

// // different issue w/ implicitly converted type trying to convert to SQL (works!)
// TenderId? tenderId = new TenderId {Value = Guid.NewGuid()};
// await db.GetTable<Tender>().Where(i => tenderId != null && i.Id == tenderId.Value).AnyAsync();

// order by bool (problem)
var offlineBool = false;
await db.GetTable<Tender>()
    .OrderBy(i => offlineBool && i.Name.Length > 1)
    .FirstOrDefaultAsync();
