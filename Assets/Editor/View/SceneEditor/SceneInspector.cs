// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.SceneEditor {

/// <summary>
/// </summary>
public static class SceneInspector {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
            // - Textures -
            private static Texture2D _msHeaderTexture;
            
            // - Contents -
            private static GUIContent _msHeaderContent;
            
            // - Styles -
            private static GUIStyle _msHeaderStyle;
            
            // - Rects -
            private static Rect Rect = new Rect();
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            public static void Initialize() {
                SceneInspector._CreateTextures();
                SceneInspector._CreateContents();
                SceneInspector._CreateStyles();
            }
            
            public static void Render(Rect area) {
                // Get the rect of the scene inspector.
                SceneInspector.Rect.size = new Vector2(x: area.width, y: area.height * 0.3f);
                GUILayout.BeginArea(screenRect: SceneInspector.Rect);
                
                // Get the current scene reference.
                Henshin.State.Directions.Scene current = State.SceneEditor.Header.CurrentScene;
                
                // Draw the header.
                GUILayout.Label(content: SceneInspector._msHeaderContent, style: SceneInspector._msHeaderStyle);
                
                // Draw the scene name.
                GUILayout.Label(text: "Scene Info");
                SceneInspector._DrawField(value: ref current.identifier, label: "Identifier");
                
                // Draw the scene background field.
                GUILayout.BeginHorizontal();
                GUILayout.Label(text: "Background", GUILayout.Width(width: sceneRect.width * .3f));
                Object newObj = UnityEditor.EditorGUILayout.ObjectField(
                    obj: State.SceneEditor.Header.CurrentScene.background,
                    objType: typeof(Sprite),
                    allowSceneObjects: false
                );
                if (newObj is Sprite sprite) {
                    State.SceneEditor.Header.CurrentScene.background = sprite;
                }
                GUILayout.EndHorizontal();
                
                // Draw the scene text fields.
                GUILayout.Space(pixels: 8);
                GUILayout.Label(text: "Gameplay Info");
                GUILayout.BeginHorizontal();
                GUILayout.Label(text: "Mode", GUILayout.Width(width: sceneRect.width * .3f));
                gameplayMode = (Henshin.State.Scenery.TextBox.EMode)UnityEditor.EditorGUILayout.EnumPopup(selected: State.SceneEditor.Header.CurrentScene.gameplayMode);
                GUILayout.EndHorizontal();
                
                if (State.SceneEditor.Header.CurrentScene.gameplayMode != Henshin.State.Scenery.TextBox.EMode.None) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(text: "Original", GUILayout.Width(width: sceneRect.width * .3f));
                    State.SceneEditor.Header.CurrentScene.original = GUILayout.TextField(text: State.SceneEditor.Header.CurrentScene.original);
                    GUILayout.EndHorizontal();
                }
                
                // End the scene editor area.
                GUILayout.EndArea();
            }
            
        // -- Private Methods --
            private static void _CreateTextures() {
                SceneInspector._msHeaderTexture = new Texture2D(width: 1, height: 1, textureFormat: TextureFormat.RGB24, mipChain: false);
                SceneInspector._msHeaderTexture.SetPixel(x: 0, y: 0, color: new Color(r: 0.85f, g: 0.85f, b: 0.85f));
                SceneInspector._msHeaderTexture.wrapMode = TextureWrapMode.Repeat;
                SceneInspector._msHeaderTexture.filterMode = FilterMode.Point;
                SceneInspector._msHeaderTexture.Apply();
            }
            
            private static void _CreateContents() {
                SceneInspector._msHeaderContent = new GUIContent{ text = "Scene Inspector" };
            }
            
            private static void _CreateStyles() {
                SceneInspector._msHeaderStyle = new GUIStyle(other: UnityEditor.EditorStyles.label) {
                    normal = { background = SceneInspector._msHeaderTexture },
                    hover = { background = Texture2D.whiteTexture },
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    padding = { left = 16, top = 2, bottom = 2, right = 16 }
                };
            }
            
            private static void _DrawField<T>(ref T value, string label) {
                GUILayout.BeginHorizontal();
                GUILayout.Label(text: label, GUILayout.Width(width: SceneInspector.Rect.width * .3f));
                switch (typeof(T).Name) {
                    case "string":
                    case "String":
                        value = GUILayout.TextField(text: value as string, GUILayout.ExpandWidth(expand: true)).Tl;
                    default:
                        value = UnityEditor.EditorGUILayout.ObjectField()
                }
                    
                }
                GUILayout.EndHorizontal();
            }
    // --- /Methods ---
}
}