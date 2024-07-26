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
			{LocalizationKey.GameOverPhrase, LocalizationKey.GameOverPhrase.ToString()},
			{LocalizationKey.GameOverPhrase1, LocalizationKey.GameOverPhrase1.ToString()},
			{LocalizationKey.GameOverPhrase2, LocalizationKey.GameOverPhrase2.ToString()},
			{LocalizationKey.WinPhrase, LocalizationKey.WinPhrase.ToString()},
			{LocalizationKey.WinPhrase1, LocalizationKey.WinPhrase1.ToString()},
			{LocalizationKey.WinPhrase2, LocalizationKey.WinPhrase2.ToString()},
			{LocalizationKey.TutorialGoToBattle, LocalizationKey.TutorialGoToBattle.ToString()},
			{LocalizationKey.TutorialMove, LocalizationKey.TutorialMove.ToString()},
			{LocalizationKey.TutorialSoldiers, LocalizationKey.TutorialSoldiers.ToString()},
			{LocalizationKey.TutorialTraps, LocalizationKey.TutorialTraps.ToString()},
			{LocalizationKey.TutorialBoost, LocalizationKey.TutorialBoost.ToString()},
			{LocalizationKey.TutorialObstacls, LocalizationKey.TutorialObstacls.ToString()},
			{LocalizationKey.TutorialMoney, LocalizationKey.TutorialMoney.ToString()},
			{LocalizationKey.TutorialFinish, LocalizationKey.TutorialFinish.ToString()},
			{LocalizationKey.TutorialMerge, LocalizationKey.TutorialMerge.ToString()},
			{LocalizationKey.TutorialGameZone, LocalizationKey.TutorialGameZone.ToString()},
			{LocalizationKey.TutorialUpgrade, LocalizationKey.TutorialUpgrade.ToString()},
			{LocalizationKey.TutorialEnd, LocalizationKey.TutorialEnd.ToString()},
		};
	}
}
