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

// issue #4539
var tenderIdsGuid = new List<Guid> {Guid.NewGuid(), Guid.NewGuid()};
await db.GetTable<Tender>().Where(i => tenderIdsGuid.Contains(i.Id.Value)).AnyAsync();

// different issue w/ implicitly converted type trying to convert to SQL
TenderId? tenderId = new TenderId {Value = Guid.NewGuid()};
await db.GetTable<Tender>().Where(i => tenderId != null && i.Id == tenderId.Value.Value).AnyAsync();
