using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("NPC資料")]
    public NPCData data;
    [Header("對話框")]
    public GameObject panelDialog;
    [Header("講話的人")]
    public Text textname;
    [Header("內容")]
    public Text textContent;
    [Header("打字速度"), Range(0.1f, 1)]
    public float printSpeed = 0.2f;
    [Header("打字音效")]
    public AudioClip soundPrint;
    [Header("任務區塊")]
    public RectTransform panelMission;
    [Header("任務數量")]
    public Text textMission;
    [Header("傳送門")]
    public GameObject[] doors;

    private AudioSource aud;
    private Animator ani;
    private Player player;
    public int count;

    public void UpdateTextMission()
    {
        count++;
        textMission.text = count + "/" + data.count;
    }
    /// <summary>
    /// 對話系統
    /// </summary>
    public void Dialog()
    {
        panelDialog.SetActive(true);
        textname.text = name;
        StartCoroutine(Print());
    }
    /// <summary>
    /// 取消對話
    /// </summary>
    private void CancelDialog()
    {
        panelDialog.SetActive(false);
        ani.SetBool("說話", false);
    }

    /// <summary>
    /// 打字效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator Print()
    {
        AnimationControl();
        Missioning();
        player.stop = true;//不能動
        string dialog = data.dialogs[(int)data._NPCSatae]; //對話=NPC資料 對話第一段
        textContent.text = "";           //清空
        for (int i=0;i< dialog.Length;i++)//跑對話第一個字到最後一個字
        {
           textContent.text += dialog[i]; //對話內容.文字+-對話[]
            aud.PlayOneShot(soundPrint, 0.5f);
            yield return new WaitForSeconds(printSpeed);
        }
        player.stop = false;//能動
        NoMission();
    }
    /// <summary>
    /// 未接任務狀態切換為任務進行中
    /// </summary>
    private void NoMission()
    {
        //如果狀態為未接任務 將狀態改為任務進行中
        if (data._NPCSatae == NPCState.NoMission)
        {
            data._NPCSatae = NPCState.Missioning;
            StartCoroutine(ShowMission());
        }
    }
    private IEnumerator ShowMission()
    {
        //當任務欄.x>440 就插值跑到440
        while(panelMission.anchoredPosition.x>440)
        {
            panelMission.anchoredPosition = Vector3.Lerp(panelMission.anchoredPosition, new Vector3(440, 270, 0), 30 * Time.deltaTime);
            yield return null;
        }
    }
    /// <summary>
    /// 切換為任務完成
    /// </summary>
    private void Missioning()
    {
        //如果 數量 大於等於NPC需求數量 將狀態改為任務完成
        if (count >= data.count) data._NPCSatae = NPCState.Finish;
        //迴圈執行 將所有傳送門顯示
        for (int i = 0; i < doors.Length; i++) doors[i].SetActive(true);
    }
    /// <summary>
    /// 動畫控制
    /// </summary>
    private void AnimationControl()
    {
        if (data._NPCSatae == NPCState.NoMission || data._NPCSatae == NPCState.Missioning)
            ani.SetBool("說話", true);
        else
            ani.SetTrigger("感謝");
    }
    private void Awake()
    {
        data._NPCSatae = NPCState.NoMission; //遊戲開始時改為未接任務
        aud = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        player = FindObjectOfType<Player>();//透過類型尋找物件*僅現場景只有一個類型
    }
    //進入區域
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "U醬") Dialog();
    }
    //離開區域
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "U醬")CancelDialog() ;
        
    }
}
