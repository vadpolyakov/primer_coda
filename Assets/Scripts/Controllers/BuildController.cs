using GameParametrs;
using GameVariables;
using UnityEngine;
namespace GameControllers
{
    public class BuildController : MonoBehaviour
    {
        public static void BuyBuild(BuildParametr build)
        {
            if (build.Cost > Money.MoneyValue)
                return;

            Money.MoneyValue -= build.Cost;
            build.Count++;

            if (build.MoneyPerSecBonus != 0)
                Money.MoneyPerSec += build.MoneyPerSecBonus;
            if (build.HumansBonus != 0)
                Humans.HumansValue += build.HumansBonus;
            if (build.HappyBonus != 0)
                Happy.HappyValue += build.HappyBonus;
        }
    }
}
