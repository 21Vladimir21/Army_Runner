using System.Collections;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using Kimicu.YandexGames;
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

        yield return new WaitUntil(InitSavesService);
       
        LoadScene();
    }
    private bool InitSavesService()
    {
        var savesService = new SavesService();
        ServiceLocator.Instance.TryAddService(savesService);
        savesService.LoadSaveData(()=>_saveInited = true);
        return true;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    } 
}
