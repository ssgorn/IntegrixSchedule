using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrixSchedule.Model;

namespace IntegrixSchedule.BisnessObjects
{
	public interface IDBManager
	{
		void SetNewDBContext(IMainDBContext context);

		ObservableCollection<Organization> OrganizationsGet(bool force = false);

		ObservableCollection<ScheduleHeader> ScheduleHeadersGet(Guid? idOrganization);

		ScheduleDetailsCollection ScheduleDetailsGet(Guid? idHeader);

		void SaveScheduleHeader(ScheduleHeader hed);

		void SaveScheduleDetails(List<ScheduleDetail> dets);

		ScheduleTemplate GetActualScheduleTemplate(Guid idOrganization);

		ObservableCollection<ScheduleTemplate> SceduleTemplatesGet(Guid idOrganization);

		ScheduleTemplate DeleteSelectedTemplate(ScheduleTemplate templ);

		void SaveSelectedTemplate(ScheduleTemplate templ);
	}

	public class DBManager : IDBManager
	{
		public static IDBManager Instance = new DBManager();

		private IMainDBContext _dbContext;
		private IMainDBContext DbContext
		{
			get
			{
				if (_dbContext == null)
				{
					_dbContext = new MainDBContext();
					_dbContext.SaveChanges();
				}

				return _dbContext;
			}
		}

		public void SetNewDBContext(IMainDBContext context)
		{
			_dbContext = context;
		}

		public ObservableCollection<Organization> OrganizationsGet(bool force = false)
		{
			return new ObservableCollection<Organization>(DbContext.Organizations.ToList());
		}

		public ObservableCollection<ScheduleHeader> ScheduleHeadersGet(Guid? idOrganization)
		{
			if (idOrganization == null)
				return new ObservableCollection<ScheduleHeader>();

			return new ObservableCollection<ScheduleHeader>((from a in DbContext.ScheduleHeaders
				where a.IdOrganization == idOrganization.Value
				select a).ToList());
		}

		public ScheduleDetailsCollection ScheduleDetailsGet(Guid? idHeader)
		{
			if (idHeader == null)
				return new ScheduleDetailsCollection();

			return new ScheduleDetailsCollection((from a in DbContext.ScheduleDetails
					where a.IdScheduleHeader == idHeader.Value
					select a).ToList(), idHeader.Value);
		}

		public void SaveScheduleHeader(ScheduleHeader hed)
		{
			if (!DbContext.ScheduleHeaders.Any(x => x.Id == hed.Id))
			{
				DbContext.ScheduleHeaders.Add(hed);
				DbContext.SaveChanges();
			}
		}

		public void SaveScheduleDetails(List<ScheduleDetail> dets)
		{
			foreach (var det in dets)
			{
				if (!DbContext.ScheduleDetails.Any(x => x.Id == det.Id))
					DbContext.ScheduleDetails.Add(det);
			}
			DbContext.SaveChanges();
		}

		public ScheduleTemplate GetActualScheduleTemplate(Guid idOrganization)
		{
			return DbContext.SceduleTemplates.FirstOrDefault(x => x.IdOrganization == idOrganization && x.IsActual);
		}

		public ObservableCollection<ScheduleTemplate> SceduleTemplatesGet(Guid idOrganization)
		{
			return new ObservableCollection<ScheduleTemplate>((from a in DbContext.SceduleTemplates
															 where a.IdOrganization == idOrganization
															 select a).ToList());
		}

		public ScheduleTemplate DeleteSelectedTemplate(ScheduleTemplate templ)
		{
			if (templ != null)
			{
				var item = DbContext.SceduleTemplates.FirstOrDefault(x => x.Id == templ.Id);
				if (item != null)
				{
					DbContext.SceduleTemplates.Remove(item);
					DbContext.SaveChanges();
				}
			}
			return templ;
		}

		public void SaveSelectedTemplate(ScheduleTemplate templ)
		{
			if (templ != null)
			{
				var item = DbContext.SceduleTemplates.FirstOrDefault(x => x.Id == templ.Id);
				if (item == null)
					DbContext.SceduleTemplates.Add(templ);
				else
					item.Update(templ);

				DbContext.SaveChanges();
			}
		}
	}
}
