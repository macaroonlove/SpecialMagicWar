using FrameWork.Loading;
using FrameWork.UIBinding;

namespace SpecialMagicWar.Core
{
    public class UIBattleResultCanvas : UIBase
    {
        #region 바인딩
        enum Texts
        {
            BattleResultText,
        }
        enum Buttons
        {
            RestartButton,
            QuitButton,
        }
        #endregion

        protected override void Initialize()
        {
            BindText(typeof(Texts));
            BindButton(typeof(Buttons));

            GetButton((int)Buttons.RestartButton).onClick.AddListener(Restart);
            GetButton((int)Buttons.QuitButton).onClick.AddListener(Quit);

            BattleManager.Instance.onVictory += OnVictory;
            BattleManager.Instance.onDefeat += OnDefeat;
        }

        private void OnDestroy()
        {
            BattleManager.Instance.onVictory -= OnVictory;
            BattleManager.Instance.onDefeat -= OnDefeat;
        }

        private void OnVictory()
        {
            Show(true);

            GetText((int)Texts.BattleResultText).text = "<color=blue>승리</color>";
        }

        private void OnDefeat()
        {
            Show(true);

            GetText((int)Texts.BattleResultText).text = "<color=red>패배</color>";
        }

        private void Restart()
        {
            LoadingManager.Instance.LoadScene("SpecialMagicWar");
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}