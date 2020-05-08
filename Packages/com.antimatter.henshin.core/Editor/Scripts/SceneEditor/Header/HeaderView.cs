// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Editor.SceneEditor.GraphArea;
using Henshin.Editor.Skin;
using Henshin.Runtime.Directions.Act;
using Henshin.Runtime.Directions.Scene;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
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
                
                // Check if any value has changed.
                bool needsUpdate = false;
                
                // Draw the application state selector.
                int appIdx = EditorGUILayout.Popup(
                    selectedIndex: header.EditedApplicationIndex,
                    displayedOptions: HeaderState.EditableApplications
                        .Select(selector: app => app.name)
                        .ToArray(),
                        GUILayout.ExpandWidth(expand: false)
                );
                if (appIdx != header.EditedApplicationIndex) {
                    needsUpdate = true;
                    header.EditedApplicationIndex = appIdx;
                }
                
                // Check if the application object is valid.
                if (header.EditedApplication != null) {
                    GUILayout.BeginHorizontal();
                    
                    // Prepare the list of identifiers.
                    string[] actIdentifiers = new string[header.EditedApplication.ActList.Count + 1];
                    
                    // Copy the identifiers into the list.
                    int index = 0;
                    header.EditedApplication.ActList.ForEach(action: act => {
                        actIdentifiers[index] = act.Identifier;
                        index++;
                    });
                    
                    // Add the create option at the end of the list.
                    actIdentifiers[actIdentifiers.Length - 1] = "Add new ...";
                    
                    // Draw the act state selector.
                    int actIdx = EditorGUILayout.Popup(
                        selectedIndex: header.EditedActIndex,
                        displayedOptions: actIdentifiers,
                        GUILayout.ExpandWidth(expand: false)
                    );
                    
                    // Check if the last option was selected.
                    if (actIdx == actIdentifiers.Length - 1) {
                        // Add a new act to the list.
                        header.EditedApplication.ActList.Add(item: new ActState {
                            Index = actIdx
                        });
                        needsUpdate = true;
                    }
                     
                    // If a new act was chosen.
                    if (actIdx != header.EditedActIndex) {
                        needsUpdate = true;
                        header.EditedActIndex = actIdx;
                    }
                    
                    // Draw a button used to delete the act.
                    GUI.enabled = header.EditedApplication.ActList.Count > 1;
                    if (GUILayout.Button(
                        image: SkinState.Textures.Delete, 
                        style: SkinState.Styles.DeleteMini
                    )) {
                        // Delete the act.
                        GraphAreaController.DeleteAct(act: header.EditedAct);
                        
                        // Decrement the index.
                        header.EditedActIndex--;
                        
                        // Update the object.
                        needsUpdate = true;
                    }
                    GUI.enabled = true;
                    
                    GUILayout.EndHorizontal();
                }
                
                // Check if the act object is valid.
                if (header.EditedAct != null) {
                    GUILayout.BeginHorizontal();
                    
                    // Prepare the list of identifiers.
                    string[] sceneIdentifiers = new string[header.EditedAct.SceneList.Count + 1];
                    
                    // Copy the identifiers into the list.
                    int index = 0;
                    header.EditedAct.SceneList.ForEach(action: scene => {
                        sceneIdentifiers[index] = scene.Identifier;
                        index++;
                    });
                    
                    // Add the create option at the end of the list.
                    sceneIdentifiers[sceneIdentifiers.Length - 1] = "Add new ...";
                    
                    // Draw the scene state selector.
                    int scnIdx = EditorGUILayout.Popup(
                        selectedIndex: header.EditedSceneIndex,
                        displayedOptions: sceneIdentifiers,
                        GUILayout.ExpandWidth(expand: false)
                    );
                    
                    // Check if the last option was selected.
                    if (scnIdx == sceneIdentifiers.Length - 1) {
                        // Add a new act to the list.
                        header.EditedAct.SceneList.Add(item: new SceneState {
                            Index = scnIdx,
                            Owner = header.EditedAct                            
                        });
                        needsUpdate = true;
                    }
                    
                    // If the selected scene has changed.
                    if (scnIdx != header.EditedSceneIndex) {
                        needsUpdate = true;
                        header.EditedSceneIndex = scnIdx;
                    }
                    
                    // Draw a button used to delete the scene.
                    GUI.enabled = header.EditedAct.SceneList.Count > 1;
                    if (GUILayout.Button(
                        image: SkinState.Textures.Delete, 
                        style: SkinState.Styles.DeleteMini
                    )) {
                        // Delete the scene.
                        GraphAreaController.DeleteScene(scene: header.EditedScene);
                        
                        // Decrement the index.
                        header.EditedSceneIndex--;
                        
                        // Update the object.
                        needsUpdate = true;
                    }
                    GUI.enabled = true;
                    
                    GUILayout.EndHorizontal();
                }
                
                // If any change was applied.
                if (needsUpdate) {
                    // Reserialize the assets.
                    AssetDatabase.ForceReserializeAssets();
                    
                    // Set the owner's graph area reference.
                    GraphAreaController.ReloadStores();
                    header.Owner.Instance.UpdateGraphArea(
                        graphArea: GraphAreaController.FindGraphArea(sceneState: header.EditedScene)
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
                    
                    // Re-initialize the window.
                    header.Owner.IsInitialized = false;
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
                    
                    // Re-initialize the window.
                    header.Owner.IsInitialized = false;
                    
                    // Set the flag for the current scene.
                    header.EditedScene.IsDebugScene = true;
                    
                    // Enter play mode.
                    UnityEditor.EditorApplication.EnterPlaymode();
                }
                // End the save button area.
                GUILayout.FlexibleSpace(); GUILayout.EndVertical(); 
                
                // Draw a separator.
                HeaderView._DrawSeparator();
                
                // Start a new button area.
                GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
                // Draw the center button.
                if (GUILayout.Button(
                    content: SkinState.Contents.SceneEditorCenter,
                    style: SkinState.Styles.SceneHeaderButton
                )) {
                    // Center the current graph area.
                    if (header.Owner.GraphArea != null) GraphAreaController.Center(graphArea: header.Owner.GraphArea);
                }
                // End the save button area.
                GUILayout.FlexibleSpace(); GUILayout.EndVertical(); 
                
                // Reset the enabled flag.
                GUI.enabled = true;
            }
    // --- /Methods ---
}
}