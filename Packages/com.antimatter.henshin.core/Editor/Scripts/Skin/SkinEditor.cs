// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Skin {

/// <summary>
/// Class used to edit the editor's skin object.
/// The only change is a simple button that refreshes the object.
/// </summary>
[UnityEditor.CustomEditor(inspectedType: typeof(SkinState))]
public class SkinEditor: UnityEditor.Editor {
    // ---  SubObjects ---
        // -- Private Enumerators --
            /// <summary>
            /// Flags used to determine which rect values are editable.
            /// </summary>
            [System.FlagsAttribute]
            private enum ERectFlags {
                X = 0b0001, Y = 0b0010,
                H = 0b0100, W = 0b1000
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>
            /// Flag set if the ratios object is expanded.
            /// </summary>
            private bool _mRatiosObjectExpanded;
            
            /// <summary>
            /// Flags set if the ratio rects are expanded.
            /// </summary>
            [UnityEngine.SerializeField]
            private System.Collections.Generic.List<bool> ExpandedRatios = 
                new System.Collections.Generic.List<bool> { false, false, false }; 
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called when the gui should be rendered.
            /// </summary>
            public override void OnInspectorGUI() {
                // Draw the refresh button.
                if (UnityEngine.GUILayout.Button(
                    content: SkinState.Contents.SkinEditorRefresh, 
                    style: UnityEditor.EditorStyles.miniButton
                )) {
                    // Reload the skin.
                    SkinState.Reload();
                }
                
                // Serialize the skin.
                UnityEditor.SerializedObject skin = new UnityEditor.SerializedObject(obj: this.target);
                UnityEditor.SerializedProperty skinProperty = skin.FindProperty(propertyPath: nameof(SkinState.TextureObject));
                
                // Draw the skin properties.
                do {
                    // If the rendered object is the ratio struct.
                    if (skinProperty.type == nameof(SkinState.RatioStruct)) {
                        // Draw the ratio object.
                        this._DrawRatioObject(ratio: skinProperty);
                    } else {
                        // Draw the full property.
                        UnityEditor.EditorGUILayout.PropertyField(property: skinProperty);
                    }
                } while(skinProperty.Next(enterChildren: false));
                
                // Save the changes.
                skin.ApplyModifiedProperties();
            }
        
        // -- Private Methods --
            /// <summary>
            /// Draws the ratio object.
            /// </summary>
            /// <param name="ratio">The property of the ratio object.</param>
            private void _DrawRatioObject(UnityEditor.SerializedProperty ratio) {
                // Draw the foldout object.
                this._mRatiosObjectExpanded = UnityEditor.EditorGUILayout.Foldout(
                    foldout: this._mRatiosObjectExpanded, 
                    content: "Ratios Object",
                    toggleOnLabelClick: false
                );
                
                // If the object is drawn.
                if (this._mRatiosObjectExpanded) {
                    // Indent slightly.
                    UnityEditor.EditorGUI.indentLevel++;
                    
                    // Draw the header rect.
                    this.ExpandedRatios[index: 0] = UnityEditor.EditorGUILayout.Foldout(
                        foldout: this.ExpandedRatios[index: 0],
                        content: "Header Ratio"
                    );
                    if (this.ExpandedRatios[index: 0]) {
                        this._DrawRectProperty(
                            property: ratio.FindPropertyRelative(
                                relativePropertyPath: nameof(SkinState.RatioStruct.SceneEditorHeaderRatio)
                            ),
                            allowed: ERectFlags.H
                        );
                    }
                    
                    // Draw the inspector rect.
                    this.ExpandedRatios[index: 1] = UnityEditor.EditorGUILayout.Foldout(
                        foldout: this.ExpandedRatios[index: 1],
                        content: "Inspector Ratio"
                    );
                    if (this.ExpandedRatios[index: 1]) {
                        this._DrawRectProperty(
                            property: ratio.FindPropertyRelative(
                                relativePropertyPath: nameof(SkinState.RatioStruct.SceneEditorInspectorRatio)
                            ),
                            allowed: ERectFlags.W
                        );
                    }
                    
                    // Draw the graph area rect.
                    this.ExpandedRatios[index: 2] = UnityEditor.EditorGUILayout.Foldout(
                        foldout: this.ExpandedRatios[index: 2],
                        content: "GraphArea Ratio"
                    );
                    if (this.ExpandedRatios[index: 2]) {
                        this._DrawRectProperty(
                            property: ratio.FindPropertyRelative(
                                relativePropertyPath: nameof(SkinState.RatioStruct.SceneEditorGraphAreaRatio)
                            ),
                            allowed: 0
                        );
                    }
                    
                    // Reset the indent level.
                    UnityEditor.EditorGUI.indentLevel--;
                }
            }
            
            /// <summary>
            /// Draws the specified rect property.
            /// </summary>
            /// <param name="property">The rect property to draw.</param>
            /// <param name="allowed">The sections of the rect that are allowed to change.</param>
            private void _DrawRectProperty(UnityEditor.SerializedProperty property, ERectFlags allowed) {
                // Check if the property is a rect.
                if (property.type == nameof(UnityEngine.Rect)) {
                    // Indent.
                    UnityEditor.EditorGUI.indentLevel++;
                    
                    // Check if the field is enabled.
                    UnityEngine.GUI.enabled = (allowed & ERectFlags.X) != 0;
                    // Get the x property.
                    UnityEditor.SerializedProperty x = property.FindPropertyRelative(relativePropertyPath: "x");
                    // Draw the property.
                    x.floatValue = UnityEditor.EditorGUILayout.FloatField(
                        label: "X",
                        value: x.floatValue
                    );
                    
                    // Check if the field is enabled.
                    UnityEngine.GUI.enabled = (allowed & ERectFlags.Y) != 0;
                    // Get the y property.
                    UnityEditor.SerializedProperty y = property.FindPropertyRelative(relativePropertyPath: "y");
                    // Draw the property.
                    y.floatValue = UnityEditor.EditorGUILayout.FloatField(
                        label: "Y",
                        value: y.floatValue
                    );
                    
                    // Check if the field is enabled.
                    UnityEngine.GUI.enabled = (allowed & ERectFlags.W) != 0;
                    // Get the w property.
                    UnityEditor.SerializedProperty w = property.FindPropertyRelative(relativePropertyPath: "width");
                    // Draw the property.
                    w.floatValue = UnityEditor.EditorGUILayout.FloatField(
                        label: "W",
                        value: w.floatValue
                    );
                    
                    // Check if the field is enabled.
                    UnityEngine.GUI.enabled = (allowed & ERectFlags.H) != 0;
                    // Get the h property.
                    UnityEditor.SerializedProperty h = property.FindPropertyRelative(relativePropertyPath: "height");
                    // Draw the property.
                    h.floatValue = UnityEditor.EditorGUILayout.FloatField(
                        label: "H",
                        value: h.floatValue
                    );
                    
                    // Reset the indent level.
                    UnityEditor.EditorGUI.indentLevel--;
                    // Reset the enabled flag.
                    UnityEngine.GUI.enabled = true;
                }
            }
    // --- /Methods ---
}
}