using System.Collections.Generic;
using LocalizationSystem.Data.Extensions;

namespace LocalizationSystem.Data.KeyGeneration
{
	public static class LocalizationKeys
	{
		public static readonly Dictionary<LocalizationKey, string> Keys = new()
		{

			{LocalizationKey.None, LocalizationKey.None.ToString()},
		};
	}
}
