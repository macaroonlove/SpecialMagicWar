using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace FrameWork
{
    /// <summary>
    /// ���� ���� ��, PeristentSingleton�� ��ӹ��� Ŭ������ �ε��ϴ� Ŭ����
    /// </summary>
    public class PersistentLoad
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoLoadAll()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var singletonTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition() == typeof(PersistentSingleton<>))
                .ToList();

                foreach (var type in singletonTypes)
                {
                    PropertyInfo instanceProperty = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    instanceProperty?.GetValue(null);
                }
            }
        }
    }
}