using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterItem : MonoBehaviour
{
    public UserCharacterModel userCharacterModel;

    [SerializeField]
	Image characterAsset;

	[SerializeField]
	Image typeAsset;

    void Start()
    {
        MasterCharacterModel masterCharacterModel = MasterCharacter.GetMasterCharacter(userCharacterModel.character_id);

        //Resourcesから画像を読み込む場合
		Sprite characterSprite = Resources.Load<Sprite>(masterCharacterModel.asset_id);
        characterAsset.sprite = characterSprite;
        Sprite typeSprite = Resources.Load<Sprite>("CharacterTypeFrame_" + masterCharacterModel.type);
        typeAsset.sprite = typeSprite;
    }

    public void SellButtonEvent()
	{
		Action action = () => {
            //通信後行いたい処理
        };

        UserProfileModel userProfileModel = UserProfile.Get();
        StartCoroutine(CommunicationManager.ConnectServer("character_sell", "&user_id=" + userProfileModel.user_id + "&id=" + userCharacterModel.id, action));
    }
}