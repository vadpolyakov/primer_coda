using UnityEngine;
using UnityEngine.UI;

namespace GameControllers
{
    public class ShopItemController : MonoBehaviour
    {
        public Text Title;
        public Text Cost;
        public Image Icon;
        public Text MoneyBonus;
        public Text HappyBonus;
        public Text HumansBonus;

        public Animator Animator;
        public Button Button;

        public LayoutElement LayoutElement;
        public CanvasGroup CanvasGroup;

        public void Generate(GameParametrs.BuildParametr build)
        {
            Title.text = build.Title;
            Cost.text = build.CostString;
            Icon.sprite = build.BuildTile.sprite;

            if (build.MoneyPerSecBonus == 0)
                MoneyBonus.transform.parent.gameObject.SetActive(false);
            else
                MoneyBonus.text = build.MoneyPerSecBonusString;

            if (build.HappyBonus == 0)
                HappyBonus.transform.parent.gameObject.SetActive(false);
            else
                HappyBonus.text = build.HappyBonusString;

            if (build.HumansBonus == 0)
                HumansBonus.transform.parent.gameObject.SetActive(false);
            else
                HumansBonus.text = build.HumansBonusString;

            Button.onClick.AddListener(delegate () { ShopController.SelectBuild(build); });
        }
    }
}
