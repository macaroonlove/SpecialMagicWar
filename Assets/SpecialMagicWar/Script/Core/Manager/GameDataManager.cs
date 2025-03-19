using FrameWork;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class GameDataManager : PersistentSingleton<GameDataManager>
    {
        [SerializeField] private PlayerLibraryTemplate _playerLibrary;
        [SerializeField] private HolyAnimalLibraryTemplate _holyAnimalLibrary;
        [SerializeField] private EnemyLibraryTemplate _bountyLibrary;
        [SerializeField] private WaveLibraryTemplate _waveLibrary;
        
        internal WaveLibraryTemplate waveLibrary => _waveLibrary;
        internal EnemyLibraryTemplate bountyLibrary => _bountyLibrary;

        internal AgentTemplate GetPlayerTemplateById(int id)
        {
            return _playerLibrary.templates.Where(x => x.id == id).FirstOrDefault();
        }

        internal HolyAnimalTemplate GetHolyAnimalTemplateById(int id)
        {
            return _holyAnimalLibrary.templates.Where(x => x.id == id).FirstOrDefault();
        }
    }
}