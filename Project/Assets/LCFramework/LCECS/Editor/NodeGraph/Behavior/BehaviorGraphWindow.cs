using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.NodeGraph.Serialize;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using XPToolchains.Help;
using XPToolchains.Json;
using XPToolchains.NodeGraph;

public abstract class BehaviorGraphWindow : BaseGraphWindow
{
    public override string BackVerPath => "Assets/LCFramework/LCECS/Editor/NodeGraph/Behavior/EDData/Back/";

    public override string SavePath => "Assets/LCFramework/LCECS/Editor/NodeGraph/Behavior/EDData/Data/";

    public string DataSavePath => "Assets/" + ECSDefinitionPath.BevTreePath;

    public override void SerializeGraph(Dictionary<string, BaseGraph> graphDict)
    {
        Dictionary<string, Node> treeDict = new Dictionary<string, Node>();
        foreach (var item in graphDict)
        {
            treeDict.Add(item.Value.displayName, GraphSerializeToTree.SerializeToTree(item.Value));
        }
        string dirPath = Path.GetDirectoryName(DataSavePath);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        File.Create(DataSavePath).Dispose();
        string jsonStr = JsonMapper.ToJson(treeDict);

        treeDict = JsonMapper.ToObject<Dictionary<string, Node>>(jsonStr);
        LCIO.WriteText(jsonStr, DataSavePath);
        AssetDatabase.Refresh();
    }
}
