//using UnityEngine;
//using System.Collections;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Extension;
//using UnityEngine.Purchasing.Security;

//public class IAPHelper : MonoBehaviour, IStoreListener
//{
//    private IStoreController m_controller;
//    private IExtensionProvider m_extension;
//    private ConfigurationBuilder m_builder;

//    private const string noAdsProductId = "com.shjy.magicroad.noads";

//    // Use this for initialization
//    void Start()
//    {
//        if (m_controller == null)
//        {
//            m_builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//            // Added in Window-Unity IAP-IAP Catalog, don't need to add programmatically
//            //m_builder.AddProduct("com.shjy.magicroad.noads", ProductType.NonConsumable, new IDs
//            //{
//            //    {"com.shjy.magicroad.noads", AppleAppStore.Name},
//            //});
//            m_builder.AddProduct(noAdsProductId, ProductType.NonConsumable);
//            UnityPurchasing.Initialize(this, m_builder);
//        }
//    }

//    // This will be called with the button callback
//    // productId: com.shjy.magicroad.noads
//    public void BuyProductID(string productId)
//    {
//        if (IsInitialized())
//        {
//            Product product = m_controller.products.WithID(productId);
//            if (product != null && product.availableToPurchase)
//            {
//                m_controller.InitiatePurchase(product);

//                Debug.Log(string.Format("IAPHelper::OnPurchaseClicked {0}", product.metadata.localizedPrice));
//            }
//            else
//            {
//                Debug.Log("IAPHelper::OnPurchaseClicked failed: Not purchasing product");
//            }
//        }
//        else
//        {
//            Debug.Log("IAPHelper::OnPurchaseClicked failed: Not purchasing product");
//        }
//    }

//    // If success, ProcessPurchase() will be called
//    public void RestorePurchases()
//    {
//        if (!IsInitialized())
//        {
//            Debug.Log("IAPHelper::RestorePurchases failed. Not initialized");
//            return;
//        }

//        if (m_extension != null
//            && (Application.platform == RuntimePlatform.IPhonePlayer
//                || Application.platform == RuntimePlatform.OSXPlayer))
//        {
//            Debug.Log("IAPHelper::RestorePurchases started ...");
//            var apple = m_extension.GetExtension<IAppleExtensions>();
//            apple.RestoreTransactions((result) => {
//                // Restore purchases initiated. See ProcessPurchase for any restored transacitons.
//                Debug.Log("IAPHelper::RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//            });
//        }
//        else
//        {
//            Debug.Log("IAPHelper::RestorePurchases failed: Not supported on this platform. Current = " + Application.platform);
//        }

//    }

//    private bool IsInitialized()
//    {
//        return m_controller != null && m_extension != null;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    /// <summary>
//    /// Called when Unity IAP is ready to make purchases.
//    /// </summary>
//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {
//        m_controller = controller;
//        m_extension = extensions;
//        Debug.Log(string.Format("IAPHelper::OnInitialized"));
//    }

//    /// <summary>
//    /// Called when Unity IAP encounters an unrecoverable initialization error.
//    ///
//    /// Note that this will not be called if Internet is unavailable; Unity IAP
//    /// will attempt initialization until it becomes available.
//    /// </summary>
//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        Debug.Log(string.Format("IAPHelper::OnInitializeFailed: {0}", error.ToString()));
//    }

//    /// <summary>
//    /// Called when a purchase completes.
//    ///
//    /// May be called at any time after OnInitialized().
//    /// </summary>
//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
//    {
//        //获取商品的描述信息，标题，价格，带单位的（￥，$等等）
//        //这一步一般可以放在初始化完成的回调里，用于刷新你的相关UI
//        Debug.Log("IAPHelper::ProcessPurchase price:" + e.purchasedProduct.metadata.localizedPriceString);
//        Debug.Log(e.purchasedProduct.metadata.localizedTitle + e.purchasedProduct.metadata.localizedDescription);

//        //商品的id(com.shjy.magicroad.noads)
//        Debug.Log("IAPHelper::ProcessPurchase storeSpecificId:" + e.purchasedProduct.definition.storeSpecificId);

//        //回执单  
//        Debug.Log("IAPHelper::ProcessPurchase receipt:" + e.purchasedProduct.receipt);

//        //交易号
//        Debug.Log("IAPHelper::ProcessPurchase transactionID:" + e.purchasedProduct.transactionID);

//        Debug.Log("IAPHelper::ProcessPurchase Sucessed!!!");

//        // remove ads
//        if (e.purchasedProduct.definition.storeSpecificId == noAdsProductId)
//        {
//            PlayerData.BuyRemoveAds();
//            TextTipsPanel.instance.ShowText("Purchase Succeeded");
//        }

//        //这里是获取订单号，用于存储到自己的服务器，以及恢复购买时的对比
////#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
////        // Get a reference to IAppleConfiguration during IAP initialization.
////        var appleConfig = m_builder.Configure<IAppleConfiguration>();
////        var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
////        AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

////        Debug.Log(receipt.bundleID);
////        Debug.Log(receipt.receiptCreationDate);
////        foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts)
////        {
////            Debug.Log("订单号：" + productReceipt.originalTransactionIdentifier);
////        }
////#endif
//        return PurchaseProcessingResult.Complete;
//    }

//    /// <summary>
//    /// Called when a purchase fails.
//    /// </summary>
//    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
//    {
//        Debug.Log(string.Format("IAPHelper::OnPurchaseFailed: {0}", p.ToString()));
//    }
//}
