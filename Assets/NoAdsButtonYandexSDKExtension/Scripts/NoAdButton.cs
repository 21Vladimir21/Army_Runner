using System.Collections;
using _Main._Scripts.SavesLogic;
using Kimicu.YandexGames;
using Kimicu.YandexGames.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NoAdButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RawImage priceImage;
    [SerializeField] private TMP_Text priceText;

    private Saves _saves;

    private void OnValidate()
    {
        _button ??= GetComponent<Button>();
    }

    public void Init(Saves saves)
    {
        _saves = saves;

        if (_saves.AdEnabled)
            _button.onClick.AddListener(BuyNoAd);
        else _button.gameObject.SetActive(false);

        priceText.text = Billing.CatalogProducts[0].priceValue;
        var picture = Billing.CatalogProducts[0].priceCurrencyPicture;
       Coroutines.StartRoutine(DownloadImage(picture));
    }

    private void BuyNoAd()
    {
        Billing.PurchaseProduct(Boot.PurchaseIndexes.NoAD.ToString(), (purchaseProductResponse) =>
        {
            Billing.ConsumeProduct(purchaseProductResponse.purchaseData.purchaseToken);
            _saves.BuyNoAd();
        });
    }

    private IEnumerator DownloadImage(string textureUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(textureUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log("Download Image Error");
        else
        {
            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            priceImage.texture = texture;
        }
    }
}