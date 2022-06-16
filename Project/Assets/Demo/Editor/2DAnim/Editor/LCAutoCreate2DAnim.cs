using LC2DAnim;
using System.IO;
using UnityEditor;

/// <summary>
/// 自动创建2D动画以及状态机（指定规则）
/// </summary>
public class LCAutoCreate2DAnim
{
    public static string SelGUID = "";

    [MenuItem("Assets/创建2D动画以及状态机")]
    public static void Create2DAnim() 
    {
        LCCreate2DAnimWizard.Open(SelGUID);
    }

    [MenuItem("Assets/创建2D动画以及状态机", true)]
    public static bool Create2DAnimValidate()
    {
        return CheckCanOpenCreate2DAnimPanel();
    }

    private static bool CheckCanOpenCreate2DAnimPanel()
    {
        //只选中一个
        string[] guidArray = Selection.assetGUIDs;
        if (guidArray == null || guidArray.Length!=1)
        {
            return false;
        }

        //必须是文件夹
        string path = AssetDatabase.GUIDToAssetPath(guidArray[0]);
        if (!Directory.Exists(path))
        {
            return false;
        }
        
        SelGUID = guidArray[0];
        
        //目录下必须有Sprite目录
        string selectFloder = AssetDatabase.GUIDToAssetPath(SelGUID);
        string[] childFloder = AssetDatabase.GetSubFolders(selectFloder);
        for (int i = 0; i < childFloder.Length; i++)
        {
            DirectoryInfo temDirInfo = new DirectoryInfo(childFloder[i]);
            if (temDirInfo.Name=="Sprite")
            {
                return true;
            }
        }
        return false;
    }
}
