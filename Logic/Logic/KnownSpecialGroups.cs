using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

namespace DreamRecorder . Directory . Logic
{

	public static class KnownSpecialGroups
	{

		public static Guid Everyone => Guid.Parse("CCAD71E9-041B-4190-AE0D-1B7034FF2E15");

		public static Guid AuthorizedEntities => Guid.Parse("D7A1809A-2E3B-4BCD-8927-3E2D96751F1F");

		public static Guid SpecialGroups => Guid.Parse("7C08B933-AE17-4879-9420-CFA209B9690A");

		public static Guid DirectoryServices => Guid.Parse("020e0fc0-01af-470f-85ed-c1b497713c35");

		public static Guid LoginServices => Guid.Parse("162e0359-7a24-41d0-bc92-d627f51b9ae9");

		public static Guid Services => Guid.Parse("C9CF393A-8969-4F7F-8851-AB1AA5192564");

		public static Guid Groups => Guid.Parse("E02BFEA6-EF75-405E-92BE-3141DDC200F1");

		public static Guid Users => Guid.Parse("80F95DEE-A591-4E41-9747-0ECE02585075");

	}

}