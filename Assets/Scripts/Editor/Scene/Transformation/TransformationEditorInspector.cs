// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using Henshin.Core.Scene.Scenery;
using Henshin.Editor.Misc;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Scene.Transformation {

/// <summary>
/// Draws the inspector on the left side of the <see cref="TransformationEditorWindow"/>.
/// </summary>
public static class TransformationEditorInspector {
    // ---  Attributes ---
        // -- Public Constants --
            /// <summary>Ratio of the header the whole inspector.</summary>
            public static readonly Vector2 HeaderRatio = new Vector2(x: 1f, y: 0.05f);
            
            /// <summary>Ratio of the scene info to the whole inspector.</summary>
            public static readonly Vector2 SceneInfoRatio = new Vector2(x: 1f, y: 0.4f - TransformationEditorInspector.HeaderRatio.y);
            
            /// <summary>Ratio of the transformation info to the whole inspector.</summary>
            public static readonly Vector2 TransformationInfoRatio = new Vector2(x: 1f, y: 1f - TransformationEditorInspector.SceneInfoRatio.y - TransformationEditorInspector.HeaderRatio.y);
            
        // -- Private Attributes --
            /// <summary>Current rect of the inspector.</summary>
            private static Rect _msInspectorRect;
            
            /// <summary>Position of the scene info scrollable area.</summary>
            private static Vector2 _msSceneInfoPosition;
            
            /// <summary>Flag set if the actor sections is expanded.</summary>
            private static bool _msExpandedActors;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Renders the inspector.</summary>
            public static void OnGUI() {
                // Get the rect of the inspector.
                TransformationEditorInspector._msInspectorRect = TransformationEditorState.GetInspectorRect();
                
                // Draw the header.
                TransformationEditorInspector._DrawHeader();
                
                // Draw the scene info.
                TransformationEditorInspector._DrawSceneInfo();
                
                // Draw the transformation info.
                TransformationEditorInspector._DrawTransformationInfo();
                
                // Draw the line at the right side of the inspector.
                EditorGUIHelper.VerticalLine(
                    height: TransformationEditorInspector._msInspectorRect.height, 
                    x: TransformationEditorInspector._msInspectorRect.width - 2, 
                    width: 2, 
                    y: TransformationEditorInspector._msInspectorRect.y
                );
            }
            
        // -- Private Methods --
            // - Rect Helpers -
            /// <returns>The rect of the header section.</returns>
            private static Rect _GetHeaderRect() {
                return new Rect {
                    x = TransformationEditorInspector._msInspectorRect.x,
                    y = TransformationEditorInspector._msInspectorRect.y,
                    width  = TransformationEditorInspector._msInspectorRect.width  * TransformationEditorInspector.HeaderRatio.x,
                    height = TransformationEditorInspector._msInspectorRect.height * TransformationEditorInspector.HeaderRatio.y
                };
            }
            
            /// <returns>The rect of the scene info section.</returns>
            private static Rect _GetSceneInfoRect() {
                return new Rect {
                    x = TransformationEditorInspector._msInspectorRect.x,
                    y = TransformationEditorInspector._msInspectorRect.y + TransformationEditorInspector._msInspectorRect.height * TransformationEditorInspector.HeaderRatio.y,
                    width  = TransformationEditorInspector._msInspectorRect.width  * TransformationEditorInspector.SceneInfoRatio.x,
                    height = TransformationEditorInspector._msInspectorRect.height * TransformationEditorInspector.SceneInfoRatio.y
                };
            }
            
            /// <returns>The rect of the transformation info section.</returns>
            private static Rect _GetTransformationInfoRect() {
                return new Rect {
                    x = TransformationEditorInspector._msInspectorRect.x,
                    y = TransformationEditorInspector._msInspectorRect.y + 
                        TransformationEditorInspector._msInspectorRect.height * TransformationEditorInspector.HeaderRatio.y + 
                        TransformationEditorInspector._msInspectorRect.height * TransformationEditorInspector.SceneInfoRatio.y,
                    width  = TransformationEditorInspector._msInspectorRect.width  * TransformationEditorInspector.TransformationInfoRatio.x,
                    height = TransformationEditorInspector._msInspectorRect.height * TransformationEditorInspector.TransformationInfoRatio.y
                };
            }
            
            // - Drawers -
            /// <summary>Draws the header of the inspector.</summary>
            private static void _DrawHeader() {
                // Get the rect of the header.
                Rect headerRect = TransformationEditorInspector._GetHeaderRect();
                
                // Start a new area.
                GUILayout.BeginArea(screenRect: headerRect);
                
                // Print the header.
                GUILayout.Label(content: new GUIContent{ text = "Transformation Editor Inspector" }, style: StyleStore.PartHeader);
                
                // Print the bottom line.
                EditorGUIHelper.HorizontalLine(width: headerRect.width, y: headerRect.height - 1);
                
                // End the header area.
                GUILayout.EndArea();
            }
            
            /// <summary>Draws the scene info part of the inspector.</summary>
            private static void _DrawSceneInfo() {
                // Get the rect of the scene.
                Rect sceneRect = TransformationEditorInspector._GetSceneInfoRect();
                
                // Start a new area.
                GUILayout.BeginArea(screenRect: sceneRect, style: new GUIStyle{ padding = { left = 4, right = 4, top = 4, bottom = 4 }});
                
                // Print the header.
                GUILayout.Label(content: new GUIContent{ text = "Scene Inspector" }, style: EditorStyles.boldLabel);
                EditorGUIHelper.HorizontalLine(width: sceneRect.width - 8, y: EditorGUIUtility.singleLineHeight, height: 1, x: 4);
                
                // Get the current scene.
                Core.Scene.Directions.Scene current = TransformationEditorState.CurrentScene;
                
                // If the current scene is set.
                if (current != null) {
                    // Begin a scrollable area.
                    TransformationEditorInspector._msSceneInfoPosition = GUILayout.BeginScrollView(scrollPosition: TransformationEditorInspector._msSceneInfoPosition);
                    
                    // Print the scene identifier.
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(label: "Identifier: ", GUILayout.MinWidth(minWidth: 0));
                    string newName = EditorGUILayout.TextField(text: current.identifier);
                    EditorGUILayout.EndHorizontal();
                    
                    // Update the scene name.
                    current.identifier = newName;
                    
                    // Print the scene index.
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(label: "Scene index: ", GUILayout.MinWidth(minWidth: 0));
                    int newIndex = EditorGUILayout.IntField(value: TransformationEditorState.CurrentSceneIndex);
                    EditorGUILayout.EndHorizontal();
                    
                    // Update the scene index.
                    if (
                        newIndex != TransformationEditorState.CurrentSceneIndex &&
                        newIndex >= 0 &&
                        newIndex < TransformationEditorState.CurrentAct.scenes.Count
                    ) {
                        // Remove the scene from the array.
                        TransformationEditorState.CurrentAct.scenes.Remove(item: current);
                        // Insert it back at the specified location.
                        TransformationEditorState.CurrentAct.scenes.Insert(index: newIndex, item: current);
                        // Select the current scene.
                        TransformationEditorState.CurrentSceneIndex = newIndex;
                    }
                    
                    // Draw the number of lines in the scene.
                    EditorGUILayout.LabelField(label: "Number of lines: ", label2: $"{TransformationEditorState.CurrentScene.lines.Count}");
                    
                    // Draw the list of all the actors in the scene.
                    TransformationEditorInspector._msExpandedActors = EditorGUILayout.Foldout(TransformationEditorInspector._msExpandedActors, content: "Actors");
                    if (TransformationEditorInspector._msExpandedActors) {
                        // Draw the list of all the actors.
                        for (int i = 0; i < TransformationEditorState.CurrentScene.actors.Count; i++) {
                            // Get the actor instance.
                            Actor actor = TransformationEditorState.CurrentScene.actors[index: i];
                            
                            // Draw the actor.
                            Actor newActor = EditorGUIHelper.AssetField<Actor>(assetName: actor.name, $"Resources/{Actor.DEFAULT_PATH}");
                            
                            // If the actor was altered.
                            if (actor != newActor) {
                                // If the new actor is removed.
                                if (newActor == null) {
                                    // Clear the value in the array.
                                    TransformationEditorState.CurrentScene.actors.RemoveAt(index: i);
                                } else {
                                    // Update the reference in the array.
                                    TransformationEditorState.CurrentScene.actors[index: i] = newActor;
                                }
                            }
                        }
                        
                        // Add a drop area for new actors.
                        Actor addedActor = EditorGUIHelper.AssetDropArea<Actor>(fieldName: "Add actor: ");
                        if (addedActor != null) {
                            TransformationEditorState.CurrentScene.actors.Add(item: addedActor);
                        }
                    }
                    
                    // Add the delete button.
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(text: "Delete scene", style: new GUIStyle(other: "Button") { margin = { left = 32, right = 32 }})) {
                        // Remove the scene from the array.
                        TransformationEditorState.CurrentAct.scenes.Remove(item: current);
                        // Select the previous scene.
                        TransformationEditorState.CurrentSceneIndex--;
                    }
                    
                    // End the scrollable area.
                    GUILayout.EndScrollView();
                    
                    // Print the bottom line.
                    EditorGUIHelper.HorizontalLine(width: sceneRect.width, y: sceneRect.height - 1);
                } else {
                    // Draw a message.
                    EditorGUILayout.HelpBox(message: "There is no scene in the selected act.", type: MessageType.Info);
                }
                
                // End the header area.
                GUILayout.EndArea();
            }
            
            /// <summary>Draws the transformation info part of the inspector.</summary>
            private static void _DrawTransformationInfo() {
                // Get the rect of the scene.
                Rect inspectorRect = TransformationEditorInspector._GetTransformationInfoRect();
                
                // Start a new area.
                GUILayout.BeginArea(screenRect: inspectorRect, style: new GUIStyle{ padding = { left = 4, right = 4, top = 4, bottom = 4 }});
                
                // Print the header.
                GUILayout.Label(content: new GUIContent{ text = "Transformation Inspector" }, style: EditorStyles.boldLabel);
                EditorGUIHelper.HorizontalLine(width: inspectorRect.width - 8, y: EditorGUIUtility.singleLineHeight, height: 1, x: 4);
                
                // End the header area.
                GUILayout.FlexibleSpace();
                GUILayout.EndArea();
            }
    // --- /Methods ---
}
}