
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("目標")]
    public Transform target;
    [Header("速度"), Range(0, 100)]
    public float speed = 1;
    [Header("旋轉速度"), Range(0, 100)]
    public float turn = 1;
    [Header("上下角度限制")]
    public Vector2 limit = new Vector2(-30, 30);
    /// <summary>
    /// 滑鼠控制旋轉角度
    /// </summary>
    private Quaternion rot;
   /// <summary>
   /// 追蹤
   /// </summary>
    private void Track()
    {
        Vector3 posA = transform.position;                               //A點
        Vector3 posB = target.position;                                  //B點
        posA = Vector3.Lerp(posA, posB, Time.deltaTime * speed);         //A點=Vector3.插值(A點B點,Time.deltaTime*speed);
        transform.position = posA;                                     //攝影機.座標=A點
        ///旋轉
        rot.x += -Input.GetAxis("Mouse Y") * turn;
        rot.y += Input.GetAxis("Mouse X") * turn;
        rot.x = Mathf.Clamp(rot.x, limit.x, limit.y);
        transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);

    }
    private void Awake()
    {
        Cursor.visible = false;//指標.可視=否
    }
    private void LateUpdate()
    {
        Track();
    }
}
