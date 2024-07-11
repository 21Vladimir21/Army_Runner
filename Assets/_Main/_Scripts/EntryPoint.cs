using System.Collections.Generic;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.Level.StateMachine;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.Soilders.Bullets;
using _Main._Scripts.UI;
using UnityEngine;

namespace _Main._Scripts
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private MainConfig mainConfig;

        [SerializeField] private List<CellToMerge> reserveCells;
        [SerializeField] private List<CellToMerge> gameCells;

        [SerializeField] private UIViewsHolder views;
        [SerializeField] private CameraService cameraService;

        [SerializeField] private Transform levelSpawnPoint;
        [SerializeField] private Transform bulletPoolParent;
        [SerializeField] private Transform soldiersPoolParent;
        
        [SerializeField] private Player player;

        private LevelStateMachine _levelStateMachine;


        private void Awake()
        {
             SavesService savesService = ServiceLocator.Instance.GetServiceByType<SavesService>();
#if UNITY_EDITOR
            savesService = InitSaves();
#else
            savesService = ServiceLocator.Instance.GetServiceByType<SavesService>();
#endif

            var soldiersPool = new SoldiersPool(mainConfig.SoldiersPoolConfig, soldiersPoolParent);
            var levelService = InitLevelService();
            var uiLocator = InitUILocator(savesService);
            var bulletPool = new BulletPool(mainConfig.BulletPoolConfig, bulletPoolParent);
            player.Init(savesService.Saves,bulletPool,soldiersPool);
            
            _levelStateMachine =
                new LevelStateMachine(savesService.Saves, levelService, mainConfig, reserveCells, gameCells, uiLocator,
                    cameraService,soldiersPool, player);

        }

        private LevelService InitLevelService()
        {
            var levelService = new LevelService(levelSpawnPoint, mainConfig.LevelsConfig);
            ServiceLocator.Instance.TryAddService(levelService);
            return levelService;
        }

        private UILocator InitUILocator(SavesService savesService)
        {
            var uiLocator = new UILocator(views);
            return uiLocator;
        }

        private SavesService InitSaves()
        {
            var savesService = new SavesService();
            ServiceLocator.Instance.TryAddService(savesService);
            return savesService;
        }

        private void Update()
        {
            _levelStateMachine.Update(); //TODO: Надо придумать куда запихнуть это
        }
    }
}