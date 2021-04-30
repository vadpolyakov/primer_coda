using UnityEngine;
using GameParametrs;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GameControllers
{
    public class ShopController : MonoBehaviour
    {
        private static GameObject item_prefub;
        [SerializeField]
        private Transform item_parent;

        [SerializeField]
        private Animator BuildsButtonAnim;
        [SerializeField]
        private Button BuildsButton;
        [SerializeField]
        private Animator OtherButtonAnimator;
        [SerializeField]
        private Button OtherButton;

        private static ShopController instance;

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
            BuildController.BuyBuild(build);
            Debug.Log(build.CostString);
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
                items[build].Cost.text = build.CostString;
            }
        }

        public static void Refresh()
        {
            instance.BuildsButton.interactable = CurrentState != ShopState.Builds;
            instance.OtherButton.interactable = CurrentState != ShopState.Other;

            instance.BuildsButtonAnim.SetBool("Selected", CurrentState == ShopState.Builds);
            instance.OtherButtonAnimator.SetBool("Selected", CurrentState == ShopState.Other);

            CheckAllBuilds();
        }

        private static void CheckAllBuilds()
        {
            foreach (var build in items.Keys)
                CheckBuild(build);
        }

        private void generate_builds()
        {
            foreach(var build in BuildController.AllBuilds)
            {
                ShopItemController controller = Instantiate<GameObject>(item_prefub, item_parent).GetComponent<ShopItemController>();
                items.Add(build, controller);
                controller.Generate(build);
            }

            Refresh();
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
            instance = FindObjectOfType<ShopController>();
            instance.generate_builds();

            instance.BuildsButton.onClick.AddListener(delegate () { CurrentState = ShopState.Builds; });
            instance.OtherButton.onClick.AddListener(delegate () { CurrentState = ShopState.Other; });
            BuildParametr.onUpdate.AddListener(CheckBuild);

            GameVariables.Money.onValueChange.AddListener(CheckAllBuilds);
        }
    }
}
