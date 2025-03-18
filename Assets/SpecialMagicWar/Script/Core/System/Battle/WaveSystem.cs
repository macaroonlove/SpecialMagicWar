using System.Collections;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class WaveSystem : MonoBehaviour, IBattleSystem
    {
        private EnemySpawnSystem _enemySpawnSystem;
        private TimeSystem _timeSystem;
        private WaveLibraryTemplate _waveLibrary;

        private int _currentWaveIndex;
        private bool _isWaveEnd;

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
            // ������ �� �� ����
            yield return new WaitForSeconds(waveInfo.delayTime);

            var wfs = new WaitForSeconds(waveInfo.spawnInterval);

            for (int i = 0; i < waveInfo.spawnCount; i++)
            {
                var unit = _enemySpawnSystem.SpawnUnit(waveInfo.template, waveInfo.spawnPosition);

                // ��� �ʱ�ȭ
                unit.GetAbility<MoveWayPointAbility>().InitializeWayPoint(waveInfo.wayPoint);

                if (i < waveInfo.spawnCount - 1)
                {
                    // ���� ���� ���
                    yield return wfs;
                }
            }
        }
    }
}