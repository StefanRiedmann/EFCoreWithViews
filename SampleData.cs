using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace EFCoreBasic
{
    public static class SampleData
    {
        public static void ReadCrops(GardenContext db, int year)
        {
            var assignments = db.CropAssignments.AsNoTracking()
                        .Where(ca => ca.Year == year)
                        .Include(ca => ca.Bed).ThenInclude(b => b.Garden)
                        .Include(ca => ca.Crop)
                        .OrderBy(ca => ca.Bed.Garden.Name).ThenBy(ca => ca.Crop.Name).ThenBy(ca => ca.Bed.Number)
                        .ToList();
            assignments.ForEach(a => Console.WriteLine($"Growing {a.Crop.Name} on bed No {a.Bed.Number} in '{a.Bed.Garden.Name}' ({year})"));
        }

        public static void ReadGardenInfos(GardenContext db, params int[] years)
        {
            var gardenInfos = db.GardenInfos.AsNoTracking()
                        .Include(gi => gi.Garden)
                        .Where(gi => years.Contains(gi.Year))
                        .OrderBy(gi => gi.Garden.Name).ThenBy(gi => gi.Year)
                        .ToList();
            gardenInfos.ForEach(info => Console.WriteLine($"Crops in '{info.Garden.Name}' ({info.Year}): {info.AllGardenCrops}"));
        }

        public static void CreateSampleData(GardenContext db)
        {
            var garden = (db.Gardens.Add(new Garden { Name = "My first garden" })).Entity;
            db.SaveChanges();

            var bed1 = (db.Beds.Add(new Bed{ GardenId = garden.GardenId, Number = 1 })).Entity;
            db.SaveChanges();
            var bed2 = (db.Beds.Add(new Bed{ GardenId = garden.GardenId, Number = 2 })).Entity;
            db.SaveChanges();
            var bed3 = (db.Beds.Add(new Bed{ GardenId = garden.GardenId, Number = 3 })).Entity;
            db.SaveChanges();

            var pumpkin = (db.Crops.Add(new Crop{Name = "Pumpkin"})).Entity;
            db.SaveChanges();
            var salad = (db.Crops.Add(new Crop{Name = "Salad"})).Entity;
            db.SaveChanges();
            var tomatoes = (db.Crops.Add(new Crop{Name = "Tomatoes"})).Entity;
            db.SaveChanges();
            var beans = (db.Crops.Add(new Crop{Name = "Beans"})).Entity;
            db.SaveChanges();

            var assignment1 = (db.CropAssignments.Add(
                new CropAssignment{
                    CropId = pumpkin.CropId,
                    BedId = bed1.BedId,
                    Year = 2020
                })).Entity;
            db.SaveChanges();
            var assignment2 = (db.CropAssignments.Add(
                new CropAssignment{
                    CropId = salad.CropId,
                    BedId = bed2.BedId,
                    Year = 2020
                })).Entity;
            db.SaveChanges();
            var assignment3 = (db.CropAssignments.Add(
                new CropAssignment{
                    CropId = beans.CropId,
                    BedId = bed3.BedId,
                    Year = 2020
                })).Entity;
            db.SaveChanges();
            var assignment4 = (db.CropAssignments.Add(
                new CropAssignment{
                    CropId = pumpkin.CropId,
                    BedId = bed3.BedId,
                    Year = 2021
                })).Entity;
            db.SaveChanges();
            var assignment5 = (db.CropAssignments.Add(
                new CropAssignment{
                    CropId = tomatoes.CropId,
                    BedId = bed2.BedId,
                    Year = 2021
                })).Entity;
            db.SaveChanges();
        }
    }
}