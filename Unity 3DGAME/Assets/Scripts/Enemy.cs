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

    private NavMeshAgent nav;//導覽代理器
    private Animator ani; //動畫控制器
    private Transform player;//玩家

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        nav.speed = speed;
        nav.stoppingDistance = distanceAttack; 
        player = GameObject.Find("U醬").transform;
    }
    private void Update()
    {
        Move();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.35f);
        Gizmos.DrawSphere(transform.position, distanceAttack);
    }
    private void Move()
    {
        nav.SetDestination(player.position); //追蹤玩家座標
        ani.SetFloat("移動", nav.velocity.magnitude);//設定移動動畫 導覽器.加速度.數值
        if (nav.remainingDistance < distanceAttack) Attack();

    }
    private void Attack()
    {
        ani.SetTrigger("攻擊");
    }
}
