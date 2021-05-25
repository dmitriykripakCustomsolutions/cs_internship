using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Lifecycle
{
	public class LifecycleService : IScopedInterface, ITransientInterface, ISingletonInterface
	{
		private readonly string GuidString;

		public LifecycleService()
		{
			GuidString = Guid.NewGuid().ToString();
		}
		public string GetGuid()
		{
			return GuidString;
		}
	}
}
