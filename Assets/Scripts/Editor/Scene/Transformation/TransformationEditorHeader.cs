// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Linq;
using Henshin.Core.Scene.Directions;
using Henshin.Editor.App;
using Henshin.Editor.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Scene.Transformation {

/// <summary>
/// 
/// </summary>
public static class TransformationEditorHeader {
    // ---  Attributes ---
        // -- Public Constants --
            /// <summary>Ratio of the act selector to the whole header.</summary>
            public static readonly Vector2 ActSelectorRatio = new Vector2(x: 0.1f, y: 1f);
            
            /// <summary>Ratio of the scene tabs to the whole header.</summary>
            public static readonly Vector2 SceneTabsRatio = new Vector2(x: 1f - TransformationEditorHeader.ActSelectorRatio.x, y: 1f);
            
        // -- Private Attributes --
            /// <summary>Current rect of the header.</summary>
            private static Rect _msHeaderRect;
            
            /// <summary>Position of the scene tabs.</summary>
            private static Vector2 _msSceneTabPosition;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Renders the header.</summary>
            public static void OnGUI() {
                // Get the header rect.
                TransformationEditorHeader._msHeaderRect = TransformationEditorState.GetHeaderRect();
                
                // Draw the act selector.
                TransformationEditorHeader._DrawActSelector();
                
                // Draw the scene tabs.
                TransformationEditorHeader._DrawSceneTabs();
                
                // Draw the lines.
                EditorGUIHelper.VerticalLine  (height: TransformationEditorHeader._msHeaderRect.height, x: TransformationEditorHeader._msHeaderRect.width * TransformationEditorHeader.ActSelectorRatio.x);
                EditorGUIHelper.HorizontalLine(width:  TransformationEditorHeader._msHeaderRect.width,  y: TransformationEditorHeader._msHeaderRect.height - 2, height: 2);
            }
            
        // -- Private Methods --
            // - Rect Helpers -
            /// <returns>The rect of the act selector.</returns>
            private static Rect _GetActSelectorRect() {
                return new Rect {
                    x = TransformationEditorHeader._msHeaderRect.x,
                    y = TransformationEditorHeader._msHeaderRect.y,
                    width  = TransformationEditorHeader._msHeaderRect.width  * TransformationEditorHeader.ActSelectorRatio.x,
                    height = TransformationEditorHeader._msHeaderRect.height * TransformationEditorHeader.ActSelectorRatio.y
                };
            }
            
            /// <returns>The rect of the scene tab section.</returns>
            private static Rect _GetSceneTabsRect() {
                // Compute the tabs rect.
                return new Rect {
                    x = TransformationEditorHeader._msHeaderRect.x + TransformationEditorHeader._msHeaderRect.width * TransformationEditorHeader.ActSelectorRatio.x,
                    y = TransformationEditorHeader._msHeaderRect.y,
                    width  = TransformationEditorHeader._msHeaderRect.width  * TransformationEditorHeader.SceneTabsRatio.x + 1,
                    height = TransformationEditorHeader._msHeaderRect.height * TransformationEditorHeader.SceneTabsRatio.y
                };
            }
            
            // - Drawers -
            /// <summary>Draws the act selector.</summary>
            private static void _DrawActSelector() {
                // Start a new area for the selector.
                GUILayout.BeginArea(screenRect: TransformationEditorHeader._GetActSelectorRect(), style: new GUIStyle{ padding = { left = 2, right = 2, bottom = 2, top = 2 }});
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                
                // Draw the act in a popup.
                TransformationEditorState.CurrentActIndex = EditorGUILayout.Popup(
                    selectedIndex: TransformationEditorState.CurrentActIndex,
                    displayedOptions: ManagerEditorState.CurrentState.acts.Select(selector: act => act.identifier).ToArray()
                );
                
                // Draw a simple refresh button.
                if (GUILayout.Button(image: TextureStore.Refresh, style: EditorStyles.miniButton, GUILayout.Width(width: 32))) {
                    // Set the current act as dirty.
                    EditorUtility.SetDirty(target: TransformationEditorState.CurrentAct);
                }
                
                // End the selector area.
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndArea(); 
            }
            
            /// <summary>Draws the scene tabs.</summary>
            private static void _DrawSceneTabs() {
                // Get the tab rect.
                Rect sceneTabsRect = TransformationEditorHeader._GetSceneTabsRect();
                
                // Start a new area for the tabs.
                GUILayout.BeginArea(
                    screenRect: sceneTabsRect,
                    style: new GUIStyle{ padding = { left = 4, right = 2, bottom = 0, top = 2 }, normal = { background = Texture2D.grayTexture }}
                );
                GUILayout.FlexibleSpace();
                
                // Start a scrollable area.
                Vector2 newPosition = GUILayout.BeginScrollView(
                    scrollPosition: TransformationEditorHeader._msSceneTabPosition,
                    alwaysShowHorizontal: true,
                    alwaysShowVertical: true,
                    horizontalScrollbar: GUIStyle.none, 
                    verticalScrollbar: GUIStyle.none
                );
                GUILayout.BeginHorizontal();
                
                // Update the position of the scroll view.
                TransformationEditorHeader._msSceneTabPosition.x = newPosition.x;
                
                // Loop through the scenes.
                if (TransformationEditorState.CurrentAct != null) {
                    for (int i = 0; i < TransformationEditorState.CurrentAct.scenes.Count; i++) {
                        // Draw that tab.
                        TransformationEditorHeader._DrawTab(sceneIndex: i);
                    } 
                    
                    // Draw the "+" tab.
                    TransformationEditorHeader._DrawTab();
                }
                
                // End the tabs area.
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
            
            /// <summary>
            /// Draws the specified tab.
            /// If the <see cref="sceneIndex"/> is -1, draws the "+" tab.
            /// </summary>
            /// <param name="sceneIndex">The index of the scene to draw.</param>
            private static void _DrawTab(int sceneIndex = -1) {
                // Get the scene object.
                Core.Scene.Directions.Scene currentScene = null;
                if (sceneIndex >= 0) {
                    currentScene = TransformationEditorState.CurrentAct.scenes[index: sceneIndex];
                }
                
                // Draw the tab.
                if (GUILayout.Button(
                    content: new GUIContent {
                        text = currentScene?.identifier,
                        image = currentScene != null ? null : TextureStore.Plus,
                        tooltip = currentScene != null ? "Open scene" : "Add scene"
                    },
                    style: sceneIndex == TransformationEditorState.CurrentSceneIndex ? StyleStore.TabCurrent : StyleStore.TabSelectable,
                    options: currentScene != null ? 
                        new [] { GUILayout.MinWidth(minWidth: 128), GUILayout.MaxWidth(maxWidth: 256) } :
                        new [] { GUILayout.Width(width: 32) }
                )) {
                    // If this is a "real" scene.
                    if (currentScene != null) {
                        // Check the kind of click.
                        switch (Event.current.button) {
                        case 0:
                            // Open the scene.
                            TransformationEditorState.CurrentSceneIndex = sceneIndex;
                            
                            // Update the canvas.
                            TransformationEditorCanvas.OnSceneChanged();
                            
                            break;
                        case 1:
                            // Create the context menu.
                            GenericMenu context = new GenericMenu();
                            
                            // Add the delete item.
                            int index = sceneIndex;
                            context.AddItem(
                                content: new GUIContent{ text = "Delete" }, 
                                on: false, 
                                func: () => TransformationEditorState.CurrentAct.scenes.RemoveAt(index: index)
                            );
                            
                            // Show the menu.
                            context.ShowAsContext();
                            break;
                        }
                    } else {
                        // Create a new scene.
                        TransformationEditorState.CurrentAct.scenes.Add(item: new Core.Scene.Directions.Scene {
                            identifier = $"Scene #{TransformationEditorState.CurrentAct.scenes.Count + 1}"
                        });
                        
                        // Load that scene.
                        TransformationEditorState.CurrentSceneIndex = TransformationEditorState.CurrentAct.scenes.Count - 1;
                            
                        // Update the canvas.
                        TransformationEditorCanvas.OnSceneChanged();
                    }
                }
            }
    // --- /Methods ---
}
}