// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.View.Directions {

/// <summary>
/// 
/// </summary>
public static class Scene {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Setter used to update the stage background image.</summary>
            public static Sprite Background { set => Scene.SetStageBackground(bg: value); }
            
            /// <summary>Reference to the TextBox object in the scene.</summary>
            public static Henshin.Controller.Scenery.TextBox TextBox;
            
        // -- Private Attributes --
            /// <summary>Renderer of the background.</summary>
            private static Image _msBackgroundRenderer;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Public Methods --
            public static void CreateSceneObjects() {
                // Create the background renderer.
                GameObject background = new GameObject(name: "Background", components: new[] { typeof(Image), typeof(Canvas) });
                // Attach the background to the stage.
                background.transform.SetParent(parent: Application.Stage.transform, worldPositionStays: false);
                
                // Prepare the background parameters.
                Scene._msBackgroundRenderer = background.GetComponent<Image>();
                RectTransform bgRect = background.GetComponent<RectTransform>();
                bgRect.anchorMin = Vector2.zero;
                bgRect.anchorMax = Vector2.one;
                bgRect.anchoredPosition3D = new Vector3(x: 0f, y: 0f, z: 0f);
                
                // Set up the canvas.
                Canvas bgCanvas = background.GetComponent<Canvas>();
                bgCanvas.overrideSorting = true;
                bgCanvas.sortingLayerID = SortingLayer.NameToID(name: "Background");
                
                // Create the text box object.
                GameObject textBox = new GameObject(name: "Text Box", components: new[] { typeof(Henshin.Controller.Scenery.TextBox) });
                // Attach it to the ui.
                textBox.transform.SetParent(parent: Application.UI.transform, worldPositionStays: false);
                
                // Configure its rect transform properties.
                RectTransform rect = textBox.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.right;
                rect.pivot = Vector2.right / 2f;
                rect.anchoredPosition3D = Vector3.zero;
                // Wait for the camera to be configured before updating the size of the rect.
                Application.AppBehaviour.WaitForFrames(frames: 3, action: () => {
                    rect.sizeDelta = new Vector2(x: -100f, y: Application.CameraSize.y / 3f);
                });
                
                // Query the text box component.
                Scene.TextBox = textBox.GetComponent<Henshin.Controller.Scenery.TextBox>();
            }
            
            public static void UpdateSceneObjects(Henshin.State.Directions.Scene scene) {
                // Update the background item.
                View.Directions.Scene.Background = scene.background;
                
                // Update the text box mode.
                Scene.TextBox.State.Mode = scene.gameplayMode;
            }
            
            public static void SetStageBackground(Sprite bg) {
                // Update the background's sprite.
                Scene._msBackgroundRenderer.sprite = bg;
            }
        // -- Private Methods --
    // --- /Methods ---
}
}