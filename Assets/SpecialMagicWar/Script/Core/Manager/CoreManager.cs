using FrameWork;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ���ӿ��� �׻� �۵��ϴ� �ý��� ����
    /// </summary>
    public class CoreManager : Singleton<CoreManager>
    {
        private Dictionary<Type, ICoreSystem> _subSystems = new Dictionary<Type, ICoreSystem>();

        internal event UnityAction onCoreInitialize;
        internal event UnityAction onCoreDeinitialize;

        protected override void Awake()
        {
            base.Awake();

            var systems = this.GetComponentsInChildren<ICoreSystem>(true);
            foreach (var system in systems)
            {
                _subSystems.Add(system.GetType(), system);
            }

            InitializeCore();
        }

        private void OnDestroy()
        {
            DeinitializeCore();
        }

        public void InitializeCore()
        {
            foreach (var system in this._subSystems.Values)
            {
                system.Initialize();
            }

            onCoreInitialize?.Invoke();
        }

        public void DeinitializeCore()
        {
            onCoreDeinitialize?.Invoke();

            foreach (var item in _subSystems.Values)
            {
                item.Deinitialize();
            }
        }

        public T GetSubSystem<T>() where T : ICoreSystem
        {
            _subSystems.TryGetValue(typeof(T), out var subSystem);
            return (T)subSystem;
        }
    }
}