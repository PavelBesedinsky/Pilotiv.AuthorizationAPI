using System.Reflection;
using Pilotiv.DbMigrator.Postgres;

return (int) await MigratorManager.RunMigratorAsync(Assembly.GetExecutingAssembly(), args);
