using Demo.Com;
using LCECS.Core;
using System.Collections.Generic;
using UnityEngine.UI;

public class FightPanel : LCUI.LCUIPanel
{
    public Slider HpSlider;
    public Slider EnergySlider;

    public Dictionary<string, float> PlayerAttrConfDict = new Dictionary<string, float>();

    private Entity PlayerEntity = null;
    private AttributeCom PlayerAttrCom = null;

    public override void OnAwake()
    {
        
    }

    private void Init()
    {
        if (PlayerEntity!=null)
        {
            return;
        }
        PlayerEntity = LCECS.ECSLocate.Player.GetPlayerEntity();
        PlayerAttrCom = PlayerEntity.GetCom<AttributeCom>();

        //Dictionary<string, string> attrDict = LCConfigLocate.GetConfigItemDataDict("BaseAttr", PlayerEntity.Id.ToString());
        //foreach (var item in attrDict)
        //{
        //    PlayerAttrConfDict.Add(item.Key, float.Parse(item.Value));
        //}
    }

    public override void OnShow(params object[] parms)
    {

    }

    public override void OnHide()
    {

    }

    private void Update()
    {
        Init();
        if (PlayerAttrCom==null)
        {
            return;
        }
        HpSlider.value = PlayerAttrCom.AttrDict["Hp"] / PlayerAttrConfDict["Hp"];
    }

}
