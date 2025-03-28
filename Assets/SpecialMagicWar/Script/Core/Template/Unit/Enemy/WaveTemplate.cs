using FrameWork.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    /// <summary>
    /// 한 웨이브(스테이지)를 관리
    /// </summary>
    [CreateAssetMenu(menuName = "Templates/Unit/Wave", fileName = "Wave", order = 2)]
    public class WaveTemplate : ScriptableObject
    {
        [Label("최초 스폰 시간")] public float spawnTime;

        public List<WaveInfo> waveInfo = new List<WaveInfo>();
    }

    [Serializable]
    public class WaveInfo
    {
        [Label("적 유닛")] public EnemyTemplate template;

        [Label("스폰 위치")] public Vector3 spawnPosition;
        [Tooltip("최초 스폰 시간을 기준으로 몇 초 있다가 스폰할 것인지")]
        [Label("딜레이")] public float delayTime;
        [Label("스폰할 유닛의 수")] public int spawnCount = 1;
        [Label("스폰 간격")] public float spawnInterval = 0;

        [Label("경로")] public List<Vector3> wayPoint = new List<Vector3>();
    }
}
