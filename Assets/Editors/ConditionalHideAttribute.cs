using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute: PropertyAttribute
{
        public string conditionalSourceField;
        public int enumIndex;

        public ConditionalHideAttribute(string booleanVariableName)
        {
                this.conditionalSourceField = booleanVariableName;
        }

        public ConditionalHideAttribute(string booleanVariableName, int enumIndex)
        {
                this.conditionalSourceField = booleanVariableName;
                this.enumIndex = enumIndex;
        }
}