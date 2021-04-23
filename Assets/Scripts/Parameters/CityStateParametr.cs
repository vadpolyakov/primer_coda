using UnityEngine;

namespace GameParametrs
{
    [CreateAssetMenu(fileName = "newCityState", menuName = "Parametr/CityState")]
    public class CityStateParametr : ScriptableObject
    {
        public string Title = "TITLE";
        public string Description = "DESCRIPTION";

        public long NeedHumans = 100;
    }
}
