using FrameWork;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// 배틀에 관련된 시스템 관리
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

            // TODO: 포톤을 통해 방에 입장하면 RPC를 통해 target을 AllViaServer로 Ready 여부를 보냄, 두 플레이어가 모두 준비 완료했을 때, 호출하도록 구현
            //InitializeBattle();
        }

        //[ContextMenu("플레이어 생성")]
        private void Start()
        {
            // TODO: 포톤을 사용하여 방에 입장했을 경우 생성
            // 플레이어 생성
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

        [ContextMenu("배틀시작")]
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