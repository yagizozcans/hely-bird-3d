using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour , IDetailedStoreListener
{
    IStoreController controller;

    [SerializeField] string removeads;

    void Start()
    {
        IAPStart();
    }

    void IAPStart()
    {
        var module = StandardPurchasingModule.Instance();
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        builder.AddProduct(removeads, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Error" + error.ToString());
    }
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log("Error while buying" + p.ToString());
    }



    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (string.Equals(e.purchasedProduct.definition.id, removeads, StringComparison.Ordinal))
        {
            RemoveAds();
        }
        else
        {
            return PurchaseProcessingResult.Pending;
        }
        return PurchaseProcessingResult.Pending;
    }

    void RemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        Destroy(GameObject.Find("NoADS"));
    }

    public  void IAPButton(string id)
    {
        Product prod = controller.products.WithID(id);
        if(prod != null && prod.availableToPurchase)
        {
            Debug.Log("Buying");
            controller.InitiatePurchase(prod);
        }
        else
        {
            Debug.Log("Not");
        }
    }

    void IDetailedStoreListener.OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        throw new NotImplementedException();
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
