using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  [Header("怪物")]
  public Transform enemy ;
  [Header("生成點")]
  public GameObject[] points ;
  [Header("間隔時間"),Range(0f,5f)]
  public float interval= 2f ;

    private void Start()
    {
        points = GameObject.FindGameObjectsWithTag("生成點");
        InvokeRepeating("Spawn", 0, interval);
    }
private void Spawn()
    {
        int r = Random.Range(0, points.Length);
        Transform point = points[r].transform;
        Instantiate(enemy, point.position, point.rotation);
    }
}
