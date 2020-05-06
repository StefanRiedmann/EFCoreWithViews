using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBasic
{
    public class GardenContext : DbContext
    {
        public DbSet<Garden> Gardens { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<CropAssignment> CropAssignments { get; set; }
        public DbSet<GardenInfo> GardenInfos { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=CICLO-1\\CICLODB;Initial Catalog=GardenDb1;User ID=sa;Password=adminsql1");
            // => options.UseSqlServer("Server=localhost,1433; Database=GardenDb1;User=sa; Password=1StrongPassword!");
            //

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GardenInfo>(d =>
            {
                d.HasKey("GardenId", "Year");
                d.ToView("View_GardenInfos");
                d.HasOne("Garden").WithMany("GardenInfos");
            });
        } 
    }

    public class Garden
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GardenId { get; set; }
        public string Name { get; set; }

        public List<Bed> Beds { get; } = new List<Bed>();

        public List<GardenInfo> GardenInfos { get; set; }
    }

    public class Bed
    {
        public int BedId { get; set; }
        public int Number { get; set; }

        public int GardenId { get; set; }
        public Garden Garden { get; set; }
        public List<CropAssignment> CropAssignments { get; set; }
    }

    public class Crop
    {
        public int CropId { get; set; }
        public string Name  { get; set; }
        public List<CropAssignment> CropAssignments { get; set; }
    }

    public class CropAssignment
    {
        public int CropAssignmentId { get; set; }
        public int CropId { get; set; }
        public Crop Crop { get; set; }
        public int BedId { get; set; }
        public Bed Bed { get; set; }
        public int Year { get; set; }
    }

    public class GardenInfo
    {
        public int GardenId { get; set; }
        public Garden Garden { get; set; }
        public int Year { get; set; }
        
        /// <summary>
        /// A string with all the crops that are planned for 
        /// this garden in the year, connected with ' | '
        /// </summary>
        public string AllGardenCrops { get; set; }
    }
}