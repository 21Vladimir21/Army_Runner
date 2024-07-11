using System.Collections;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using Kimicu.YandexGames;
using LocalizationSystem.Components;
using LocalizationSystem.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    private bool _saveInited;

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize();
        yield return Cloud.Initialize();
        Advertisement.Initialize();
        WebApplication.Initialize(null);

        InitSavesService();
        SetLanguage();
        LoadScene();
    }

    private void InitSavesService()
    {
        var savesService = new SavesService();
        ServiceLocator.Instance.TryAddService(savesService);
    }

    private void SetLanguage()
    {
#if UNITY_WEBGL && UNITY_EDITOR
        string lang = "ru";
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
             string lang = YandexGamesSdk.Environment.i18n.lang;
#endif

        if (Localization.SetLocalization(lang))
        {
            LocalizationTextBase.ApplyLocalizationDictionary();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}