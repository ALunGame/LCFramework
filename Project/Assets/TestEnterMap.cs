using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LCMap;
using LCECS.Data;
using LCECS;

public class TestEnterMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MapLocate.Map.Enter(1001);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;
        bool dash = false;

        move.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            move.y = 1;
        }

        ParamData paramData = ECSLocate.Player.GetReqParam(RequestId.Move);
        paramData.SetVect2(move);
        ECSLocate.Player.PushPlayerReq(RequestId.Move);
    }
}
