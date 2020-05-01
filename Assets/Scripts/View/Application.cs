// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.View {

/// <summary>
/// View class used for the entire application.
/// Sets up the theatre scene based on the current <see cref="State.Application"/>.
/// </summary>
public static class Application {
    // ---  Types ---
        // -- Private Class --
            /// <summary>
            /// MonoBehaviour attached to the root GameObject of the application. 
            /// </summary>
            public class ApplicationBehaviour: MonoBehaviour {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>Event triggered on each <see cref="Update"/> call.</summary>
                        public readonly UnityEngine.Events.UnityEvent OnUpdate = new UnityEngine.Events.UnityEvent();
                        
                        /// <summary>Event triggered on the next <see cref="Update"/> call.</summary>
                        [NonSerialized]
                        public UnityEngine.Events.UnityEvent OnNextUpdate = new UnityEngine.Events.UnityEvent();
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Unity Events --
                        /// <summary>Unity event fired on every frame.</summary>
                        private void Update() {
                            // Invoke the OnUpdate call.
                            this.OnUpdate.Invoke(); 
                            
                            // Invoke the next update listeners.
                            this.OnNextUpdate.Invoke(); 
                            // Clear the event.
                            this.OnNextUpdate.RemoveAllListeners();
                        }
                        
                    // -- Public Methods --
                        /// <summary>
                        /// Waits for the specified amount of frames before calling the specified action.
                        /// </summary>
                        /// <param name="frames">The number of frames to wait.</param>
                        /// <param name="action">The action to execute.</param>
                        public void WaitForFrames(int frames, UnityEngine.Events.UnityAction action) {
                            // Wrap to the private method.
                            this.StartCoroutine(routine: this._WaitForFrames(frames: frames, action: action));
                        }
                        
                    // -- Private Methods --
                        private System.Collections.IEnumerator _WaitForFrames(int frames, UnityEngine.Events.UnityAction action) {
                            // Wait for the number of required frames.
                            for (int index = 0; index < frames; index++) {
                                yield return new WaitForFixedUpdate();
                            }
                            
                            // Call the action.
                            action.Invoke();
                        }
                // --- /Methods ---
            }
    // --- /Types ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Root of the theatre scene.</summary>
            public static GameObject Root;
            
            /// <summary>Reference to the instance of the <see cref="State.Application.spectatorPrefab"/> in the theatre.</summary>
            public static GameObject Spectator;
            
            /// <summary>Reference to the theatre's stage root object.</summary>
            public static GameObject Stage;
            
            /// <summary>Reference to the theatre's UI root object.</summary>
            // ReSharper disable once InconsistentNaming
            public static GameObject UI;
            
            /// <summary>Size of the camera in pixels.</summary>
            public static Vector2 CameraSize { get; private set; }
            
            /// <summary>Accessor to the application behaviour.</summary>
            public static ApplicationBehaviour AppBehaviour => Application._msRootApplicationBehaviour;
            
        // -- Private Attributes --
            /// <summary>Reference to the camera of the spectator.</summary>
            private static Camera _msCamera;
            
            /// <summary>Reference to the root object's <see cref="ApplicationBehaviour"/>.</summary>
            private static ApplicationBehaviour _msRootApplicationBehaviour;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates the theatre scene.
            /// </summary>
            public static void CreateTheatreScene() {
                // Get the list of all the loaded scenes.
                UnityEngine.SceneManagement.Scene[] loadedScenes = new UnityEngine.SceneManagement.Scene[UnityEngine.SceneManagement.SceneManager.sceneCount];
                for (int sceneIndex = 0; sceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCount; sceneIndex++) {
                    // Set the scene in the array.
                    loadedScenes[sceneIndex] = UnityEngine.SceneManagement.SceneManager.GetSceneAt(index: sceneIndex);
                }
            
                // Create a new scene object.
                UnityEngine.SceneManagement.Scene theatre = UnityEngine.SceneManagement.SceneManager.CreateScene(sceneName: "Theatre", parameters: new UnityEngine.SceneManagement.CreateSceneParameters{ localPhysicsMode = UnityEngine.SceneManagement.LocalPhysicsMode.Physics2D });
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene: theatre);
                
                // Start unloading all the other scenes.
                foreach (UnityEngine.SceneManagement.Scene scene in loadedScenes) { UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene: scene); }
                
                // Create the theatre root.
                Application.Root = new GameObject(name: "Root", typeof(ApplicationBehaviour));
                Application._msRootApplicationBehaviour = Application.Root.GetComponent<ApplicationBehaviour>();
                
                // Create the spectator, stage and UI.
                Application._LoadSpectator();
                Application._PrepareStage();
                Application._PrepareUI();
                
                // Create the scene objects.
                Directions.Scene.CreateSceneObjects();
            }
            
            // - Coroutine Manipulator -
            /// <summary>
            /// Executes the specified method on the next <see cref="UnityEngine.PlayerLoop.Update"/> call.
            /// Uses <see cref="_msRootApplicationBehaviour.StartCoroutine(IEnumerator)"/> internally.
            /// </summary>
            /// <param name="method">The method to execute after a fixed update.</param>
            public static void ExecuteOnNextUpdate(UnityEngine.Events.UnityAction method) {
                // Start the co routine.
                Application._msRootApplicationBehaviour.StartCoroutine(routine: Application._ExecuteOnNextUpdate(method: method));
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
                
                // Load the camera component from the camera.
                if (!(Application.Spectator.GetComponentInChildren<Camera>() is Camera camera)) {
                    // Throw an error.
                    throw Controller.Application.Error(message: "There is no Camera component in the provided Spectator prefab");
                }
                Application._msCamera = camera;
            }
            
            /// <summary>
            /// Prepares the stage of the theatre.
            /// </summary>            
            private static void _PrepareStage() {
                // Create the stage gameobject.
                Application.Stage = new GameObject(name: "Stage", components: new [] { typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler) });
                
                // Setup the canvas object.
                Canvas canvas = Application.Stage.GetComponent<Canvas>();
                canvas.worldCamera = Application.Spectator.GetComponentInChildren<Camera>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.planeDistance = 10;
                canvas.sortingLayerID = SortingLayer.NameToID(name: "Middleground"); 
                
                // Setup the canvas scaler object.
                UnityEngine.UI.CanvasScaler scaler = Application.Stage.GetComponent<UnityEngine.UI.CanvasScaler>();
                scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(x: 1920, y: 1080);
                scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0f;
                scaler.referencePixelsPerUnit = 1080;
                
                // Load the view dimensions.
                // NOTE: We need to wait two frames before the sizeDelta value is correct.
                Application.AppBehaviour.WaitForFrames(frames: 2, action: () => {
                    Application.CameraSize = canvas.GetComponent<RectTransform>().sizeDelta;
                });
                
                // Attach it to the root.
                Application.Stage.transform.SetParent(parent: Application.Root.transform, worldPositionStays: false);
            }
            
            /// <summary>
            /// Prepares the ui root of the theatre.
            /// </summary>            
            private static void _PrepareUI() {
                // Create the ui gameobject.
                Application.UI = new GameObject(name: "UI", components: new [] { typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler) });
                // Attach it to the root.
                Application.UI.transform.SetParent(parent: Application.Root.transform, worldPositionStays: false);
                
                // Setup the canvas object.
                Canvas canvas = Application.UI.GetComponent<Canvas>();
                canvas.worldCamera = Application.Spectator.GetComponentInChildren<Camera>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.planeDistance = 10;
                canvas.sortingLayerID = SortingLayer.NameToID(name: "GUI"); 
                
                // Setup the canvas scaler object.
                UnityEngine.UI.CanvasScaler scaler = Application.UI.GetComponent<UnityEngine.UI.CanvasScaler>();
                scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(x: 1920, y: 1080);
                scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0f;
                scaler.referencePixelsPerUnit = 1080;
            }
            
            /// <summary>
            /// Executes the specified method on the next Update call.
            /// </summary>
            /// <param name="method">The method to execute after a fixed update.</param>
            private static System.Collections.IEnumerator _ExecuteOnNextUpdate(UnityEngine.Events.UnityAction method) {
                // Wait for the next fixed update.
                yield return new WaitForFixedUpdate();
                
                // Invoke the method.
                method.Invoke();
            }
    // --- /Methods ---
}
}