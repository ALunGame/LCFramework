using Demo;
using LCECS;
using LCECS.Data;
using LCMap;
using System;
using UnityEngine;


public class KeyClick
{
    public KeyCode key;
    public float lastClickTime;

    public float doubleClickTime = 0.3f;

    private bool eventUsed = false;

    private Action downCallBack;
    private Action clickCallBack;
    private Action doubleClickCallBack;
    private Action upCallBack;

    public KeyClick(KeyCode key)
    {
        this.key = key;
    }

    public void CheckEvent()
    {
        if (Input.GetKeyDown(key))
        {
            if (!eventUsed)
            {
                downCallBack?.Invoke();
            }
        }
        if (Input.GetKeyUp(key))
        {
            upCallBack?.Invoke();
            if (Time.realtimeSinceStartup - lastClickTime < doubleClickTime)
            {
                eventUsed = true;
                doubleClickCallBack?.Invoke();
            }
            lastClickTime = Time.realtimeSinceStartup;
            if (!eventUsed)
            {
                clickCallBack?.Invoke();
            }
        }
    }

    public void SetDownCallBack(Action downCallBack)
    {
        this.downCallBack = downCallBack;
    }

    public void SetUpCallBack(Action upCallBack)
    {
        this.upCallBack = downCallBack;
    }

    public void SetClickCallBack(Action clickCallBack)
    {
        this.clickCallBack = clickCallBack;
    }

    public void SetDoubleClickCallBack(Action doubleClickCallBack,float doubleTime = 0.3f)
    {
        this.doubleClickCallBack = doubleClickCallBack;
        doubleClickTime = doubleTime;
    }
}

public class TestEnterMap : MonoBehaviour
{
    public bool LastReqStop = false;

    private ParamData paramData = new ParamData();
    private Vector2 moveParam   = Vector2.zero;

    public KeyClick LeftKey  = new KeyClick(KeyCode.A);
    public KeyClick RightKey = new KeyClick(KeyCode.D);
    public KeyClick JumpKey  = new KeyClick(KeyCode.Space);

    void Start()
    {
        LCSkill.SkillLocate.SetSkillServer(new SkillServer());
        LCSkill.SkillLocate.SetDamageServer(new DamageServer());    
        MapLocate.Map.Enter(1001,(() =>
        {
            InitKeyCallBack();
        }));
       
    }

    private void InitKeyCallBack()
    {
        LeftKey.SetDoubleClickCallBack(() => {
            Debug.LogError("双击LeftKey》》》》");
            paramData.SetString("100102");
            ECSLocate.Player.PushPlayerReq(RequestId.PushSkill, paramData);
        });
        RightKey.SetDoubleClickCallBack(() => {
            Debug.LogError("双击RightKey》》》》");
            paramData.SetString("100102");
            ECSLocate.Player.PushPlayerReq(RequestId.PushSkill, paramData);
        });
    }


    void Update()
    {
        LeftKey.CheckEvent();
        RightKey.CheckEvent();


        Input.GetKeyDown(KeyCode.A);
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            move.y = 1;
        }

        paramData.SetVect2(move);
        ECSLocate.Player.PushPlayerReq(RequestId.Move, paramData);


        if (Input.GetMouseButtonDown(0))
        {
            paramData.SetString("100101");
            ECSLocate.Player.PushPlayerReq(RequestId.PushSkill, paramData);
        }
    }
}
