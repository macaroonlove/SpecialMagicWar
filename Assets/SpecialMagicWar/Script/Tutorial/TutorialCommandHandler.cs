using FrameWork;
using FrameWork.VisualNovel;
using SpecialMagicWar.Core;
using UnityEngine;

namespace SpecialMagicWar.Tutorial
{
    /// <summary>
    /// Tutorial���� CommandExecutor�� �ݹ��� �޴� Ŭ����
    /// <para>(Core �� Tutorial�� VisualNovel�� ������ �� �� ���)</para>
    /// </summary>
    public class TutorialCommandHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            CommandExecutor.Instance.getItem += GetItem;
        }

        private void OnDisable()
        {
            CommandExecutor.Instance.getItem -= GetItem;
        }

        private void GetItem(ItemType itemType, int amount, string name)
        {
            switch (itemType)
            {
                case ItemType.Gold:
                    CoreManager.Instance.GetSubSystem<GoldSystem>().AddGold(amount);
                    break;
                case ItemType.ActiveItem:
                    AddressableAssetManager.Instance.GetScriptableObject<ActiveItemTemplate>(name, (template) =>
                    {
                        CoreManager.Instance.GetSubSystem<ActiveItemSystem>().AddItem(template);
                    });
                    break;
                case ItemType.PassiveItem:
                    AddressableAssetManager.Instance.GetScriptableObject<PassiveItemTemplate>(name, (template) =>
                    {
                        CoreManager.Instance.GetSubSystem<PassiveItemSystem>().AddItem(template);
                    });
                    break;
            }
        }
    }
}