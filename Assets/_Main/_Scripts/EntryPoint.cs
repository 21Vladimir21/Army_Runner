using System.Collections.Generic;
using _Main._Scripts.Level.StateMachine;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main._Scripts
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private MainConfig mainConfig;

        [SerializeField] private List<CellToMerge> reserveCells;
        [SerializeField] private List<CellToMerge> gameCells;

        [SerializeField] private UIViewsHolder views;
        [SerializeField] private CameraService cameraService;

        [SerializeField] private Player player;
        [SerializeField] private Transform levelSpawnPoint;

        private LevelStateMachine _levelStateMachine;
        private LevelService _levelService;


        private void Awake()
        {
            SavesService savesService;
#if UNITY_EDITOR
            savesService = InitSaves();
#else
            savesService = ServiceLocator.Instance.GetServiceByType<SavesService>();
#endif

            _levelService = new LevelService(levelSpawnPoint, mainConfig.LevelsConfig);
            ServiceLocator.Instance.TryAddService(_levelService);

            var uiLocator = new UILocator(views);
            player.Init(savesService.Saves, mainConfig.Soldiers);

            _levelStateMachine =
                new LevelStateMachine(savesService.Saves, _levelService, mainConfig, reserveCells, gameCells, uiLocator,
                    cameraService, player);
        }

        private SavesService InitSaves()
        {
            var savesService = new SavesService();
            ServiceLocator.Instance.TryAddService(savesService);
            // savesService.InitSaves();
            return savesService;
        }

        private void Update()
        {
            _levelStateMachine.Update(); //TODO: Надо придумать куда запихнуть это
        }
    }
}