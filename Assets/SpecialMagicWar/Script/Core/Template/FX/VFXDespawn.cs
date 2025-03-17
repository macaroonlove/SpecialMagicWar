using FrameWork.Editor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/FX/VFX Despawn", fileName = "VFX Despawn_", order = 2)]
    public class VFXDespawn : FX
    {
        [SerializeField, Label("���ֿ��Լ� ������ VFX")] private GameObject _vfxObj;
        
        public override void Play(Unit target)
        {
            if (target == null) return;

            target.GetAbility<FXAbility>().DespawnFX(_vfxObj);
        }

        public override void Play(Vector3 pos)
        {
#if UNITY_EDITOR
            Debug.LogError("Ư�� ��ġ���� ������ �� �����ϴ�.");
#endif
        }
    }
}