using FrameWork;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// ��Ʋ�� ���õ� �ý��� ����
    /// </summary>
    public class BattleManager : Singleton<BattleManager>
    {
        private Dictionary<Type, IBattleSystem> _subSystems = new Dictionary<Type, IBattleSystem>();

        [SerializeField] private int _botCount = 3;

        internal Transform canvas { get; private set; }
        internal PlayerCreateSystem playerCreateSystem { get; private set; }
        internal int botCount => _botCount;

        internal event UnityAction onBattleInitialize;
        internal event UnityAction onBattleDeinitialize;
        internal event UnityAction onBattleManagerDestroy;

        protected override void Awake()
        {
            base.Awake();

            var systems = this.GetComponentsInChildren<IBattleSystem>(true);
            foreach (var system in systems)
            {
                _subSystems.Add(system.GetType(), system);
            }

            canvas = GetComponentInChildren<Canvas>().transform;
            playerCreateSystem = GetComponentInChildren<PlayerCreateSystem>();

            playerCreateSystem.onCreatePlayer += (unit) =>
            {
                InitializeBattle();
            };
        }

        private void Start()
        {
            // �� ����
            for (int i = 1; i <= _botCount; i++)
            {
                playerCreateSystem.CreateBot(i);
            }

            // �÷��̾� ����
            playerCreateSystem.CreatePlayer();
        }

        private void OnDestroy()
        {
            onBattleManagerDestroy?.Invoke();
        }

        [ContextMenu("��Ʋ����")]
        public void InitializeBattle()
        {
            foreach (var system in this._subSystems.Values)
            {
                system.Initialize();
            }

            onBattleInitialize?.Invoke();
        }

        public void DeinitializeBattle()
        {
            onBattleDeinitialize?.Invoke();

            foreach (var item in _subSystems.Values)
            {
                item.Deinitialize();
            }
        }

        public T GetSubSystem<T>() where T : IBattleSystem
        {
            _subSystems.TryGetValue(typeof(T), out var subSystem);
            return (T)subSystem;
        }
    }
}