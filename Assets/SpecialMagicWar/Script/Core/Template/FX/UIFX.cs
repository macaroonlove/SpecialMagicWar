using FrameWork.Editor;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/FX/UIFX", fileName = "UIFX_", order = 4)]
    public class UIFX : FX
    {
        [SerializeField, Label("UI ��ƼŬ �θ�")] private GameObject _uiParticle;
        [SerializeField, Label("���� �ð�")] private float _duration;

        public override void Play(Unit target)
        {
            if (_uiParticle == null) return;

            Play();
        }

        public override void Play(Vector3 pos)
        {
            if (_uiParticle == null) return;

            Play();
        }

        private void Play()
        {
            var poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();
            var obj = poolSystem.Spawn(_uiParticle, _duration, BattleManager.Instance.canvas);

            var particle = obj.GetComponentInChildren<ParticleSystem>();
            if (particle != null)
            {
                particle.Play();
            }
        }
    }
}