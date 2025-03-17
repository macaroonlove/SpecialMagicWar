using UnityEngine;

namespace FrameWork
{
    public static class AnimatorExtensions
    {
        public static bool TrySetTrigger(this Animator animator, int parameterHash)
        {
            foreach (var param in animator.parameters)
            {
                if (param.nameHash == parameterHash)
                {
                    animator.SetTrigger(parameterHash);
                    return true;
                }
            }

            Debug.LogWarning($"�ش� �ִϸ��̼� �Ķ���Ͱ� �������� �ʽ��ϴ� {parameterHash}");
            return false;
        }
    }
}