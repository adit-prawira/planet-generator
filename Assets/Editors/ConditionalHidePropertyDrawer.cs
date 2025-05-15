using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer: PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute conditionalHideAttribute = (ConditionalHideAttribute)this.attribute;
        bool enabled = GetConditionalHideAttributeResults(conditionalHideAttribute, property);
        if(enabled) EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute conditionalHideAttribute = (ConditionalHideAttribute)this.attribute;
        bool enabled = GetConditionalHideAttributeResults(conditionalHideAttribute, property);
        if(enabled) return EditorGUI.GetPropertyHeight(property, label);
        
        // Undo spacing added before and after the property
        return -EditorGUIUtility.standardVerticalSpacing;
    }

    // Get full relative property of the source field to have ability to hide nested hiding
    // 
    private bool GetConditionalHideAttributeResults(ConditionalHideAttribute conditionalHideAttribute, SerializedProperty property)
    {
        SerializedProperty sourcePropertyValue = null;
        if (!property.isArray)
        {
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, conditionalHideAttribute.conditionalSourceField);
            
            // given find property fails -> fall back to old system
            sourcePropertyValue = property.serializedObject.FindProperty(conditionPath)
                                  ?? property.serializedObject.FindProperty(conditionalHideAttribute.conditionalSourceField);
        }
        else
        {
            sourcePropertyValue = property.serializedObject.FindProperty(conditionalHideAttribute.conditionalSourceField);
        }

        if (sourcePropertyValue != null) return this.CheckPropertyType(conditionalHideAttribute, sourcePropertyValue);
        return true;
    }

    private bool CheckPropertyType(
        ConditionalHideAttribute conditionalHideAttribute,
        SerializedProperty sourcePropertyValue)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue;
            case SerializedPropertyType.Enum:
                return sourcePropertyValue.enumValueIndex == conditionalHideAttribute.enumIndex;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }

}