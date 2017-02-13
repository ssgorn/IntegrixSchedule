using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntegrixSchedule.BisnessObjects;
using IntegrixSchedule.Model;

namespace IntegrixScheduleTests
{
	[TestClass]
	public class TestScheduleDetailsCollection
	{
		[TestMethod]
		public void TestEmptyConstructor()
		{
			var col = new ScheduleDetailsCollection();
			Assert.IsTrue(col.Count == 0);
		}

		[TestMethod]
		public void TestConstructorEmptyList()
		{
			var col = new ScheduleDetailsCollection(new List<ScheduleDetail>(), Guid.Empty);
			Assert.IsTrue(col.Count == 0);
		}

		[TestMethod]
		public void TestConstructorCorrectList()
		{
			var list = CreateCurrectMoqList();

			var col = new ScheduleDetailsCollection(list, list[0].IdScheduleHeader);
			Assert.IsTrue(col.Count == 3);
			Assert.IsTrue(col[0].ScheduleDetails.Count == 4);
			Assert.IsTrue(col[1].ScheduleDetails.Count == 3);
			Assert.IsTrue(col[2].ScheduleDetails.Count == 3);

			Assert.IsTrue(col[0].IdScheduleHeader == list[0].IdScheduleHeader);
		}

		private List<ScheduleDetail> CreateCurrectMoqList()
		{
			var res = new List<ScheduleDetail>();

			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 1, 8, 0, 0),
				TimeEnd = new DateTime(2000, 1, 1, 8, 15, 0),
			});
			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 1, 8, 15, 0),
				TimeEnd = new DateTime(2000, 1, 1, 8, 30, 0),
			});
			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 1, 8, 30, 0),
				TimeEnd = new DateTime(2000, 1, 1, 8, 45, 0),
			});
			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 1, 8, 45, 0),
				TimeEnd = new DateTime(2000, 1, 1, 9, 0, 0),
			});

			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 2, 8, 0, 0),
				TimeEnd = new DateTime(2000, 1, 2, 8, 15, 0),
			});
			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 2, 8, 15, 0),
				TimeEnd = new DateTime(2000, 1, 2, 8, 30, 0),
			});
			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 2, 8, 30, 0),
				TimeEnd = new DateTime(2000, 1, 2, 8, 45, 0),
			});

			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 4, 8, 0, 0),
				TimeEnd = new DateTime(2000, 1, 4, 8, 15, 0),
			});
			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 4, 8, 15, 0),
				TimeEnd = new DateTime(2000, 1, 4, 8, 45, 0),
				IsBreak = true
			});
			res.Add(new ScheduleDetail()
			{
				Id = Guid.NewGuid(),
				TimeStart = new DateTime(2000, 1, 4, 8, 45, 0),
				TimeEnd = new DateTime(2000, 1, 4, 9, 0, 0),
			});

			return res;
		}
	}
}
