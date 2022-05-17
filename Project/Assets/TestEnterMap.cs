using Demo;
using LCECS;
using LCECS.Data;
using LCMap;
using UnityEngine;

public class TestEnterMap : MonoBehaviour
{
    public bool LastReqStop = false;

    void Start()
    {
        LCSkill.SkillLocate.SetSkillServer(new SkillServer());
        LCSkill.SkillLocate.SetDamageServer(new DamageServer());    
        MapLocate.Map.Enter(1001);
    }

    ParamData paramData = new ParamData();

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
