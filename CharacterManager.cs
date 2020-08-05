using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour
{
    public void CharacterListButtonEvent()
	{
		UserProfileModel userProfileModel = UserProfile.Get();
		if (string.IsNullOrEmpty(userProfileModel.user_id)) {
			UnityEngine.Debug.LogError("TitleSceneを起動してユーザー登録を行ってください。");
			return;
		}

		Action action = () => {
			//キャラクターリスト取得後のアクション
        };
        StartCroutine(CommunicationManager.ConnectServer("character", "&user_id=" + userProfileModel.user_id, action));
    }
}