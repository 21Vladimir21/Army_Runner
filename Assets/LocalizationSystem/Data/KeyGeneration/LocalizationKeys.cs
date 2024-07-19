using System.Collections.Generic;
using LocalizationSystem.Data.Extensions;

namespace LocalizationSystem.Data.KeyGeneration
{
	public static class LocalizationKeys
	{
		public static readonly Dictionary<LocalizationKey, string> Keys = new()
		{

			{LocalizationKey.None, LocalizationKey.None.ToString()},
			{LocalizationKey.Battle, LocalizationKey.Battle.ToString()},
			{LocalizationKey.PlusRewardSoldier, LocalizationKey.PlusRewardSoldier.ToString()},
			{LocalizationKey.Damage, LocalizationKey.Damage.ToString()},
			{LocalizationKey.BulletSpeed, LocalizationKey.BulletSpeed.ToString()},
			{LocalizationKey.FireRate, LocalizationKey.FireRate.ToString()},
			{LocalizationKey.GameOver, LocalizationKey.GameOver.ToString()},
			{LocalizationKey.Back, LocalizationKey.Back.ToString()},
			{LocalizationKey.Claim, LocalizationKey.Claim.ToString()},
			{LocalizationKey.NoThanks, LocalizationKey.NoThanks.ToString()},
			{LocalizationKey.Win, LocalizationKey.Win.ToString()},
			{LocalizationKey.BulletScale, LocalizationKey.BulletScale.ToString()},
			{LocalizationKey.DoubleShot, LocalizationKey.DoubleShot.ToString()},
			{LocalizationKey.RewardForTheLevel, LocalizationKey.RewardForTheLevel.ToString()},
			{LocalizationKey.MoneyCollected, LocalizationKey.MoneyCollected.ToString()},
			{LocalizationKey.SoldiersCollected, LocalizationKey.SoldiersCollected.ToString()},
			{LocalizationKey.Level, LocalizationKey.Level.ToString()},
		};
	}
}
