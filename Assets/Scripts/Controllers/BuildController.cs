using GameParametrs;
using GameVariables;
using System.Collections.Generic;
using UnityEngine;
namespace GameControllers
{
    public static class BuildController
    {
        public static List<BuildParametr> AllBuilds;
        
        public static void LoadBuild()
        {
            GameHelpers.InReady.BuildsReady = false;

            var all_builds = Resources.LoadAll<BuildParametr>(GameStaticValues.Path.Builds);
            AllBuilds = new List<BuildParametr>();

            while(true)
            {
                double cost = -1;
                BuildParametr build = null;
                foreach(var b in all_builds)
                {
                    if((b.Cost < cost || cost == -1) && !AllBuilds.Contains(b))
                    {
                        cost = b.StartCost;
                        build = b;
                    }
                }
                if (build == null)
                    break;

                build.Count = 0;

                AllBuilds.Add(build);
            }
            GameHelpers.InReady.BuildsReady = true;
        }

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
