using UnityEngine;
using GameParametrs;
using System.Collections.Generic;

namespace GameControllers
{
    public class ShopController : MonoBehaviour
    {
        private static GameObject item_prefub;
        [SerializeField]
        private Transform item_parent;

        public enum ShopState
        {
            Builds,
            Shops,
            Factorys,
            Other
        }

        public enum ShopFilter
        {
            None,
            PerCost
        }

        private static Dictionary<BuildParametr, ShopItemController> items;

        private static ShopState current_state = GameStaticValues.DefaultValues.ShopState;
        private static ShopState CurrentState { get { return current_state; } set { current_state = value; Refresh(); } }

        public static void SelectBuild(BuildParametr build)
        {
            Debug.Log(build.Title);
        }

        public static void CheckBuild(BuildParametr build)
        {
            if (build.NeedStateIndex > CityStateController.CurrentStateIndex || CurrentState != GetShopState(build.BuildType))
            {
                items[build].LayoutElement.ignoreLayout = true;
                items[build].CanvasGroup.alpha = 0;
                items[build].CanvasGroup.interactable = false;
                items[build].CanvasGroup.blocksRaycasts = false;
            }
            else
            {
                items[build].LayoutElement.ignoreLayout = false;
                items[build].CanvasGroup.alpha = 1;
                items[build].CanvasGroup.interactable = true;
                items[build].CanvasGroup.blocksRaycasts = true;
                items[build].Animator.SetBool("Open", build.CanBuy);
            }
        }

        public static void Refresh()
        {
            foreach(var build in items.Keys)
                CheckBuild(build);
        }

        private void generate_builds()
        {
            foreach(var build in BuildController.AllBuilds)
            {
                ShopItemController controller = Instantiate<GameObject>(item_prefub, item_parent).GetComponent<ShopItemController>();
                items.Add(build, controller);
                controller.Generate(build);
                CheckBuild(build);
            }
        }

        private static ShopState GetShopState(BuildType type)
        {
            switch(type)
            {
                case BuildType.Build: return ShopState.Builds;
                case BuildType.Factory: return ShopState.Factorys;
                case BuildType.Shop: return ShopState.Shops;
                default: return ShopState.Other;
            }
        }

        public static void Starting()
        {
            item_prefub = Resources.Load<GameObject>("UI/ShopItem");
            items = new Dictionary<BuildParametr, ShopItemController>();
            FindObjectOfType<ShopController>().generate_builds();
            BuildParametr.onUpdate.AddListener(CheckBuild);
        }
    }
}
