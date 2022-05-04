using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LCMap;
using LCECS.Data;
using LCECS;

public class TestEnterMap : MonoBehaviour
{
    public bool LastReqStop = false;

    // Start is called before the first frame update
    void Start()
    {
        MapLocate.Map.Enter(1001);
    }

    ParamData paramData = new ParamData();
    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            move.y = 1;
        }

        paramData.SetVect2(move);
        ECSLocate.Player.PushPlayerReq(RequestId.Move, paramData);

        //if (move == Vector2.zero)
        //{
        //    if (LastReqStop)
        //        return;
        //    LastReqStop = true;
        //    paramData.SetVect2(move);
        //    ECSLocate.Player.PushPlayerReq(RequestId.Move, paramData);
        //}
        //else
        //{
        //    LastReqStop = false;
        //    paramData.SetVect2(move);

        //    if (move.y == 1)
        //    {
        //        Debug.LogWarning("ÌøÔ¾£¡£¡£¡£¡£¡£¡£¡£¡£¡£¡£¡");
        //    }
        //    ECSLocate.Player.PushPlayerReq(RequestId.Move, paramData);
        //}
    }
}
