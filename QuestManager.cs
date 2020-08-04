using UnityEngine;

public class QuestManager : MonoBehaviour
{
    void Start()
	{
        UserProfileModel userProfileModel = UserProfile.Get();
        if (userProfileModel.tutorial_progress < UserProfile.TUTORIAL_QUEST) {
            ///チュートリアルの処理
        } else {
            //通常処理
        }
    }
}