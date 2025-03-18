using FrameWork;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class UnitAnimationAbility : AlwaysAbility
    {
        private Animator _animator;

        private int _attack;
        private int _moveState;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();

            _attack = Animator.StringToHash("attack");
            _moveState = Animator.StringToHash("isMoving");
        }

        internal void Attack()
        {
            _animator.SetTrigger(_attack);
        }

        internal void Move(bool isMove)
        {
            _animator.SetBool(_moveState, isMove);
        }

        internal bool TrySetTrigger(int hash)
        {
            return _animator.TrySetTrigger(hash);
        }
    }
}