using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SpecialMagicWar.Core
{
    public class WaveSystem : MonoBehaviour, IBattleSystem
    {
        private EnemySpawnSystem _enemySpawnSystem;
        private TimeSystem _timeSystem;
        private WaveLibraryTemplate _waveLibrary;

        private int _currentWaveIndex;
        private bool _isWaveEnd;

        public UnityAction<int, float> onWaveChanged;

        public void Initialize()
        {
            _enemySpawnSystem = BattleManager.Instance.GetSubSystem<EnemySpawnSystem>();
            _timeSystem = BattleManager.Instance.GetSubSystem<TimeSystem>();
            _waveLibrary = GameDataManager.Instance.waveLibrary;
        }

        public void Deinitialize()
        {
            _enemySpawnSystem = null;
            _timeSystem = null;
        }

        private void Update()
        {
            if (_timeSystem == null) return;
            if (_isWaveEnd) return;

            if (_currentWaveIndex >= _waveLibrary.waves.Count)
            {
                _isWaveEnd = true;
                return;
            }

            if (_timeSystem.currentTime >= _waveLibrary.waves[_currentWaveIndex].spawnTime)
            {
                WaveTemplate currentWave = _waveLibrary.waves[_currentWaveIndex];
                StartWave(currentWave);         

                onWaveChanged?.Invoke(_currentWaveIndex + 1, (_waveLibrary.waves.Count == _currentWaveIndex + 1) ? 0 : _waveLibrary.waves[_currentWaveIndex + 1].spawnTime - _waveLibrary.waves[_currentWaveIndex].spawnTime);

                _currentWaveIndex++;
            }
        }

        private void StartWave(WaveTemplate wave)
        {
            foreach (var waveInfo in wave.waveInfo)
            {
                StartCoroutine(SpawnWave(waveInfo));
            }
        }

        private IEnumerator SpawnWave(WaveInfo waveInfo)
        {
            // 딜레이 후 적 스폰
            yield return new WaitForSeconds(waveInfo.delayTime);

            var wfs = new WaitForSeconds(waveInfo.spawnInterval);

            for (int i = 0; i < waveInfo.spawnCount; i++)
            {
                var unit = _enemySpawnSystem.SpawnUnit(waveInfo.template, waveInfo.spawnPosition);

                // 경로 초기화
                unit.GetAbility<MoveWayPointAbility>().InitializeWayPoint(waveInfo.wayPoint);

                if (i < waveInfo.spawnCount - 1)
                {
                    // 스폰 간격 대기
                    yield return wfs;
                }
                
                // 보스의 아이디를 설정해주기(추후 템플릿에서 받아오도록 변경)
                if (waveInfo.template.id == 1)
                {
                    unit.healthAbility.onDeath += () =>
                    {
                        BattleManager.Instance.VictoryBattle();
                    };

                    yield break;
                }
            }
        }
    }
}