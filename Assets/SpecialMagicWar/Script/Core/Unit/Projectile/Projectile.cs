using FrameWork.Editor;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField, Label("�߻� ��, ȣ��� FX")] protected FX _launchFX;
        [SerializeField, Label("�浹 ��, ȣ��� FX")] protected FX _collisionFX;

        [Space]
        [SerializeField, Label("���� �ٶ��� ����")] protected bool _isLookTarget;

        protected bool _isInit;
        protected UnityAction<Unit, Unit> _action;
        
        protected void DeSpawn()
        {
            CoreManager.Instance.GetSubSystem<PoolSystem>().DeSpawn(gameObject);
            _isInit = false;
        }

        #region FX
        protected void ExecuteCasterFX(Unit caster)
        {
            if (_launchFX != null)
            {
                _launchFX.Play(caster);
            }
        }

        protected void ExecuteTargetFX(Unit target)
        {
            if (_collisionFX != null)
            {
                _collisionFX.Play(target);
            }
        }
        #endregion
    }
}