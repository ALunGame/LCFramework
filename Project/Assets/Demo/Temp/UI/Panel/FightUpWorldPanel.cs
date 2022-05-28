using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LCECS.Core;
using LCToolkit;

/// <summary>
/// 战斗飘字界面
/// </summary>
public class FightUpWorldPanel : LCUI.LCUIPanel
{
    [Header("隐藏时间")]
    public float HideTime = 0.3f;

    [Header("缩放时间")]
    public float ScaleTime  = 0.2f;
    [Header("起始缩放")]
    public float StartScale = 0.5f;
    [Header("结束缩放")]
    public float EndScale   = 0.8f;

    [Header("向上飘动时间")]
    public float MoveUpTime = 0.3f;
    [Header("向上飘动距离")]
    public Vector2 MoveUpPos = new Vector2(4,30);
    [Header("向上飘动曲线")]
    public Ease MoveUpEase = Ease.OutQuad;

    [Header("向下飘动时间")]
    public float MoveDownTime = 0.3f;
    [Header("向下飘动距离")]
    public Vector2 MoveDownPos = new Vector2(8, 20);
    [Header("向下飘动曲线")]
    public Ease MoveDownEase = Ease.InQuad;


    [Header("缓存池数量")]
    public int CacheCnt = 30;
    public RectTransform UpWorldItem;
    public RectTransform UpWorldList;
    private List<RectTransform> activeUpWorldList = new List<RectTransform>();
    private List<RectTransform> cacheUpWorldList = new List<RectTransform>();

    #region Cache

    private void PushUpWorldItemInCache(RectTransform rect)
    {
        cacheUpWorldList.Add(rect);
    }

    private RectTransform GetUpWorldItem()
    {
        RectTransform item = null;
        if (cacheUpWorldList.Count <= 0)
        {
            item = Instantiate(UpWorldItem);
        }
        else
        {
            item = cacheUpWorldList[0];
            cacheUpWorldList.RemoveAt(0);
        }
        item.gameObject.SetActive(false);
        item.SetParent(UpWorldList);
        return item;
    }

    #endregion

    public void PlayUpWorldItem(Entity entity, string str, Sprite sprite = null)
    {
        RectTransform worldItem = GetUpWorldItem();
        worldItem.Find("Img/Str").GetComponent<Text>().text = str;
        if (sprite == null)
        {
            worldItem.Find("Img").GetComponent<Image>().enabled = false;
        }
        else
        {
            worldItem.Find("Img").GetComponent<Image>().enabled = false;
            worldItem.Find("Img").GetComponent<Image>().sprite = sprite;
        }

        ////设置位置
        //GameObjectCom objectCom = entity.GetCom<GameObjectCom>();
        ////Vector2 uiPoint = LCTransform.WorldPointToUI(objectCom.Go.transform.position, Canvas);
        //Vector2 uiPoint = Vector2.zero;
        //worldItem.anchoredPosition3D = uiPoint;
        //worldItem.GetComponent<CanvasGroup>().alpha = 1;
        //worldItem.gameObject.SetActive(true);

        //Vector2 upTarget= new Vector3(worldItem.anchoredPosition.x+ MoveUpPos.x, worldItem.anchoredPosition.y + MoveUpPos.y);

        //Vector2 downTarget = new Vector3(worldItem.anchoredPosition.x + MoveDownPos.x, worldItem.anchoredPosition.y + MoveDownPos.y);

        ////Dotween
        ////1，先放大并向上飘动
        ////2，向下飘动并渐隐
        //worldItem.localScale = new Vector3(StartScale, StartScale, StartScale);
        //worldItem.DOScale(EndScale, ScaleTime);
        //worldItem.DOAnchorPos(upTarget, MoveUpTime).SetEase(MoveUpEase).OnComplete(()=> {
        //    worldItem.DOAnchorPos(downTarget, MoveDownTime).SetEase(MoveDownEase);
        //    worldItem.GetComponent<CanvasGroup>().DOFade(0, HideTime).OnComplete(() => {
        //        worldItem.gameObject.SetActive(false);
        //        worldItem.anchoredPosition3D = uiPoint;
        //        PushUpWorldItemInCache(worldItem);
        //    });
        //});
    }
}
