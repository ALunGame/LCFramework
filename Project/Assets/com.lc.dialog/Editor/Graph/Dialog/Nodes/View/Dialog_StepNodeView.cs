using LCNode;
using LCNode.Model;
using LCNode.View;
using UnityEditor;
using UnityEngine.UIElements;

namespace LCDialog.DialogGraph
{
    [CustomNodeView(typeof(Dialog_StepNode))]
    public class Dialog_StepNodeView : BaseNodeView
    {
        public readonly VisualElement contents;

        private TextField textField;

        public Dialog_StepNodeView()
        {
            contents = nodeBorder.Q(name: "contents");
            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/com.lc.dialog/Editor/Graph/Dialog/Nodes/View/Style/Dialog_StepNodeView.uss");
            textField = new TextField();
            textField.tooltip = "对话内容";
            textField.styleSheets.Add(style);
            textField.name = "contents_Text";
            contents.Add(textField);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dialog_StepNode node = Model.Model as Dialog_StepNode;
            textField.value = node.content;
            textField.RegisterValueChangedCallback((e) =>
            {
                OnTextContentChange(e.newValue);
            });
        }

        private void OnTextContentChange(string newValue)
        {
            Dialog_StepNode node = Model.Model as Dialog_StepNode;
            Owner.CommandDispacter.Do(new ChangeValueCommand(node.content, newValue,(str) =>
            {
                node.content = str.ToString();
                textField.value = node.content;
            }, (str) => {
                node.content = str.ToString();
                textField.value = node.content;
            }));
        }
    } 
}