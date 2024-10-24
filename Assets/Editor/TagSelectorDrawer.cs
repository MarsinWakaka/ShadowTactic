using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //���ʹ���������Ե����ַ���
        if (property.propertyType == SerializedPropertyType.String)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use TagSelector with strings.");
        }
    }
}