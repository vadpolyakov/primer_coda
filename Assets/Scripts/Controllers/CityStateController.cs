using System.Collections.Generic;
using UnityEngine;
using GameParametrs;
using UnityEngine.Events;
using GameVariables;

namespace GameControllers
{
    public static class CityStateController
    {
        public static UnityEvent StatusChanged;

        public static bool NeedCityStateRevard = false;

        public static List<CityStateParametr> AllStates;

        private static int current_state_index = -1;
        public static CityStateParametr CurrentState
        {
            get { if (current_state_index == -1) return null; return AllStates[current_state_index]; }
        }

        private static int next_state_index = -1;
        public static CityStateParametr NextState
        {
            get { if (next_state_index == -1) return null; return AllStates[next_state_index]; }
        }

        public static void CheckState()
        {
            if(current_state_index == -1)
            {
                for(int i = 0; i < AllStates.Count; i++)
                {
                    if (AllStates[i].NeedHumans <= Humans.HumansValue)
                        current_state_index = i;
                }
                if (AllStates.Count > current_state_index + 1)
                    next_state_index = current_state_index + 1;
                else
                    next_state_index = -1;
                UpdateState(false);
                return;
            }
            if (next_state_index == -1)
                return;
            if(AllStates[next_state_index].NeedHumans <= Humans.HumansValue)
            {
                next_state_index++;
                if (AllStates.Count > current_state_index + 1)
                    next_state_index = current_state_index + 1;
                else
                    next_state_index = -1;
                UpdateState();
            }
        }

        public static void UpdateState(bool awards = true)
        {
            NeedCityStateRevard = awards;
            StatusChanged.Invoke();
        }

        public static void LoadStates()
        {
            GameHelpers.InReady.CityStateReady = false;

            StatusChanged = new UnityEvent();

            var all_states = Resources.LoadAll<CityStateParametr>(GameStaticValues.Path.CityStates);

            while(true)
            {
                CityStateParametr parametr = null;
                long need_humans = -1;

                foreach(var p in all_states)
                {
                    if ((p.NeedHumans < need_humans || need_humans == -1) && !AllStates.Contains(p))
                    {
                        parametr = p;
                        need_humans = p.NeedHumans;
                    }
                }

                if (parametr == null)
                    break;

                AllStates.Add(parametr);
            }

            GameHelpers.InReady.CityStateReady = true;
        }
    }
}
