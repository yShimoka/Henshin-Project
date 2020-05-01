// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.SceneEditor {

/// <summary>
/// Renders the scene editor <see cref="Window"/>'s header.
/// Computes the rect from the specified ratio.
/// </summary>
public static class Header {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
            // - Constants -
            /// <summary>Level of gray used for the header background.</summary>
            private const float _mBACKGROUND_GRAY_LEVEL = 0.6f;
            /// <summary>Level of gray used for the individual items of the layout.</summary>
            private const float _mCONTENT_GRAY_LEVEL = 0.5f;
            /// <summary>Level of gray used for content's separator.</summary>
            private const float _mCONTENT_SEPARATOR_GRAY_LEVEL = 0.7f;
            
            // - Textures -
            /// <summary>Texture of the header's background.</summary>
            public static Texture2D _msBackgroundTexture;
            /// <summary>Texture of the header's button background.</summary>
            private static Texture2D _msContentBgTexture;
            /// <summary>Texture of the header's button separator.</summary>
            private static Texture2D _msContentSeparatorTexture;
            /// <summary>Texture of the floppy disk used for the save button.</summary>
            private static Texture2D _msSaveTexture;
            /// <summary>Texture of the play button.</summary>
            private static Texture2D _msPlayTexture;
            /// <summary>Texture of the center button.</summary>
            private static Texture2D _msCenterTexture;
            /// <summary>Texture of the scale button.</summary>
            private static Texture2D _msScaleTexture;
            
            // - Contents -
            /// <summary>Content of the save button.</summary>
            private static GUIContent _msSaveContent;
            /// <summary>Content of the play button.</summary>k
            private static GUIContent _msPlayContent;
            /// <summary>Content of the center button.</summary>k
            private static GUIContent _msCenterContent;
            /// <summary>Content of the scale button.</summary>k
            private static GUIContent _msScaleContent;
            
            // - Styles -
            /// <summary>Style of the background texture.</summary>
            private static GUIStyle _msBackgroundStyle;
            /// <summary>Style of the content background texture.</summary>
            private static GUIStyle _msContentBgStyle;
            /// <summary>Style of the content area.</summary>
            private static GUIStyle _msContentAreaStyle;
            /// <summary>Style of the content separator.</summary>
            private static GUIStyle _msContentSeparatorStyle;
            /// <summary>Style of the save button.</summary>
            private static GUIStyle _msSaveStyle;
            /// <summary>Style of the play button.</summary>
            private static GUIStyle _msPlayStyle;
            /// <summary>Style of the center button.</summary>
            private static GUIStyle _msCenterStyle;
            /// <summary>Style of the scale button.</summary>
            private static GUIStyle _msScaleStyle;
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initializes the header's components.</summary>
            public static void Initialize() {
                // Load all the textures of the header.
                Header._CreateTextures();
                
                // Load all the GUIContent objects of the header.
                Header._CreateContents();
                
                // Load all the GUIStyle objects of the header.
                Header._CreateStyles();
            }
            
            /// <summary>Renders the header of the scene editor.</summary>
            public static void Render() {
                // Draw the header's background.
                GUI.Box(position: State.SceneEditor.Header.Rect, content: GUIContent.none, style: Header._msBackgroundStyle);
                
                // Start a new GUI area.
                GUILayout.BeginArea(screenRect: State.SceneEditor.Header.Rect, style: Header._msContentAreaStyle);
                GUILayout.BeginHorizontal(style: Header._msContentBgStyle);
                
                // Draw the current scene selector.
                Header._DrawSceneSelector();
                
                // Draw the separator.
                GUILayout.Box(content: GUIContent.none, style: Header._msContentSeparatorStyle);
                
                // Draw the centered save button.
                GUILayout.BeginVertical(style: Helper.NoWidthExpansion);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(content: Header._msSaveContent, style: Header._msSaveStyle)) {
                    // Save the state of the scene.
                    Controller.Graph.RenderArea.Save(area: State.SceneEditor.Canvas.CurrentRenderArea);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                
                // Draw the centered play button.
                GUILayout.BeginVertical(style: Helper.NoWidthExpansion);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(content: Header._msPlayContent, style: Header._msPlayStyle)) {
                    // Save the state of the scene.
                    Controller.Graph.RenderArea.Save(area: State.SceneEditor.Canvas.CurrentRenderArea);
                    
                    // Unset the test scene on all the scenes.
                    foreach (Henshin.State.Directions.Scene scene in State.SceneEditor.Header.SceneList) {
                        scene.testScene = false;
                    }
                    // Set the scene as the debug one.
                    State.SceneEditor.Header.CurrentScene.testScene = true;
                    
                    // Clear the render area.
                    State.SceneEditor.Canvas.CurrentRenderArea = null;
                    
                    // Start the editor.
                    EditorApplication.EnterPlaymode();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                
                // Draw the separator.
                GUILayout.Box(content: GUIContent.none, style: Header._msContentSeparatorStyle);
                
                // Draw the center button.
                GUILayout.BeginVertical(style: Helper.NoWidthExpansion);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(content: Header._msCenterContent, style: Header._msCenterStyle)) {
                    // Center the render area.
                    Controller.SceneEditor.Canvas.Center(area: State.SceneEditor.Canvas.CurrentRenderArea);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                
                // Draw the scale reset button.
                GUILayout.BeginVertical(style: Helper.NoWidthExpansion);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(content: Header._msScaleContent, style: Header._msScaleStyle)) {
                    // Center the render area.
                    State.SceneEditor.Canvas.CurrentRenderArea.scale = 1f;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                
                // End the header horizontal area.
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
            
        // -- Private Methods --
            // - Initialization -
            /// <summary>
            /// Creates and loads all the required textures of the header.
            /// </summary>
            private static void _CreateTextures() {
                // Load the background texture.
                Header._msBackgroundTexture = new Texture2D(width: 1, height: 1, textureFormat: TextureFormat.RGB24, mipChain: false) {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Point
                };
                Header._msBackgroundTexture.SetPixel(x: 0, y: 0, color: new Color(r: Header._mBACKGROUND_GRAY_LEVEL, g: Header._mBACKGROUND_GRAY_LEVEL, b: Header._mBACKGROUND_GRAY_LEVEL));
                Header._msBackgroundTexture.Apply();
                
                // Load the button background texture.
                Header._msContentBgTexture = Helper.MakeRoundedRectangleTexture(
                    size: new Vector2(x: 128, y: 128), radius: 16, 
                    content: new Color(r: Header._mCONTENT_GRAY_LEVEL, g: Header._mCONTENT_GRAY_LEVEL, b: Header._mCONTENT_GRAY_LEVEL)
                );
                // Load the button separator texture.
                Header._msContentSeparatorTexture = Helper.MakeRoundedRectangleTexture(
                    size: new Vector2(x: 16, y: 128), radius: 8, 
                    content: new Color(r: Header._mCONTENT_SEPARATOR_GRAY_LEVEL, g: Header._mCONTENT_SEPARATOR_GRAY_LEVEL, b: Header._mCONTENT_SEPARATOR_GRAY_LEVEL)
                );
                
                // Load the save texture.
                Header._msSaveTexture = Resources.Load<Texture2D>(path: "Editor/Elements/UI_EDITOR_Save");
                // Load the play texture.
                Header._msPlayTexture = Resources.Load<Texture2D>(path: "Editor/Elements/UI_EDITOR_Play");
                // Load the center texture.
                Header._msCenterTexture = Resources.Load<Texture2D>(path: "Editor/Elements/UI_EDITOR_Center");
                // Load the scale texture.
                Header._msScaleTexture = Resources.Load<Texture2D>(path: "Editor/Elements/UI_EDITOR_Scale");
            }
            
            /// <summary>
            /// Creates and loads all the required <see cref="GUIContent"/>s of the header.
            /// </summary>
            private static void _CreateContents() {
                // Create the save button contents.
                Header._msSaveContent = new GUIContent{ image = Header._msSaveTexture, tooltip = "Save the scene" };
                // Create the play button contents.
                Header._msPlayContent = new GUIContent{ image = Header._msPlayTexture, tooltip = "Play from the current scene" };
                // Create the play button contents.
                Header._msCenterContent = new GUIContent{ image = Header._msCenterTexture, tooltip = "Return to the center of the area" };
                // Create the reset scale button contents.
                Header._msScaleContent = new GUIContent{ image = Header._msScaleTexture, tooltip = "Reset the scale of the area" };
            }
            
            /// <summary>
            /// Creates and loads all the required <see cref="GUIStyle"/>s of the header.
            /// </summary>
            private static void _CreateStyles() {
                // Create the background style.
                Header._msBackgroundStyle = new GUIStyle{ normal = { background = Header._msBackgroundTexture }};
                // Create the button area style.
                Header._msContentAreaStyle = new GUIStyle {
                    padding = { left = 4, top = 4, right = 4, bottom = 4 }
                };
                // Create the button background style.
                Header._msContentBgStyle = new GUIStyle {
                    normal = { background = Header._msContentBgTexture }, 
                    border = { left = 16, top = 16, right = 16, bottom = 16 },
                    padding = { left = 8, top = 8, right = 8, bottom = 8 },
                    stretchHeight = true,
                    stretchWidth = false
                };
                // Create the button separator style.
                Header._msContentSeparatorStyle = new GUIStyle {
                    margin = { left = 8, right = 8 },
                    normal = { background = Header._msContentSeparatorTexture },
                    stretchHeight = true,
                    stretchWidth = false,
                    fixedWidth = 2
                };
                // Create the save button style.
                Header._msSaveStyle = new GUIStyle(other: "button") {
                    padding = { left = 8, top = 4, right = 8, bottom = 4 },
                    clipping = TextClipping.Clip,
                    stretchWidth = false,
                    stretchHeight = false
                };
                // Create the play button style.
                Header._msPlayStyle = new GUIStyle(other: Header._msSaveStyle){ };
                // Create the save button style.
                Header._msCenterStyle = new GUIStyle(other: Header._msSaveStyle) {};
                // Create the scale button style.
                Header._msScaleStyle = new GUIStyle(other: Header._msSaveStyle) {};
            }
            
            // - Renderers -
            /// <summary>
            /// Draws the selector for the current scene object.
            /// </summary>
            private static void _DrawSceneSelector() {
                // Start an horizontal element to push the selector in the center.
                GUILayout.BeginVertical(style: Helper.NoWidthExpansion);
                GUILayout.FlexibleSpace();
                
                // Draw the selector item.
                int sceneIndex = EditorGUILayout.Popup(
                    selectedIndex: State.SceneEditor.Header.CurrentSceneIndex,
                    displayedOptions: State.SceneEditor.Header.SceneNames,
                    GUILayout.ExpandWidth(expand: false)
                );
                
                // Check if the scene index has changed.
                if (sceneIndex != State.SceneEditor.Header.CurrentSceneIndex) {
                    // Update the index of the current scene.
                    State.SceneEditor.Header.CurrentSceneIndex = sceneIndex;
                    
                    // Trigger the change event.
                    State.SceneEditor.Header.OnSceneChange.Invoke();
                }
                
                // End the horizontal element.
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
            }
    // --- /Methods ---
}
}