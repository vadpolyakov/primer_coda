using UnityEngine;
using UnityEngine.UI;
using GameVariables;

namespace GameControllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Button MainButton;
        [SerializeField]
        private Animator MainButtonAnim;
        [SerializeField]
        private Button ShopButton;
        [SerializeField]
        private Animator ShopButtonAnim;
        [SerializeField]
        private Animator ShopAnimator;
        [SerializeField]
        private Button SettingsButton;
        [SerializeField]
        private Animator SettingsButtonAnim;
        [SerializeField]
        private Animator SettingsAnimator;

        [SerializeField]
        private Text MoneyText;
        [SerializeField]
        private Text HappyText;
        [SerializeField]
        private Text HumansText;

        public static GamePanel Game_Panel = GamePanel.Main;

        public static void ChangeMainState(GameStatus status)
        {

        }

        private void ChangeGamePanel(GamePanel newPanel)
        {
            MainButtonAnim.SetBool("Selected", newPanel == GamePanel.Main);
            ShopButtonAnim.SetBool("Selected", newPanel == GamePanel.Shop);
            SettingsButtonAnim.SetBool("Selected", newPanel == GamePanel.Settings);

            ShopAnimator.SetBool("Open", newPanel == GamePanel.Shop);
            SettingsAnimator.SetBool("Open", newPanel == GamePanel.Settings);
        }

        public static void Starting()
        {
            FindObjectOfType<UIController>().connect();
        }

        private void connect()
        {
            MainButton.onClick.AddListener(delegate () { ChangeGamePanel(GamePanel.Main); });
            ShopButton.onClick.AddListener(delegate () { ChangeGamePanel(GamePanel.Shop); });
            SettingsButton.onClick.AddListener(delegate () { ChangeGamePanel(GamePanel.Settings); });

            Money.onValueChange.AddListener(delegate () { MoneyText.text = Money.MoneyString; });
            Happy.onValueChange.AddListener(delegate () { HappyText.text = Happy.HappyString; });
            Humans.onValueChange.AddListener(delegate () { HumansText.text = Humans.HappyString; });

            ChangeGamePanel(GamePanel.Main);
        }
    }

    public enum GamePanel
    {
        Main,
        Shop,
        Settings
    }
}
