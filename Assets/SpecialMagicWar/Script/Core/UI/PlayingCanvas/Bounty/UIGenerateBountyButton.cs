using FrameWork;
using FrameWork.UIBinding;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpecialMagicWar.Core
{
    public class UIGenerateBountyButton : UIBase
    {
        #region ¹ÙÀÎµù
        enum Buttons
        {
            GenerateButton,
        }
        enum Images
        {
            Icon,
            Bounty,
            BountyIcon,
        }
        enum Texts
        {
            HPText,
            BountyText,
        }
        #endregion

        [SerializeField] private Sprite _costSprite;
        [SerializeField] private Sprite _soulSprite;
        [SerializeField] private Color _costColor;
        [SerializeField] private Color _soulColor;

        private EnemySpawnSystem _enemySpawnSystem;
        private EnemyTemplate _template;
        private UIBountyCanvas _uiBountyCanvas;
        private Vector3 _spawnPos;
        private List<Vector3> _wayPoint = new List<Vector3>();

        private bool _isBind = false;

        internal void Initialize(EnemyTemplate template, UIBountyCanvas uiBountyCanvas)
        {
            Binding();

            _spawnPos = new Vector3(-2.4f, -2, 0);
            _wayPoint.Clear();
            _wayPoint.Add(new Vector3(-2.4f, 4, 0));

            _enemySpawnSystem = BattleManager.Instance.GetSubSystem<EnemySpawnSystem>();
            _template = template;
            _uiBountyCanvas = uiBountyCanvas;

            GetImage((int)Images.Icon).sprite = template.sprite;
            GetText((int)Texts.HPText).text = $"¢¾{template.MaxHP.Format(4)}";

            if (template.gainCost > 0)
            {
                GetImage((int)Images.BountyIcon).sprite = _costSprite;
                GetImage((int)Images.Bounty).color = _costColor;
                GetText((int)Texts.BountyText).text = $"{template.gainCost}";
            }
            else
            {
                GetImage((int)Images.BountyIcon).sprite = _soulSprite;
                GetImage((int)Images.Bounty).color = _soulColor;
                GetText((int)Texts.BountyText).text = $"{template.gainSoul}";
            }
        }

        private void Binding()
        {
            if (_isBind) return;

            BindButton(typeof(Buttons));
            BindImage(typeof(Images));
            BindText(typeof(Texts));

            GetButton((int)Buttons.GenerateButton).onClick.AddListener(Generate);

            _isBind = true;
        }

        private void Generate()
        {
            var unit = _enemySpawnSystem.SpawnUnit(_template, _spawnPos);

            if (unit == null) return;

            unit.GetAbility<MoveWayPointAbility>().InitializeWayPoint(_wayPoint);

            _uiBountyCanvas.DisableGenerate();
        }
    }
}