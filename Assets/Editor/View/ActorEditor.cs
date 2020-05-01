// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View {

/// <summary>
/// 
/// </summary>
[CustomEditor(inspectedType: typeof(Henshin.State.Scenery.Actor))]
public class ActorEditor: UnityEditor.Editor {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>Flag set if the pose list is expanded.</summary>
            private bool _mIsPoseExpanded = true;
            
            // - Textures -
            private static Texture2D _msDeleteTexture;
            private static GUIContent _msDeleteContent;
            private static GUIContent _msAddContent;
            private static GUIStyle _msDeleteStyle;
            
            private static float _msIdentifierWidth;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            private void OnEnable() {
                ActorEditor._msDeleteTexture = Resources.Load<Texture2D>(path: "Editor/Icons/UI_EDITOR_Delete");
                ActorEditor._msDeleteContent = new GUIContent{ image = ActorEditor._msDeleteTexture, tooltip = "Remove this pose." }; 
                ActorEditor._msAddContent = new GUIContent{ text = "Add a new Pose" }; 
            }

            /// <summary>
            /// Renders the inspector.
            /// </summary>
            public override void OnInspectorGUI() {
                // If the style was not created yet.
                if (ActorEditor._msDeleteStyle == null) {
                    ActorEditor._msDeleteStyle = new GUIStyle(other: EditorStyles.miniButton){ fixedWidth = EditorStyles.miniButton.fixedHeight, padding = { left = 0, top = 0, right = 0, bottom = 0 } }; 
                }
                
                // Serialize the target object.
                SerializedObject serialized = new SerializedObject(obj: this.target);
                
                // Render the prefab editor.
                EditorGUILayout.ObjectField(property: serialized.FindProperty(propertyPath: nameof(Henshin.State.Scenery.Actor.actorPrefab)));
                
                // Render the pose editor.
                this._mIsPoseExpanded = EditorGUILayout.Foldout(foldout: this._mIsPoseExpanded, content: "Poses");
                if (this._mIsPoseExpanded) {
                    // Get the object's pose array.    
                    SerializedProperty serializedPoseArray = serialized.FindProperty(propertyPath: nameof(Henshin.State.Scenery.Actor.poses));
                    
                    // Loop through the poses of the actor.
                    int index;
                    for (index = 0; index < serializedPoseArray.arraySize; index++) {
                        // Get the pose at the specified index.
                        SerializedProperty serializedPose = serializedPoseArray.GetArrayElementAtIndex(index: index);
                        
                        // Start an horizontal display.
                        Rect area = EditorGUILayout.BeginHorizontal();
                        if (Event.current.type == EventType.Repaint) {
                            ActorEditor._msIdentifierWidth = area.width / 3f;
                        }

                        // Get the pose's identifier and sprite.
                        SerializedProperty serializedIdentifier = serializedPose.FindPropertyRelative(relativePropertyPath: nameof(Henshin.State.Scenery.Actor.ActorPose.identifier));
                        SerializedProperty serializedSprite    = serializedPose.FindPropertyRelative(relativePropertyPath: nameof(Henshin.State.Scenery.Actor.ActorPose.sprite));
                        
                        // Render the name of the pose.
                        serializedIdentifier.stringValue = EditorGUILayout.TextField(text: serializedIdentifier.stringValue, GUILayout.Width(width: ActorEditor._msIdentifierWidth));
                        
                        // Render the sprite used for the pose.
                        serializedSprite.objectReferenceValue = EditorGUILayout.ObjectField(obj: serializedSprite.objectReferenceValue, objType: typeof(Sprite), allowSceneObjects: false);
                        
                        // Draw the delete button.
                        if (GUILayout.Button(content: ActorEditor._msDeleteContent, style: ActorEditor._msDeleteStyle)) {
                            // Remove the element from the list.
                            serializedPoseArray.DeleteArrayElementAtIndex(index: index);
                        }

                        // End the horizontal display.
                        EditorGUILayout.EndHorizontal();
                        
                        // Add a small amount of space.
                        GUILayout.Space(pixels: 4);
                    }
                    
                    // Add the create pose area.
                    Object newPose = EditorGUILayout.ObjectField(label: ActorEditor._msAddContent, obj: null, objType: typeof(Sprite), allowSceneObjects: false);
                    if (newPose != null) {
                        // Add a new element in the pose array.
                        serializedPoseArray.InsertArrayElementAtIndex(index: index);
                        
                        // Set the value of the element
                        SerializedProperty newSerializedPose = serializedPoseArray.GetArrayElementAtIndex(index: index);
                        newSerializedPose.FindPropertyRelative(relativePropertyPath: nameof(Henshin.State.Scenery.Actor.ActorPose.identifier)).stringValue = newPose.name;
                        newSerializedPose.FindPropertyRelative(relativePropertyPath: nameof(Henshin.State.Scenery.Actor.ActorPose.sprite)).objectReferenceValue = newPose;
                    }
                }
                
                // Save the changes.
                serialized.ApplyModifiedProperties();
            }
    // --- /Methods ---
}
}