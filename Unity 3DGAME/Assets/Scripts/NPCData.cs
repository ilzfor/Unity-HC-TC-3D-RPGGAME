using UnityEngine;



    //列舉:下拉式選單
public enum NPCState
    {
        NoMission,Missioning,Finish
    }
 //ScriptableObject腳本化物件:可儲存於專案的資料
 [CreateAssetMenu(fileName ="NPC資料",menuName ="JO/NPC資料")]
public class NPCData:ScriptableObject
{
    [Header("NPC狀態")]
    public NPCState _NPCSatae= NPCState.NoMission;
    [Header("任務需求數量")]
    public int count;
    [Header("對話:未取得任務、任務進行中、任務完成")]
    public string[] dialogs = new string[3];
}

