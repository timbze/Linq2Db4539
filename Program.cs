using Linq2Db4539;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

var ms = new MappingSchema();
TenderId.LinqToDbMapping(ms);

// get connection string from .env file
var env = File.ReadAllLines(".env");
var connectionString = env[0].Substring("connection=".Length);

var db = new DataConnection(
    new DataOptions().UseMappingSchema(ms)
        .UsePostgreSQL(connectionString));

var tenderIdsGuid = new List<Guid> {Guid.NewGuid(), Guid.NewGuid()};
await db.GetTable<Tender>().Where(i => tenderIdsGuid.Contains(i.Id.Value)).AnyAsync();
