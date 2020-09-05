using System;
using UnityEngine;


public class TutorialManager : MonoBehaviour
{

    public void TutorialButtonEvent()
	{
        //ボタンが押された時の処理
        UserProfileModel userProfileModel = userProfileModel.Get();
        StartCoroutine(CommunicationManager.ConnectServer("quest_tutorial", "&user_id=" + userProfileModel.user_id));
    }
}