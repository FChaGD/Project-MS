// Editor/TabTypeDrawer.cs
#if UNITY_EDITOR
using SystemDefinition;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TabType))]
public class TabTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 현재 타입 이름 표시
        string currentTypeName = property.managedReferenceValue?.GetType().Name ?? "None";

        Rect labelRect = new Rect(position.x, position.y, position.width - 60, position.height);
        Rect buttonRect = new Rect(position.x + position.width - 58, position.y, 58, position.height);

        EditorGUI.LabelField(labelRect, label.text + $" [{currentTypeName}]");

        if (GUI.Button(buttonRect, "선택"))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("None"), property.managedReferenceValue == null,
                () => { property.managedReferenceValue = null; property.serializedObject.ApplyModifiedProperties(); });

            menu.AddItem(new GUIContent("InventoryTabType"), property.managedReferenceValue is InventoryTabType,
                () => { property.managedReferenceValue = new InventoryTabType("Inventory"); property.serializedObject.ApplyModifiedProperties(); });

            menu.AddItem(new GUIContent("PartyTabType"), property.managedReferenceValue is PartyTabType,
                () => { property.managedReferenceValue = new PartyTabType("Party"); property.serializedObject.ApplyModifiedProperties(); });

            menu.ShowAsContext();
        }

        // 선택된 타입이 있으면 name 필드 표시
        if (property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            SerializedProperty nameProp = property.FindPropertyRelative("name");
            if (nameProp != null)
            {
                Rect nameRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height);
                EditorGUI.PropertyField(nameRect, nameProp);
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue != null)
            return EditorGUIUtility.singleLineHeight * 2 + 4;
        return EditorGUIUtility.singleLineHeight;
    }
}
#endif