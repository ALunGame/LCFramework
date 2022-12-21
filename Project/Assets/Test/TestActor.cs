using System;
using System.Collections;
using System.Collections.Generic;
using Demo.Com;
using UnityEngine;
using DG.Tweening;
using LCToolkit;

[ExecuteAlways]
public class TestActor : MonoBehaviour
{
    public Transform displayTrans;
    public Vector3 strength = new Vector3(0.1f,0,0);
    public int vibrato = 1;
    public float randomness = 0;

    public BoxCollider2D collider2D;
    public float ColliderSize = 1;

    [NonSerialized] public ColliderCheckInfo UpCheckInfo;
    [NonSerialized] public ColliderCheckInfo DownCheckInfo;
    [NonSerialized] public ColliderCheckInfo LeftCheckInfo;
    [NonSerialized] public ColliderCheckInfo RightCheckInfo;
        
    //上右拐角
    [NonSerialized] public ColliderCheckInfo UpRightCornerCheckInfo;
    //上左拐角
    [NonSerialized] public ColliderCheckInfo UpLeftCornerCheckInfo;
    //下右拐角
    [NonSerialized] public ColliderCheckInfo DownRightCornerCheckInfo;
    //下左拐角
    [NonSerialized] public ColliderCheckInfo DownLeftCornerCheckInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        UpdateCollider();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shake();
        }
    }

    private void Shake()
    {
        // displayTrans.DOComplete(false);
        // displayTrans.DOPunchPosition(strength, 0.3f, vibrato,0);
        //displayTrans.DOShakePosition(0.3f, strength, vibrato, randomness);
    }
    
    private void UpdateCollider()
    {
        if (collider2D == null || transform == null)
        {
            return;
        }
        Bounds bounds = collider2D.bounds;
            
        //盒子检测
        UpCheckInfo = new ColliderCheckInfo(new Vector2(bounds.extents.x, bounds.size.y - ColliderSize / 2),
            new Vector2(bounds.size.x - ColliderSize*2,ColliderSize));
        DownCheckInfo = new ColliderCheckInfo(new Vector2(bounds.extents.x, ColliderSize / 2),
            new Vector2(bounds.size.x - ColliderSize*2,ColliderSize));
        LeftCheckInfo = new ColliderCheckInfo(new Vector2(ColliderSize / 2, collider2D.bounds.extents.y),
            new Vector2(ColliderSize,bounds.size.y - ColliderSize*2));
        RightCheckInfo = new ColliderCheckInfo(new Vector2(bounds.size.x - ColliderSize / 2, collider2D.bounds.extents.y),
            new Vector2(ColliderSize,bounds.size.y - ColliderSize*2));
            
        //拐角检测
        UpRightCornerCheckInfo = new ColliderCheckInfo(
            new Vector2(bounds.size.x - ColliderSize / 2,bounds.size.y - ColliderSize / 2),
            new Vector2(ColliderSize,ColliderSize));
        UpLeftCornerCheckInfo = new ColliderCheckInfo(
            new Vector2(ColliderSize / 2,bounds.size.y - ColliderSize / 2),
            new Vector2(ColliderSize,ColliderSize));
        DownRightCornerCheckInfo = new ColliderCheckInfo(
            new Vector2(bounds.size.x - ColliderSize / 2, ColliderSize / 2),
            new Vector2(ColliderSize,ColliderSize));
        DownLeftCornerCheckInfo = new ColliderCheckInfo(
            new Vector2(ColliderSize / 2, ColliderSize / 2),
            new Vector2(ColliderSize,ColliderSize));
    }
    
    public Vector2 GetColliderOffset()
    {
        Bounds bounds = collider2D.bounds;
        Vector2 boundRectPos = new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y);
        Vector2 offsetPos = transform.position.ToVector2() - boundRectPos; 
        return offsetPos;
    }

    private void OnDrawGizmosSelected()
    {
        Bounds bounds = collider2D.bounds;
        //Vector2 pos = transform.position.ToVector2();
        //Vector2 pos = bounds.center;
        Vector2 pos = new Vector2(bounds.center.x-bounds.extents.x,bounds.center.y-bounds.extents.y);;
            
        GizmosHelper.DrawBounds(new Bounds(UpCheckInfo.centerPos+pos,UpCheckInfo.size),Color.blue);

        GizmosHelper.DrawBounds(new Bounds(DownCheckInfo.centerPos+pos,DownCheckInfo.size),Color.blue);
            
        GizmosHelper.DrawBounds(new Bounds(LeftCheckInfo.centerPos+pos,LeftCheckInfo.size),Color.blue);
            
        GizmosHelper.DrawBounds(new Bounds(RightCheckInfo.centerPos+pos,RightCheckInfo.size),Color.blue);

        GizmosHelper.DrawBounds(new Bounds(UpRightCornerCheckInfo.centerPos+pos,UpRightCornerCheckInfo.size),Color.red);
        GizmosHelper.DrawBounds(new Bounds(UpLeftCornerCheckInfo.centerPos+pos,UpLeftCornerCheckInfo.size),Color.red);
        GizmosHelper.DrawBounds(new Bounds(DownRightCornerCheckInfo.centerPos+pos,DownRightCornerCheckInfo.size),Color.red);
        GizmosHelper.DrawBounds(new Bounds(DownLeftCornerCheckInfo.centerPos+pos,DownLeftCornerCheckInfo.size),Color.red);
    }
}
