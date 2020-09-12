using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
     [Header("移動速度"),Range(0.1f,3)]
     public float speed = 2.5f ; 
     [Header("攻擊力"),Range(35f,50f)]
     public float attack =40f ; 
    [Header("血量"),Range(200,300)]
    public float hp = 200; 
    [Header("怪物經驗值"),Range(30,100)]
    public float exp = 30;
    [Header("攻擊停止距離"),Range(0.1f,3)]
    public float distanceAttack = 1.5f;
    [Header("攻擊冷卻時間"),Range(0.1f,5f)]
    public float cd = 2.5f;
    [Header("面向玩家的速度"),Range(0.1f,50f)]
    public float turn = 5f;
    [Header("寶石")]
    public Transform gem;
    [Header("掉落機率1=100%"), Range(0f, 1f)]
    public float gemprop = 0.358f;

    private NavMeshAgent nav;//導覽代理器
    private Animator ani; //動畫控制器
    private Transform player;//玩家
    private float timer;//計時器

    private Rigidbody rig;

    private void Awake()
    {
        ani = GetComponent<Animator>();           //取得動畫控制器
        nav = GetComponent<NavMeshAgent>();       //取得導航代理器器
        nav.speed = speed;                        //設定速度
        nav.stoppingDistance = distanceAttack;    //設定攻擊停止距離
        player = GameObject.Find("U醬").transform;//取得玩家
        nav.SetDestination(player.position);      //避免一開始就進行攻擊動作
        rig = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Move();
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if(other.name=="U醬")
        {
            float range = Random.Range(-10f, 10f);
            other.GetComponent<Player>().Hit(attack+range,transform);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.35f);
        Gizmos.DrawSphere(transform.position, distanceAttack);
    }
    private void OnParticleCollision(GameObject other)
    {
        if(other.name == "碎石")
        {
            Hit(player.GetComponent<Player>().stoneDamage,player.transform);
        }
    }
    private void Move()
    {
        nav.SetDestination(player.position); //追蹤玩家座標
        ani.SetFloat("移動", nav.velocity.magnitude);//設定移動動畫 導覽器.加速度.數值
        if (nav.remainingDistance < distanceAttack) Attack();

    }
    private void Attack()
    {
        Quaternion look = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * turn);
        timer += Time.deltaTime; //計時器累加
        if(timer>=cd)
        {
            timer = 0;
            ani.SetTrigger("攻擊");
        }
    }
    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        ani.SetTrigger("受傷");
        rig.AddForce(direction.forward * 100 + direction.up * 150);

        hp = Mathf.Clamp(hp, 0, 99999);
        if (hp == 0) Dead();   

    }
    private void Dead()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        ani.SetBool("死亡", true);
        enabled = false;
        nav.isStopped = true;
        player.GetComponent<Player>().Exp(exp);
        float r = Random.Range(0f, 1f);

        if (r <= gemprop) Instantiate(gem, transform.position+Vector3.up, transform.rotation);
    }
}
