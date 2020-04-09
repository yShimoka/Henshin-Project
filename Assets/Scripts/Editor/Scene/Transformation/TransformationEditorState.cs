// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using Henshin.Core.Scene.Directions;
using Henshin.Editor.App;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Scene.Transformation {

/// <summary>
/// Stores the state of the <see cref="TransformationEditorWindow"/> object.
/// </summary>
public static class TransformationEditorState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Helper Properties -
            /// <summary>Helper property used to access the current act.</summary>
            public static Act CurrentAct {
                get {
                    // Check if there are acts to return.
                    if (ManagerEditorState.CurrentState.acts.Count == 0) {
                        // Return a null reference.
                        return null;
                    }
                    
                    // Check the bounds of the act list.
                    if (TransformationEditorState.CurrentActIndex < 0 || TransformationEditorState.CurrentActIndex >= ManagerEditorState.CurrentState.acts.Count) {
                        // Reset the index.
                        TransformationEditorState.CurrentActIndex = 0;
                        return TransformationEditorState.CurrentAct; 
                    } else {
                        // Return the act object.
                        return ManagerEditorState.CurrentState.acts[index: TransformationEditorState.CurrentActIndex];
                    }
                }
            }

            /// <summary>Helper property used to access the current scene.</summary>
            public static Core.Scene.Directions.Scene CurrentScene {
                get {
                    // Check if there are scenes to return.
                    if (TransformationEditorState.CurrentAct == null || TransformationEditorState.CurrentAct.scenes.Count == 0) {
                        // Return a null reference.
                        return null;
                    }
                    
                    // Check the bounds of the scene list.
                    if (TransformationEditorState.CurrentSceneIndex < 0 || TransformationEditorState.CurrentSceneIndex >= TransformationEditorState.CurrentAct.scenes.Count) {
                        // Reset the index.
                        TransformationEditorState.CurrentSceneIndex = 0;
                        return TransformationEditorState.CurrentScene; 
                    } else {
                        // Return the scene object.
                        return TransformationEditorState.CurrentAct.scenes[index: TransformationEditorState.CurrentSceneIndex];
                    }
                }
            }
            
            // - Indices -
            /// <summary>The index of the currently selected act.</summary>
            public static int CurrentActIndex;
            
            /// <summary>The index of the currently selected scene.</summary>
            public static int CurrentSceneIndex;
            
            // - Window State -
            /// <summary>Stores the current rect of the <see cref="TransformationEditorWindow"/>.</summary>
            public static Rect WindowRect;
        
        // -- Public Constants --
            // - Ratios -
            /// <summary>Ratio of the header to the full body.</summary>
            public static readonly Vector2 HeaderRatio = new Vector2(x: 1f, y: 0.05f);
            
            /// <summary>Ratio of the inspector to the full body.</summary>
            public static readonly Vector2 InspectorRatio = new Vector2(x: 0.2f, y: 1f - TransformationEditorState.HeaderRatio.y);
            
            /// <summary>Ratio of the canvas to the full body.</summary>
            public static readonly Vector2 CanvasRatio = new Vector2(x: 1f - TransformationEditorState.InspectorRatio.x, y: 1f - TransformationEditorState.HeaderRatio.y);
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - Rect Helpers -
            /// <returns>The expected rect of the header.</returns>
            public static Rect GetHeaderRect() {
                return new Rect {
                    x = 0,
                    y = 0,
                    width  = TransformationEditorState.WindowRect.width  * TransformationEditorState.HeaderRatio.x,
                    height = TransformationEditorState.WindowRect.height * TransformationEditorState.HeaderRatio.y
                };
            }
            
            /// <returns>The expected rect of the inspector.</returns>
            public static Rect GetInspectorRect() {
                return new Rect {
                    x = 0,
                    y = TransformationEditorState.WindowRect.height * TransformationEditorState.HeaderRatio.y,
                    width  = TransformationEditorState.WindowRect.width  * TransformationEditorState.InspectorRatio.x,
                    height = TransformationEditorState.WindowRect.height * TransformationEditorState.InspectorRatio.y
                };
            }
            
            /// <returns>The expected rect of the canvas.</returns>
            public static Rect GetCanvasRect() {
                return new Rect {
                    x = TransformationEditorState.WindowRect.width  * TransformationEditorState.InspectorRatio.x,
                    y = TransformationEditorState.WindowRect.height * TransformationEditorState.HeaderRatio.y,
                    width  = TransformationEditorState.WindowRect.width  * TransformationEditorState.CanvasRatio.x,
                    height = TransformationEditorState.WindowRect.height * TransformationEditorState.CanvasRatio.y
                };
            }
        // -- Private Methods --
    // --- /Methods ---
}
}