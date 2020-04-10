// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using Henshin.State;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.View {

/// <summary>
/// View class used for the entire application.
/// Sets up the theatre scene based on the current <see cref="State.Application"/>.
/// </summary>
public static class Application {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Root of the theatre scene.</summary>
            public static GameObject Root;
            
            /// <summary>Reference to the instance of the <see cref="State.Application.spectatorPrefab"/> in the theatre.</summary>
            public static GameObject Spectator;
            
            /// <summary>Reference to the theatre's stage root object.</summary>
            public static GameObject Stage;
            
            /// <summary>Setter used to update the stage background image.</summary>
            public static Sprite Background { set => _SetStageBackground(value); }
            
        // -- Private Attributes --
            private static SpriteRenderer _mBackgroundRenderer;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates the theatre scene.
            /// </summary>
            public static void CreateTheatreScene() {
                // Get the list of all the loaded scenes.
                Scene[] loadedScenes = new Scene[SceneManager.sceneCount];
                for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++) {
                    // Set the scene in the array.
                    loadedScenes[sceneIndex] = SceneManager.GetSceneAt(index: sceneIndex);
                }
            
                // Create a new scene object.
                Scene theatre = SceneManager.CreateScene(sceneName: "Theatre", parameters: new CreateSceneParameters{ localPhysicsMode = LocalPhysicsMode.Physics2D });
                SceneManager.SetActiveScene(scene: theatre);
                
                // Start unloading all the other scenes.
                foreach (Scene scene in loadedScenes) { SceneManager.UnloadSceneAsync(scene: scene); }
                
                // Create the theatre root.
                Application.Root = new GameObject(name: "Root");
                
                // Create the spectator and stage.
                Application._LoadSpectator();
                Application._PrepareStage();
            }
            
        // -- Private Methods --
            /// <summary>
            /// Loads the <see cref="State.Application.spectatorPrefab"/> into the theatre scene.
            /// </summary>
            private static void _LoadSpectator() {
                // Ensure that the prefab instance was assigned.
                if (State.Application.Current.spectatorPrefab == null) {
                    throw Controller.Application.Error(message: $"There was no spectator prefab in the application state named #\"{State.Application.Current.identifier}\"");
                }
            
                // Create the spectator prefab object.
                Application.Spectator = Object.Instantiate(
                    original: State.Application.Current.spectatorPrefab,
                    parent: Application.Root.transform,
                    position: Vector3.zero,
                    rotation: Quaternion.identity
                );
                // Set the name of the spectator.
                Application.Spectator.name = "Spectator";
            }
            
            /// <summary>
            /// Prepares the stage of the theatre.
            /// </summary>            
            private static void _PrepareStage() {
                // Create the stage gameobject.
                Application.Stage = new GameObject(name: "Stage", components: new [] { typeof(Canvas), typeof(CanvasScaler) });
                
                // Setup the canvas object.
                Canvas canvas = Application.Stage.GetComponent<Canvas>();
                canvas.worldCamera = Application.Spectator.GetComponentInChildren<Camera>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.planeDistance = 10;
                canvas.sortingLayerID = SortingLayer.NameToID(name: "Middleground"); 
                
                // Setup the canvas scaler object.
                CanvasScaler scaler = Application.Stage.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(x: 1920, y: 1080);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
                scaler.referencePixelsPerUnit = 1080;
                
                // Attach it to the root.
                Application.Stage.transform.SetParent(parent: Application.Root.transform, worldPositionStays: false);
                
                
                // Create the background renderer.
                GameObject background = new GameObject(name: "Background", components: new[] { typeof(SpriteRenderer) });
                
                // Prepare the background parameters.
                Application._mBackgroundRenderer = background.GetComponent<SpriteRenderer>();
                Application._mBackgroundRenderer.sortingLayerID = SortingLayer.NameToID(name: "Background");
                
                // Attach the background to the stage.
                background.transform.SetParent(parent: Application.Stage.transform, worldPositionStays: false);
            }
            
        // -- Private Methods --
            /// <summary>
            /// Updates the background of the stage.
            /// </summary>
            /// <param name="bg">The background of the stage.</param>
            private static void _SetStageBackground(Sprite bg) {
                // Update the background's sprite.
                Application._mBackgroundRenderer.sprite = bg;
            }
    // --- /Methods ---
}
}