using System.Windows.Documents;
using System;
using System.Data.Entity;
using System.Linq;

namespace IntegrixSchedule.Model
{
	public interface IMainDBContext
	{
		DbSet<Organization> Organizations { get; set; }

		DbSet<ScheduleTemplate> SceduleTemplates { get; set; }

		DbSet<ScheduleHeader> ScheduleHeaders { get; set; }

		DbSet<ScheduleDetail> ScheduleDetails { get; set; }

		int SaveChanges();
	}

	public class MainDBContext : DbContext, IMainDBContext
	{
		public MainDBContext()
			: base("name=MainModel")
		{
		}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		public virtual DbSet<Organization> Organizations { get; set; }

		public virtual DbSet<ScheduleTemplate> SceduleTemplates { get; set; }

		public virtual DbSet<ScheduleHeader> ScheduleHeaders { get; set; }

		public virtual DbSet<ScheduleDetail> ScheduleDetails { get; set; }
	}
}