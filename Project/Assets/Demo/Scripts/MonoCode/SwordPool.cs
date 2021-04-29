using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 剑池
/// </summary>
public class SwordPool : MonoBehaviour
{
    public GameObject SwordRoot;
    public GameObject SwordItem;

    public List<GameObject> CurrShowSwordList = new List<GameObject>();
    public List<GameObject> CacheSwordList    = new List<GameObject>();

    private Sequence IdleSequence = null;

    private void Awake()
    {
        IdleSequence = DOTween.Sequence();
        TurnSpeed = (StartPoint.x - EndPoint.x)/ TurnTime;
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Create();
        }

        Turn();
    }

    #region TurnPool
    public Vector3 StartPoint;//开始位置
    public Vector3 EndPoint;//结束位置
    public AnimationCurve AnimTurnScale;
    public float TurnSpeed;

    public float TurnTime=1;

    public float TurnScaleValue = 0;

    [Header("剑池最多展示几个")]
    public int SwordIdleShowMaxCnt = 5;

    public List<float> CurveTimeList = new List<float>();
    #endregion

    private void Turn()
    {
        float delayTime = TurnTime / SwordIdleShowMaxCnt;
        for (int i = 0; i < CurrShowSwordList.Count; i++)
        {
            if (i >= SwordIdleShowMaxCnt)
            {
                CurrShowSwordList[i].SetActive(false);
            }
            else
            {
                TurnSword(CurrShowSwordList[i], i, delayTime);

            }
        }
    }

    private void TurnSword(GameObject swordItem,int index,float delayTime)
    {

        //曲线时间
        float curveTime = 0;
        if (index <= CurveTimeList.Count-1)
        {
            curveTime = CurveTimeList[index];
        }
        else
        {
            //找上一个
            int lastIndex = index - 1;
            if (lastIndex<0)
            {
                curveTime = Time.time;
                CurveTimeList.Add(curveTime);
            }
            else
            {
                curveTime = CurveTimeList[lastIndex] - delayTime;
                CurveTimeList.Add(curveTime);
            }
           
        }

        float value = Time.time - curveTime;
        if (value >= TurnTime)
        {
            value = 0;

            curveTime = Time.time;
            CurveTimeList[index] = curveTime;
        }

        float endDis = Vector3.Distance(swordItem.transform.localPosition, EndPoint);
        //到达终点
        if (endDis<=0.01f && swordItem.transform.localScale.x==0.85f)
        {
            swordItem.gameObject.SetActive(false);
            swordItem.transform.localPosition = StartPoint;
            return;
        }
        //移动
        swordItem.gameObject.SetActive(true);
        swordItem.transform.localPosition = Vector3.MoveTowards(swordItem.transform.localPosition, EndPoint, TurnSpeed*Time.deltaTime);
        
        float tmpScale = AnimTurnScale.Evaluate(value);
        swordItem.transform.localScale = new Vector3(tmpScale, tmpScale, 1);
    }

    /// <summary>
    /// 发射飞剑
    /// </summary>
    public void Shot()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject swordItem = null;
            if (CurrShowSwordList.Count <= 0)
            {
                swordItem = GetSwordItem();
            }
            else
            {
                swordItem = CurrShowSwordList[0];
                CurrShowSwordList.RemoveAt(0);
                CurveTimeList.RemoveAt(0);
            }
            swordItem.gameObject.SetActive(true);
            PlayShot(swordItem);
        }
        
    }

    /// <summary>
    /// 发射飞剑
    /// </summary>
    public void Create()
    {
        GameObject swordItem = GetSwordItem();
        CurrShowSwordList.Add(swordItem);
        swordItem.transform.localPosition = StartPoint;
        swordItem.SetActive(true);
        PlayIdle();
    }

    #region Cache

    private void PushSwordItemInCache(GameObject item)
    {
        item.gameObject.SetActive(false);
        CacheSwordList.Add(item);
    }

    private GameObject GetSwordItem()
    {
        GameObject item = null;
        if (CacheSwordList.Count <= 0)
        {
            item = Instantiate(SwordItem);
        }
        else
        {
            item = CacheSwordList[0];
            CacheSwordList.RemoveAt(0);
        }
        item.transform.SetParent(SwordRoot.transform);
        item.SetActive(false);
        return item;
    }

    #endregion

    #region Play
    [Header("升空最大水平偏移")]
    public float UpOffsetMaxX = -0.4F;
    [Header("升空最小水平偏移")]
    public float UpOffsetMinX = -0.6F;

    [Header("升空最大垂直偏移")]
    public float UpOffsetMaxY = 0.6F;
    [Header("升空最小垂直偏移")]
    public float UpOffsetMinY = 0.4F;

    [Header("升空花费的时间")]
    public float UpCostTime = 1;

    [Header("升空闲置最大水平偏移")]
    public float UpIdleOffsetMaxX = -0.3F;
    [Header("升空闲置最小水平偏移")]
    public float UpIdleOffsetMinX = -0.5F;

    [Header("升空闲置最大垂直偏移")]
    public float UpIdleOffsetMaxY = 0.2F;
    [Header("升空闲置最小垂直偏移")]
    public float UpIdleOffsetMinY = 0.3F;

    [Header("升空闲置花费的时间")]
    public float UpIdleCostTime = 1;

    [Header("升空闲置最大角度")]
    public float UpIdleMaxAngule = 0.1f;
    [Header("升空闲置最小角度")]
    public float UpIdleMinAngule = -0.1f;

    public void PlayShot(GameObject swordItem)
    {
        //1，升空旋转到目标点
        //2，在空中待机
        PlayUp(swordItem);
    }

    public void PlayUp(GameObject swordItem)
    {
        Vector3 selfPos = new Vector3(swordItem.transform.position.x, swordItem.transform.position.y, 0);
        
        Vector3 upTarget = new Vector3(selfPos.x + Random.Range(UpOffsetMinX, UpOffsetMaxX), selfPos.y + Random.Range(UpOffsetMinY, UpOffsetMaxY), 0);

        swordItem.transform.DOMove(upTarget, UpCostTime);
        float angle = Vector3.SignedAngle(Vector3.up, upTarget, Vector3.forward);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        swordItem.transform.DORotateQuaternion(rotation, UpCostTime).OnComplete(()=> {
            PlayUpIdle(swordItem);
        });
    }

    public void PlayUpIdle(GameObject swordItem)
    {
        Vector3 selfPos = new Vector3(swordItem.transform.position.x, swordItem.transform.position.y, 0);

        Vector3 upTarget = new Vector3(selfPos.x + Random.Range(UpIdleOffsetMinX, UpIdleOffsetMaxX), selfPos.y + Random.Range(UpIdleOffsetMinY, UpIdleOffsetMaxY), 0);
        swordItem.transform.DOMove(upTarget, UpIdleCostTime);
        float angle = Vector3.SignedAngle(Vector3.up, upTarget, Vector3.forward);

        float endAngule = -90 + Random.Range(UpIdleMinAngule,UpIdleMaxAngule);
        Quaternion rotation = Quaternion.Euler(0, 0, endAngule);
        swordItem.transform.DORotateQuaternion(rotation, UpIdleCostTime).OnComplete(()=> {

            Vector3 target = new Vector3(Random.Range(3, 4), -0.4f, 0);
            swordItem.transform.DOMove(target, 0.8f);

            float angle01 = -122 + Random.Range(UpIdleMinAngule, UpIdleMaxAngule);
            Quaternion rotation01 = Quaternion.Euler(0, 0, angle01);
            swordItem.transform.DORotateQuaternion(rotation01, 0.8f);

        });

    }

    [Header("剑池动画曲线")]
    public float SwordIdleTime = 1;
    
    [Header("剑池最多展示几个")]
    public float SwordIdleSize     = 5;
    public void PlayIdle()
    {
        ////动画队列
        //Sequence s = DOTween.Sequence();
        //float delayTime = SwordIdleTime / SwordIdleShowMaxCnt;
        //for (int i = 0; i < SwordIdleShowMaxCnt; i++)
        //{
        //    if (i>CurrShowSwordList.Count-1)
        //        break;
        //    GameObject swordItem = CurrShowSwordList[i];
        //    s.AppendInterval(i * delayTime);
        //    s.AppendCallback(() => PlaySwordIdle(swordItem)).OnComplete(()=> {
        //        swordItem.SetActive(false);
        //    });
        //}
    }

    public void PlaySwordIdle(GameObject swordItem)
    {
        Debug.Log("PlaySwordIdle");
        swordItem.SetActive(false);
        swordItem.transform.localPosition = new Vector3(swordItem.transform.localPosition.x + SwordIdleSize,swordItem.transform.localPosition.y, swordItem.transform.localPosition.z);

        //1,移动
        swordItem.SetActive(true);
        swordItem.transform.DOLocalMoveX(swordItem.transform.position.x - SwordIdleSize, SwordIdleTime);
        //2,前半段放大
        swordItem.transform.DOScale(new Vector3(0.5f,0.5f,1), SwordIdleTime/2).OnComplete(()=> {
            swordItem.transform.DOScale(new Vector3(1, 1, 1), SwordIdleTime / 2);
        });
    }

    #endregion
}
