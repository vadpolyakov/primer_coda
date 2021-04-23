using UnityEngine;
using UnityEngine.Events;
using GameHelpers;
using GameStaticValues;
using UnityEngine.Tilemaps;

namespace GameParametrs
{
    public class UpdateBuildParametr : UnityEvent<BuildParametr> { }

    [CreateAssetMenu(fileName = "newBuild", menuName = "Parametr/Build")]
    public class BuildParametr : ScriptableObject
    {
        public static UpdateBuildParametr onUpdate = new UpdateBuildParametr();

        public bool isUpgrade = false;

        public string Title = "TITLE";
        public string Description = "DESCRIPTION";

        public double StartCost = 100;
        public double CostMult = 0.25;

        public double MoneyPerSecBonus = 0;
        [HideInInspector]
        public string MoneyPerSecBonusString 
        { 
            get {   if (MoneyPerSecBonus >= 0) return '+' + InString.TrueString(MoneyPerSecBonus) + Symbols.MoneySymbol + Symbols.PerSecSymbol;
                return InString.TrueString(MoneyPerSecBonus) + Symbols.MoneySymbol + Symbols.PerSecSymbol;  }
        }

        public double HappyBonus = 0;
        [HideInInspector]
        public string HappyBonusString
        {
            get
            {
                if (HappyBonus >= 0) return '+' + InString.TrueString(HappyBonus) + Symbols.HappySymbol;
                return InString.TrueString(HappyBonus) + Symbols.HappySymbol;
            }
        }

        public long HumansBonus = 0;
        [HideInInspector]
        public string HumansBonusString
        {
            get
            {
                if (HumansBonus >= 0) return '+' + InString.TrueString(HumansBonus);
                return InString.TrueString(HumansBonus);
            }
        }

        private long count = 0;
        [HideInInspector]
        public long Count
        {
            get { return count; }
            set
            {
                count = value;
                onUpdate.Invoke(this);
            }
        }
        [HideInInspector]
        public string CountString { get { return InString.TrueString(count); } }

        [HideInInspector]
        public double Cost { get { return StartCost + (StartCost * count * CostMult); } }
        [HideInInspector]
        public string CostString { get { return InString.TrueString(Cost) + Symbols.MoneySymbol; } }

        public Tile BuildTile;

        [SerializeField]
        public System.Guid ID = System.Guid.NewGuid();
    }
}
