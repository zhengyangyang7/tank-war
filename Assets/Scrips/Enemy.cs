using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    //属性值

    private Vector3 bulletEulerAngles;
    public float movespeed = 3;
    private float v;
    private float h;

    //引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite;//上 右 下 左 坦克贴图
    public GameObject bulletPrefab;//子弹贴图
    public GameObject explosionPrefab;//爆炸特效
    public GameObject defendEffectPrefab;//保护特效

    //计时器
    private float timeVal;
    private float TurnTimeVal;


    private void Awake()//取引用 早于start
    {
        sr = GetComponent<SpriteRenderer>();//获得渲染组件
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeVal > 3f)//时间限制
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }
    private void FixedUpdate()//声明周期函数 在update后执行 时间平均 每一帧固定
    {

        Move();

    }
    private void Move()//tank移动方法
    {
        if (TurnTimeVal > 4)
        {
            int num = Random.Range(0, 8);
            if (num >= 5)
            {
                v = -1;
                h = 0;
            }
            else if (num == 0)
            {
                v = 1;
                h = 0;
            }
            else if (num > 0 && num <= 2)
            {
                h = -1;
                v = 0;
            }
            else if (num > 2 && num <= 4)
            {
                h = 1;
                v = 0;
            }
            TurnTimeVal = 0;
        }
        else
        {
            TurnTimeVal += Time.deltaTime;
        }

        transform.Translate(Vector3.up * v * movespeed * Time.fixedDeltaTime, Space.World);
        if (v < 0)
        {
            sr.sprite = tankSprite[2];//旋转向下
            bulletEulerAngles = new Vector3(0, 0, -180);
        }
        else if (v > 0)
        {
            sr.sprite = tankSprite[0];//旋转向上
            bulletEulerAngles = new Vector3(0, 0, 360);
        }
        if (v != 0)//设置优先级，如果垂直方向有操作，直接返回，不管水平操作
        {
            return;
        }


        transform.Translate(Vector3.right * h * movespeed * Time.fixedDeltaTime, Space.World);//x轴的方向
        if (h < 0)
        {
            sr.sprite = tankSprite[3];//旋转向左
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankSprite[1];//旋转向 右
            bulletEulerAngles = new Vector3(0, 0, -90);
        }


    }
    //坦克的攻击方式时间间隔                                       
    private void Attack()
    {

        //子弹产生的角度=坦克的角度=子弹应旋转的角度
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
        //游戏物体，位置，方向
        timeVal = 0;

    }




    //坦克死亡
    private void Die()
    {
        PlayerManager.Instance.playerScore++;
        //产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        //死亡
        Destroy(gameObject);
    }
}
