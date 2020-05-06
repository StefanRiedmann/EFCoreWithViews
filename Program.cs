using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCoreBasic
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new GardenContext())
            {
                //to create the database in the first place
                if(args.Length > 0 && args[0] == "Create")
                {
                    db.Database.EnsureCreated();
                }
                //to reset the database
                else if(args.Length > 0 && args[0] == "Delete")
                {
                    db.Database.EnsureDeleted();
                    return;
                }
                else
                {
                    // updating migrations for tables...
                    db.Database.Migrate();
                    // ... and views
                    CreateViews(db.Database);
                }
                if(args.Length > 0 && args[0] == "CreateSampleData")
                {
                    SampleData.CreateSampleData(db);
                }
                else
                {
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Getting all the crops for this year...");
                    SampleData.ReadCrops(db, 2020);
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Getting garden plans for 2020 and 2021...");
                    SampleData.ReadGardenInfos(db, 2020, 2021);
                    Console.WriteLine("------------------------");
                }
            }
        }

        private static void CreateViews(DatabaseFacade db)
        {
            InjectView(db, "GardenInfo.sql", "View_GardenInfos");
        }

        private static void InjectView(DatabaseFacade db, string sqlFileName, string viewName)
        {
            var assembly = typeof(Program).Assembly;
            var assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));
            var resource = assembly.GetManifestResourceStream($"{assemblyName}.{sqlFileName}");
            var sqlQuery = new StreamReader(resource).ReadToEnd();
            //we always delete the old view, in case the sql query has changed
            db.ExecuteSqlRaw($"IF OBJECT_ID('{viewName}') IS NOT NULL BEGIN DROP VIEW {viewName} END");
            //creating a view based on the sql query
            db.ExecuteSqlRaw($"CREATE VIEW {viewName} AS {sqlQuery}");
        }
    }
}
