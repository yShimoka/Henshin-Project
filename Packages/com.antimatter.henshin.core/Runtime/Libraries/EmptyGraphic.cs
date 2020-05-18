// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Libraries {

/// <summary>
/// Implementation of the <see cref="Graphic"/> class that does not render anything.
/// However, it is still able to receive graphic raycast inputs.
/// </summary>
[RequireComponent(requiredComponent: typeof(RectTransform), requiredComponent2: typeof(CanvasRenderer))]
public abstract class EmptyGraphic: Graphic {
    // ---  Methods ---
        // -- Public Methods --
            /// <inheritdoc cref="Graphic.SetMaterialDirty"/>
            public override void SetMaterialDirty() {}
            
            /// <inheritdoc cref="Graphic.SetVerticesDirty"/>
            public override void SetVerticesDirty() {}
            
        // -- Protected Methods --
            /// <inheritdoc cref="Graphic.OnPopulateMesh(VertexHelper)"/>
            protected override void OnPopulateMesh(VertexHelper vh) { vh.Clear(); }
    // --- /Methods ---
}

#if UNITY_EDITOR
/// <summary>
/// Custom editor implementation used to manipulate <see cref="EmptyGraphic"/> objects.
/// </summary>
[UnityEditor.CanEditMultipleObjects]
[UnityEditor.CustomEditor(inspectedType: typeof(EmptyGraphic), editorForChildClasses: true)]
public class EmptyGraphicInspector: UnityEditor.UI.GraphicEditor {
    // ---  Methods ---
        // -- Public Methods --
            /// <inheritdoc cref="UnityEditor.UI.GraphicEditor.OnInspectorGUI"/>
            public override void OnInspectorGUI() {
                // Update the serialized instance.
                this.serializedObject.Update();
                
                // Draw the script field.
                GUI.enabled = false;
                UnityEditor.EditorGUILayout.PropertyField(property: this.m_Script);
                GUI.enabled = true;
                
                // Draw the raycast target.
                this.RaycastControlsGUI();
                
                // Apply the modifications.
                this.serializedObject.ApplyModifiedProperties();
            }
    // --- /Methods ---
} 
#endif
}