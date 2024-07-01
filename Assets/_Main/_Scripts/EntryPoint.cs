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
        [FormerlySerializedAs("levelSpawner")] [SerializeField] private LevelService levelService;
        [SerializeField] private MainConfig mainConfig;

        [SerializeField] private List<CellToMerge> reserveCells;
        [SerializeField] private List<CellToMerge> gameCells;

        [SerializeField] private UIViewsHolder views;
        [SerializeField] private CameraService cameraService;

        private LevelStateMachine _levelStateMachine;
        
        [SerializeField] private Player player;
        

        private void Awake()
        {
            var savesService = InitSaves();
            var uiLocator = new UILocator(views);
            player.Init(savesService.Saves);
            ServiceLocator.Instance.TryAddService(levelService);
            
            _levelStateMachine =
                new LevelStateMachine(savesService.Saves, levelService, mainConfig, reserveCells, gameCells, uiLocator,
                    cameraService,player);
        }

        private SavesService InitSaves()
        {
            var savesService = new SavesService();
            ServiceLocator.Instance.TryAddService(savesService);
            savesService.InitSaves();
            return savesService;
        }

        private void Update()
        {
            _levelStateMachine.Update(); //TODO: Надо придумать куда запихнуть это
        }
    }
}