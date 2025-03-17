using FrameWork.Editor;
using System.Collections;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    [CreateAssetMenu(menuName = "Templates/FX/VFX", fileName = "VFX_", order = 1)]
    public class VFX : FX
    {
        [SerializeField, Label("��ƼŬ�ΰ�?")] private bool _isParticle = true;
        [SerializeField, Label("VFX")] private GameObject _vfxObj;
        [SerializeField, Label("���� ��ġ")] private ESpawnPoint _spawnPoint;
        
        [Space(10)]
        [SerializeField, Label("�������� ����")] private bool _isInfinity;
        [Condition("_isInfinity", false, false)]
        [SerializeField, Label("���� �ð�")] private float _duration;

        [Space(10)]
        [Tooltip("������ ������� �Ͽ��� ���� ����")]
        [SerializeField, Label("�ݺ� ���� ����")] private bool _isRepeat;
        [Condition("_isRepeat", true, false)]
        [SerializeField, Label("���� ����Ŭ")] private float _cycle;

        [Space(10)]
        [SerializeField, Label("��ġ ������")] private Vector3 _posOffset;
        [SerializeField, Label("ȸ�� ������")] private Vector3 _rotOffset;

        [Space(10)]
        [Tooltip("Ÿ�� ������� �����߸� ��� ����")]
        [SerializeField, Label("���� ������ ����")] private bool _isFollowTarget;

        public override void Play(Unit target)
        {
            if (target == null) return;

            var fxAbility = target.GetAbility<FXAbility>();

            if (_isRepeat)
            {
                var coroutine = fxAbility.StartCoroutine(CoPlay(target, fxAbility));
                fxAbility.AddCoroutineFX(_vfxObj, coroutine);
            }
            else
            {
                DoPlay(target, fxAbility);
            }            
        }

        private IEnumerator CoPlay(Unit target, FXAbility fxAbility)
        {
            var wfs = new WaitForSeconds(_cycle);

            while (true)
            {
                DoPlay(target, fxAbility);
                yield return wfs;
            }
        }

        private void DoPlay(Unit target, FXAbility fxAbility)
        {
            Vector3 pos = GetSpawnPoint(target);

            Quaternion baseRot = target.transform.rotation;
            Quaternion rot = baseRot * Quaternion.Euler(_rotOffset);

            var obj = Play(pos, rot);

            if (_isFollowTarget)
            {
                Follow follow = obj.gameObject.GetComponent<Follow>();
                if (follow == null)
                {
                    follow = obj.gameObject.AddComponent<Follow>();
                }
                follow.SetTarget(target.transform, _posOffset);
            }

            fxAbility.AddFX(obj);
        }

        public override void Play(Vector3 pos)
        {
            Quaternion rot = Quaternion.Euler(_rotOffset);

            Play(pos, rot);
        }

        private GameObject Play(Vector3 pos, Quaternion rot)
        {
            var poolSystem = CoreManager.Instance.GetSubSystem<PoolSystem>();
            
            GameObject obj;
            if (_isInfinity)
            {
                obj = poolSystem.Spawn(_vfxObj);
            }
            else
            {
                obj = poolSystem.Spawn(_vfxObj, _duration);
            }

            pos += _posOffset;

            obj.transform.SetPositionAndRotation(pos, rot);

            if (_isParticle)
            {
                var particle = obj.GetComponent<ParticleSystem>();
                if (particle != null)
                {
                    particle.Play();
                }
            }

            return obj;
        }

        private Vector3 GetSpawnPoint(Unit target)
        {
            Vector3 point = target.transform.position;

            switch (_spawnPoint)
            {
                case ESpawnPoint.Head:
                    if (target.headPoint != null)
                    {
                        point = target.headPoint.position;
                    }
                    break;
                case ESpawnPoint.Body:
                    if (target.bodyPoint != null)
                    {
                        point = target.bodyPoint.position;
                    }
                    break;
                case ESpawnPoint.LeftHand:
                    if (target.leftHandPoint != null)
                    {
                        point = target.leftHandPoint.position;
                    }
                    break;
                case ESpawnPoint.RightHand:
                    if (target.rightHandPoint != null)
                    {
                        point = target.rightHandPoint.position;
                    }
                    break;
                case ESpawnPoint.Foot:
                    if (target.footPoint != null)
                    {
                        point = target.footPoint.position;
                    }
                    break;
                case ESpawnPoint.ProjectileHit:
                    if (target.projectileHitPoint != null)
                    {
                        point = target.projectileHitPoint.position;
                    }
                    break;
            }

            return point;
        }
    }
}