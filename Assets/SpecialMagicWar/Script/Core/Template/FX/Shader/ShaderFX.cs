using UnityEngine;

namespace SpecialMagicWar.Core
{
    public abstract class ShaderFX : FX
    {
        public override void Play(Vector3 pos)
        {
#if UNITY_EDITOR
            Debug.LogError("Ư�� ��ġ���� ������ �� �����ϴ�.");
#endif
        }
    }
}
