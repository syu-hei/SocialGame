using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	[SerializeField]
	Text costLabel;

	[SerializeField]
	Text amountLabel;

    void Start()
	{
		costLabel.text = masterShopModel.cost + " 円";
		amountLabel.text = "× " + masterShopModel.amount;
	}

    public void PressEvent()
    {
        //決済処理を行う
		//決済処理後
		Action action = () => {
			GameObject shopManagerObject = GameObject.Find("ShopManager");
			if (shopManagerObject == null) {
				UnityEngine.Debug.LogError("ShopManagerが存在しません。");
				return;
			}
			ShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();
			if (shopManager == null) {
				UnityEngine.Debug.LogError("ShopManagerがアタッチされていません。");
				return;
			}
			shopManager.Dialog.SetActive(true);
			shopManager.DialogCrystalAmount.text = "× " + masterShopModel.amount;
		};
        UserProfileModel userProfileModel = UserProfile.Get();
        StartCoroutine(CommunicationManager.ConnectServer("Shop", "&user_id=" + userProfileModel.user_id + "&shop_id=" + masterShopModel.shop_id, action))
    }