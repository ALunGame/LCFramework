using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.NodeGraph.Serialize;
using LCHelp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XPToolchains.NodeGraph;

public abstract class DecisionGraphWindow : BaseGraphWindow
{
    public override string BackVerPath => "Assets/LCFramework/LCECS/Editor/NodeGraph/Decision/EDData/Back/";

    public override string SavePath => "Assets/LCFramework/LCECS/Editor/NodeGraph/Decision/EDData/Data/";

    public string DataSavePath => "Assets/" + ECSDefinitionPath.DecTreePath;


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
        string jsonStr = XPToolchains.Json.JsonMapper.ToJson(treeDict);
        LCIO.WriteText(jsonStr, DataSavePath);
        AssetDatabase.Refresh();
    }

    protected override void GetNodeId(BaseGraph graph, BaseNode node)
    {
        string nodeId = Guid.NewGuid().ToString();
        node.id = nodeId;
    }
}
