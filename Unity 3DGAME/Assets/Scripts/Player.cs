using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("速度"), Range(0, 1000)]
    public float speed = 1;
    [Header("旋轉角度"), Range(0, 1000)]
    public float turn = 60;

    //在屬性面板上隱藏
    [HideInInspector]
    /// <summary>
    /// 停止不能移動
    /// </summary>
    public bool stop;

    [Header("傳送門:0 NPC，1殭屍")]
    public Transform[] doors;
    [Header("介面區塊")]
    public Image barHP;
    public Image barMP;
    public Image barEXP;

    private float attack = 10f;
    private float hp = 300f;
    private float maxhp = 300f;
    private float mp = 50f;
    private float exp;
    private int lv = 1;

    private Rigidbody rig;
    private Animator ani;
    private Transform cam;//攝影機根物件
    private NPC npc;
    #endregion
    #region 事件
    private void Awake()
    {
        //取得元件<泛型>();
        //泛型:所有類型
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        cam=GameObject.Find("攝影機根物件").transform;

        npc = FindObjectOfType<NPC>();
    }
    private void FixedUpdate()
    {
        if (stop) return;//如果 停止 跳出
        
            Move();
       
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "寶石") GetProp(collision.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "傳送門-NPC")
        {
            transform.position = doors[1].position;
            doors[1].GetComponent<CapsuleCollider>().enabled = false;
            Invoke("OpenDoorBUG", 3);
        }
        if (other.name == "傳送門-甲蟲")
        {
            transform.position = doors[0].position;
            doors[0].GetComponent<CapsuleCollider>().enabled = false;
            Invoke("OpenDoorNPC", 3);
        }

    }
    #endregion
    #region 方法
    private void OpenDoorNPC()
    {
        doors[0].GetComponent<CapsuleCollider>().enabled = true;
    }
    private void OpenDoorBUG()
    {
        doors[1].GetComponent<CapsuleCollider>().enabled = true;
    }
    /// <summary>
    /// 移動方法:前後左右移動與動畫
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxis("Vertical");//前後:WS 上下
        float h = Input.GetAxis("Horizontal");
        Vector3 pos =cam.forward*v+cam.right*h ;//移動座標=攝影機前後左右(左右，0，前後)
        rig.MovePosition(transform.position + pos*speed);//移動座標(原本座標+移動座標*速度)

        ani.SetFloat("移動", Mathf.Abs(v) + Mathf.Abs(h));//設定浮點數(絕對值V與H)
        if (v != 0 || h != 0)//如果控制中
        {
            pos.y = 0;
            Quaternion angle = Quaternion.LookRotation(pos);//B角度=面向(移動座標)
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, turn);//A角度.插值(A角度，B角度，旋轉角度)
        }
    }
    private void Attack()
    {

    }
    private void Skill()
    {

    }
    /// <summary>
    /// 取得道具
    /// </summary>
    /// <param name="prop">碰到的道具</param>
    private void GetProp(GameObject prop)
    {
        Destroy(prop);
        //播放音效
        npc.UpdateTextMission();
    }
    public void Hit(float damage,Transform direction)
    {
        hp -= damage;
        ani.SetTrigger("受傷");
        rig.AddForce(direction.forward * 100 + direction.up * 150);

        hp = Mathf.Clamp(hp, 0, 99999);
        barHP.fillAmount = hp / maxhp;
        if (hp == 0) Dead();
    }
    private void Dead()
    {
        ani.SetBool("死亡", true);
        enabled = false;
    }
    private void Exp()
    {

    }
    #endregion

}
