using System.Collections;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using Agava.YandexGames;
using Kimicu.YandexGames;
using LocalizationSystem.Components;
using LocalizationSystem.Main;
using UnityEngine;
using UnityEngine.SceneManagement;
using Billing = Kimicu.YandexGames.Billing;
using WebApplication = Kimicu.YandexGames.WebApplication;
using YandexGamesSdk = Kimicu.YandexGames.YandexGamesSdk;

public class Boot : MonoBehaviour
{
    private bool _billingSuccses;
    private SavesService _savesService;

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize();
        yield return Cloud.Initialize();
        Advertisement.Initialize();
        WebApplication.Initialize(OnStopGame);

        InitSavesService();

        yield return Billing.Initialize();
        yield return Consume();

        SetLanguage();
        Advertisement.ShowInterstitialAd();
        LoadScene();
    }

    private void InitSavesService()
    {
        _savesService = new SavesService();
        ServiceLocator.Instance.TryAddService(_savesService);
    }

    private IEnumerator Consume()
    {
        Billing.GetPurchasedProducts(UpdateProductCatalog);
        yield return new WaitUntil(() => _billingSuccses);
    }
    
    private void UpdateProductCatalog(GetPurchasedProductsResponse response)
    {
        _billingSuccses = true;
        PurchasedProduct[] purchaseProducts = response.purchasedProducts;

        var countProducts = purchaseProducts.Length;
        for (var i = 0; i < countProducts; i++)
        {
            var product = purchaseProducts[i];
            if (product.productID.Equals(PurchaseIndexes.NoAD.ToString()))
                _savesService.Saves.BuyNoAd();


            Billing.ConsumeProduct(product.purchaseToken);
        }
    }

    private void SetLanguage()
    {
#if   UNITY_EDITOR
        string lang = "en";
#endif
#if   !UNITY_EDITOR
             string lang = YandexGamesSdk.Environment.i18n.lang;
#endif

        if (Localization.SetLocalization(lang))
        {
            LocalizationTextBase.ApplyLocalizationDictionary();
        }
    }
    
    private static void OnStopGame(bool value)
    {
        AudioListener.volume = value ? 1 : 0;
        AudioListener.pause = !value;
        Time.timeScale = value ? 1 : 0;
    }


    private void LoadScene() => SceneManager.LoadScene(sceneBuildIndex: 1);


    internal enum PurchaseIndexes
    {
        NoAD
    }
}