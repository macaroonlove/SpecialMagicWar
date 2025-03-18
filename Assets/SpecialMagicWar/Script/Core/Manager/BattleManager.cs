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

        internal Transform canvas { get; private set; }
        internal PlayerCreateSystem playerCreateSystem { get; private set; }

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

            // TODO: ������ ���� �濡 �����ϸ� RPC�� ���� target�� AllViaServer�� Ready ���θ� ����, �� �÷��̾ ��� �غ� �Ϸ����� ��, ȣ���ϵ��� ����
            //InitializeBattle();
        }

        //[ContextMenu("�÷��̾� ����")]
        private void Start()
        {
            // TODO: ������ ����Ͽ� �濡 �������� ��� ����
            // �÷��̾� ����
            playerCreateSystem.CreatePlayer();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InitializeBattle();
            }
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