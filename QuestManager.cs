using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
	Text questNameText;

    private UserProfileModel userProfileModel;

    void Start()
	{
        userProfileModel = UserProfile.Get();
		if (string.IsNullOrEmpty(userProfileModel.user_id)) {
			UnityEngine.Debug.LogError("TitleSceneを起動してユーザー登録を行ってください。");
		}

		MasterQuestModel masterQuestModel = MasterQuest.GetMasterQuest(1); //quest_idが1の場合
		if (masterQuestModel.quest_id == 0) {
			UnityEngine.Debug.LogError("MasterQuestのマスターデータを設定してください。");
			return;
		}
		questNameText.text = masterQuestModel.quest_name;
	}

    public void StartButtonEvent()
	{
		if (string.IsNullOrEmpty(userProfileModel.user_id)) {
			UnityEngine.Debug.LogError("TitleSceneを起動してユーザー登録を行ってください。");
			return;
		}

		Action action = () => {

		};
		StartCoroutine(CommunicationManager.ConnectServer("quest_start", "&user_id=" + userProfileModel.user_id + "&quest_id=1", action));
	}
}