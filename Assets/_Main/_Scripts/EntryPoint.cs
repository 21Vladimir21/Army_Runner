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
using _Main._Scripts.TutorialLogic;
using _Main._Scripts.UI;
using Kimicu.YandexGames;
using SoundService.Scripts;
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
        [SerializeField] private AudioService audioService;
        [SerializeField] private TutorialService tutorialService;

        [SerializeField] private Transform levelSpawnPoint;
        [SerializeField] private Transform bulletPoolParent;
        [SerializeField] private Transform soldiersPoolParent;
        [SerializeField] private AutoMergeTimer autoMergeTimer;
        

        [SerializeField] private Player player;

        private LevelStateMachine _levelStateMachine;


        private void Awake()
        {
            SavesService savesService = ServiceLocator.Instance.GetServiceByType<SavesService>();
            ServiceLocator.Instance.TryAddService(audioService);
            ServiceLocator.Instance.TryAddService(tutorialService);
            
            audioService.Init(savesService.Saves.SoundEnabled);
            tutorialService.Init(savesService.Saves);

            var soldiersPool = new SoldiersPool(mainConfig.SoldiersPoolConfig, soldiersPoolParent);
            var levelService = InitLevelService();
            var uiLocator = InitUILocator(savesService);
            var preGameView = uiLocator.GetViewByType<PreGameView>();
            var soundController = new SoundController(preGameView.SoundsButton, preGameView.MusicButton,audioService, savesService.Saves);
            var bulletPool = new BulletPool(mainConfig.BulletPoolConfig, bulletPoolParent);
            player.Init(savesService.Saves, bulletPool, soldiersPool);

            _levelStateMachine =
                new LevelStateMachine(savesService.Saves, levelService, mainConfig, reserveCells, gameCells, uiLocator,
                    cameraService, soldiersPool, player,autoMergeTimer);

            YandexMetrika.Event("Start");
            YandexGamesSdk.GameReady();
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
            if (Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.Z))
            {
                Cloud.ClearCloudData();
                var saves = new Saves();
                saves.InvokeSave();
            }

            _levelStateMachine.Update(); //TODO: Надо придумать куда запихнуть это
        }
    }
}