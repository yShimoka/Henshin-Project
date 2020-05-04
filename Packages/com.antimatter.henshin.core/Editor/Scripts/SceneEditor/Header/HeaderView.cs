// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using System.Linq;
using Henshin.Editor.Skin;
using UnityEngine;

namespace Henshin.Editor.SceneEditor.Header {

/// <summary>
/// View class responsible for the rendering of the specified header state.
/// </summary>
public static class HeaderView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders the specified header object.
            /// </summary>
            /// <param name="header">The header object to render.</param>
            public static void Render(HeaderState header) {
                // Draw the container of the header.
                GUILayout.BeginArea(
                    screenRect: header.Rect, 
                    style: SkinState.Styles.SceneHeaderContents
                );
                
                // If the current event is a repaint of the window.
                if (Event.current.type == EventType.Repaint) {
                    // Update the window's content rect.
                    header.ContentRect = UnityEditor.EditorGUILayout.BeginHorizontal(
                        style: SkinState.Styles.SceneHeaderBackground,
                        options: new [] {
                            GUILayout.ExpandHeight(expand: true),
                            GUILayout.ExpandWidth(expand: true)
                        }
                    );
                } else {
                    // Start a simple horizontal layout.
                    UnityEditor.EditorGUILayout.BeginHorizontal(
                        style: SkinState.Styles.SceneHeaderBackground,
                        options: new [] {
                            GUILayout.ExpandHeight(expand: true),
                            GUILayout.ExpandWidth(expand: true)
                        }
                    );
                }
                
                // Draw the scene selector.
                HeaderView._DrawSceneSelector(header: header);
                
                // Draw the separator.
                HeaderView._DrawSeparator();
                
                // Draw the scene handling buttons.
                HeaderView._DrawButtons(header: header);
                
                // End the header area.
                GUILayout.FlexibleSpace();
                GUILayout.EndArea();
            }
            
        // -- Private Methods --
            // - Renderers -
            /// <summary>
            /// Draws the header separator object.
            /// </summary>
            private static void _DrawSeparator() {
                // Draw the separator object.
                GUILayout.Box(
                    content: GUIContent.none, 
                    style: SkinState.Styles.SceneHeaderSeparator
                );
            }
            
            /// <summary>
            /// Draws the scene selector object.
            /// </summary>
            private static void _DrawSceneSelector(HeaderState header) {
                // Start a new vertical gui layout.
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                
                // Draw the application state selector.
                header.EditedApplicationIndex = UnityEditor.EditorGUILayout.Popup(
                    selectedIndex: header.EditedApplicationIndex,
                    displayedOptions: HeaderState.EditableApplications
                        .Select(selector: app => app.name)
                        .ToArray(),
                        GUILayout.ExpandWidth(expand: false)
                );
                
                // Check if the application object is valid.
                if (header.EditedApplication != null) {
                    // Draw the act state selector.
                    header.EditedActIndex = UnityEditor.EditorGUILayout.Popup(
                        selectedIndex: header.EditedActIndex,
                        displayedOptions: header.EditedApplication.ActList
                            .Select(selector: act => act.Identifier)
                            .ToArray(),
                        GUILayout.ExpandWidth(expand: false)
                    );
                }
                
                // Check if the act object is valid.
                if (header.EditedAct != null) {
                    // Draw the scene state selector.
                    header.EditedSceneIndex = UnityEditor.EditorGUILayout.Popup(
                        selectedIndex: header.EditedSceneIndex,
                        displayedOptions: header.EditedAct.SceneList
                            .Select(selector: scene => scene.Identifier)
                            .ToArray(),
                        GUILayout.ExpandWidth(expand: false)
                    );
                }
                
                // End the act area.
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
            }
            
            /// <summary>
            /// Draws a list of miscellaneous buttons. 
            /// </summary>
            private static void _DrawButtons(HeaderState header) {
                // Check if the scene is valid.
                GUI.enabled = header.EditedScene != null;
                
                // Start a new button area.
                GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
                // Draw the save button.
                if (GUILayout.Button(
                    content: SkinState.Contents.SceneEditorSave,
                    style: SkinState.Styles.SceneHeaderButton
                )) {
                    // Force-reserialize the project's assets.
                    UnityEditor.AssetDatabase.ForceReserializeAssets();
                }
                // End the save button area.
                GUILayout.FlexibleSpace(); GUILayout.EndVertical(); 
                
                
                
                // Start a new button area.
                GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
                // Draw the play button.
                if (GUILayout.Button(
                    content: SkinState.Contents.SceneEditorPlay,
                    style: SkinState.Styles.SceneHeaderButton
                )) {
                    // Force-reserialize the project's assets.
                    UnityEditor.AssetDatabase.ForceReserializeAssets();
                    
                    // Set the flag for the current scene.
                    header.EditedScene.IsDebugScene = true;
                    
                    // Enter play mode.
                    UnityEditor.EditorApplication.EnterPlaymode();
                }
                // End the save button area.
                GUILayout.FlexibleSpace(); GUILayout.EndVertical(); 
                
                // Reset the enabled flag.
                GUI.enabled = true;
            }
    // --- /Methods ---
}
}