// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Application {

/// <summary>
/// Application view manager.
/// Handles all the rendered elements and GameObjects.
/// </summary>
public static class ApplicationView {
    // ---  SubObjects ---
        // -- Public Enumerators --
            /// <summary>
            /// Class used as a list of all the possible sorting layer identifiers.
            /// </summary>
            public static class SortingLayers {
                /// <summary>
                /// List of all the layer names. 
                /// </summary>
                public static string[] LayerNames = {
                    "Background", "Middleground", "Foreground", "GUI"
                };
                
                /// <summary>
                /// List of all the layer ids.
                /// In the same order as <see cref="LayerNames"/>.
                /// </summary>
                public static int[] LayerIDs = {
                    SortingLayer.NameToID(name: SortingLayers.LayerNames[0]),
                    SortingLayer.NameToID(name: SortingLayers.LayerNames[1]),
                    SortingLayer.NameToID(name: SortingLayers.LayerNames[2]),
                    SortingLayer.NameToID(name: SortingLayers.LayerNames[3])
                };
                
                /// <summary>
                /// ID of the background layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                // ReSharper disable once MemberHidesStaticFromOuterClass
                public static readonly int Background = SortingLayers.LayerIDs[0];
                
                /// <summary>
                /// ID of the middleground layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                public static readonly int Middleground = SortingLayers.LayerIDs[1];
                
                /// <summary>
                /// ID of the foreground layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                public static readonly int Foreground = SortingLayers.LayerIDs[2];
                
                /// <summary>
                /// ID of the GUI layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                // ReSharper disable once MemberHidesStaticFromOuterClass
                public static readonly int GUI = SortingLayers.LayerIDs[3];
            }
        // -- Private Classes --
            /// <summary>
            /// Exception thrown if any problem arose during the creation of the application.
            /// Thrown and caught within the <see cref="ApplicationView.Initialize"/> method.
            /// </summary>
            private class ApplicationCreationException: Exception {
                /// <summary>
                /// Message constructor.
                /// Wraps the base class constructor.
                /// </summary>
                /// <param name="message">The message to display to the user.</param>
                public ApplicationCreationException(string message): base(message: message) {}
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            // - Instances -
            /// <summary>
            /// Application controller instance.
            /// This controller is guaranteed to be on the <see cref="UnityEngine.GameObject"/>
            /// at the root of the scene. 
            /// </summary>
            public static ApplicationController Root;
            
            /// <summary>
            /// Application stage instance.
            /// All rendered object of the play should be held within this transform.
            /// </summary>
            public static Transform Stage;
            
            /// <summary>
            /// Application background element.
            /// Image that can be set to anything and will always be rendered behind the main scene.
            /// </summary>
            public static Image Background;
            
            /// <summary>
            /// Application GUI root.
            /// All elements of the graphical user interface should be rendered below this transform.
            /// </summary>
            // ReSharper disable once InconsistentNaming
            public static Transform GUI;
            
            /// <summary>
            /// Reference to the <see cref="UnityEngine.Camera"/> that renders the whole application.
            /// </summary>
            public static Camera WorldCamera;
            
        // -- Private Attributes --
            // - Scene Info -
            /// <summary>
            /// Scene object containing a handler to the current theatre scene.
            /// </summary>
            private static Scene _msTheatreScene;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Initializes the application.
            /// Creates the theatre scene and loads all the required elements.
            /// </summary>
            public static void Initialize() {
                // Catch any creation exception.
                try {
                    // Create the application scene.
                    ApplicationView._CreateScene();
                    
                    // Create the application spectator.
                    ApplicationView._CreateSpectator();
                    
                    // Create the stage and GUI.
                    ApplicationView._CreateStage();
                    ApplicationView._CreateGUI();
                } catch (ApplicationCreationException exception) {
                    // Show an error.
                    ApplicationView.Error(
                        message: "The application could not initialize itself",
                        details: exception.Message
                    );
                } catch (NullReferenceException exception) {
                    // Show an error.
                    ApplicationView.Error(
                        message: "NullReferenceException in application initialization",
                        details: exception.Message
                    );
                }
            }
            
            /// <summary>
            /// Throws an error.
            /// Renders the error to the screen if the game is in runtime mode,
            /// Otherwise, opens a message box to the developer.
            /// </summary>
            /// <param name="message">Message that gets displayed to the user.</param>
            /// <param name="details">(Optional) Additional details to send to the developer.</param>
            public static void Error(string message, string details = null) {
#if UNITY_EDITOR
                    // Show a text box to the developer.
                    EditorUtility.DisplayDialog(
                        title: "An error was thrown during the application's runtime.",
                        message: $"Message: {message}\n\nError details: {details}",
                        ok: "Close" 
                    );
                    // Show an error in the console.
                    Debug.LogError(message: $"Error thrown: {message}");
                    
                    // Stop the runtime execution.
                    EditorApplication.ExitPlaymode();
#else
                    // Load the error scene.
                    _LoadErrorScene(message: message, details: details);
#endif
            }
            
        // -- Private Methods --
            // - Scene Preparation -
            /// <summary>
            /// Creates the theatre scene.
            /// Unloads any currently loaded scene.
            /// </summary>
            /// <exception cref="ApplicationCreationException">If there is no specified <see cref="ApplicationState"/>.</exception>
            private static void _CreateScene() {
                // Ensure that there is an application state loaded.
                if (ApplicationState.Own == null) {
                    // Throw an exception.
                    throw new ApplicationCreationException(message: "There is no loaded Application state.");
                }
            
                // Prepare the array of all the loaded scenes.
                Scene[] loadedScenes = new Scene[SceneManager.sceneCount];
                // Get all the currently loaded scenes.
                for (int i = 0; i < SceneManager.sceneCount; i++) {
                    loadedScenes[i] = SceneManager.GetSceneAt(index: i);
                }
                
                // Create the theatre scene.
                ApplicationView._msTheatreScene = SceneManager.CreateScene(
                    sceneName: "Henshin - Theatre",
                    parameters: new CreateSceneParameters {
                        localPhysicsMode = LocalPhysicsMode.Physics2D
                    }
                );
                // Set the scene as the currently active one.
                SceneManager.SetActiveScene(scene: ApplicationView._msTheatreScene);
                
                // Unload all the current scenes.
                foreach (Scene scene in loadedScenes) {
                    SceneManager.UnloadSceneAsync(scene: scene);
                }
                
                // Create the theatre root.
                GameObject root = new GameObject(
                    name: "Root", 
                    components: new []{ typeof(ApplicationController) }
                );
                // Load the application controller game object.
                ApplicationView.Root = root.GetComponent<ApplicationController>();
            }
            
            /// <summary>
            /// Creates the camera spectator.
            /// Creates a new <see cref="UnityEngine.Camera"/> instance in the scene.
            /// </summary>
            private static void _CreateSpectator() {
                // Create the spectator game object.
                ApplicationView.WorldCamera = new GameObject(
                    name: "Spectator", 
                    components: new [] { typeof(Camera)}
                ).GetComponent<Camera>();
                
                // Attach the spectator to the root.
                ApplicationView.WorldCamera.transform.SetParent(p: ApplicationView.Root.transform);
                
                // Setup the camera.
                ApplicationView.WorldCamera.orthographic = true;
                if (ApplicationState.Own != null)
                    ApplicationView.WorldCamera.backgroundColor = ApplicationState.Own.ClearColor;
                ApplicationView.WorldCamera.clearFlags = CameraClearFlags.Color;
                ApplicationView.WorldCamera.orthographicSize = 1f;
                ApplicationView.WorldCamera.farClipPlane = 15f;
                ApplicationView.WorldCamera.nearClipPlane = 5f;
                
                // Move the camera backwards by 10 units.
                ApplicationView.WorldCamera.transform.position = new Vector3(x: 0f, y: 0f, z: -10f);
            }
            
            /// <summary>
            /// Creates the stage root for the theatre.
            /// </summary>
            private static void _CreateStage() {
                // Create the stage root.
                ApplicationView.Stage = new GameObject(
                    name: "Stage", 
                    components: new []{ typeof(Canvas), typeof(CanvasScaler) }
                ).transform;
                // Attach it to the scene root.
                ApplicationView.Stage.SetParent(p: ApplicationView.Root.transform);
                
                // Set the canvas up.
                Canvas canvas = ApplicationView.Stage.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = ApplicationView.WorldCamera;
                canvas.planeDistance = 10f;
                canvas.sortingLayerID = SortingLayer.NameToID(name: "Middleground");
                
                // Set the canvas scaler up.
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
                scaler.referenceResolution = new Vector2(x: 1920, y: 1080);
                
                // Add the stage background element.
                ApplicationView.Background = new GameObject(
                    name: "Background", 
                    components: new []{ typeof(Image), typeof(Canvas) }
                ).GetComponent<Image>();
                // Attach it to the stage.
                ApplicationView.Background.transform.SetParent(
                    parent: ApplicationView.Stage.transform,
                    worldPositionStays: false
                );
                
                // Set the image up.
                ApplicationView.Background.preserveAspect = false;
                
                // Set the parameters of the background's rect.
                RectTransform bgRect = ApplicationView.Background.GetComponent<RectTransform>();
                bgRect.anchorMin = Vector2.zero;
                bgRect.anchorMax = Vector2.one;
                bgRect.pivot = new Vector2(x: 0.5f, y: 0.5f);
                bgRect.anchoredPosition3D = Vector3.zero;
                bgRect.sizeDelta = Vector2.zero;
                
                // Set the parameters of the background's canvas.
                Canvas bgCanvas = ApplicationView.Background.GetComponent<Canvas>();
                bgCanvas.overrideSorting = true;
                bgCanvas.sortingLayerID = SortingLayer.NameToID(name: "Background");
            }
            
            /// <summary>
            /// Creates the GUI root for the theatre.
            /// </summary>
            private static void _CreateGUI() {
                // Create the GUI root.
                ApplicationView.GUI = new GameObject(
                    name: "GUI", 
                    components: new []{ typeof(Canvas), typeof(CanvasScaler) }
                ).transform;
                // Attach it to the scene root.
                ApplicationView.GUI.SetParent(p: ApplicationView.Root.transform);
                
                // Set the canvas up.
                Canvas canvas = ApplicationView.GUI.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = ApplicationView.WorldCamera;
                canvas.planeDistance = 10f;
                canvas.sortingLayerID = SortingLayer.NameToID(name: "GUI");
                
                // Set the canvas scaler up.
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
                scaler.referenceResolution = new Vector2(x: 1920, y: 1080);
            }
            
            // - Error Management -
            /// <summary>
            /// Loads and renders the error scene.
            /// This scene is used to communicate info to the end user about the error that just occured.
            /// </summary>
            /// <param name="message">The message to display to the user.</param>
            /// <param name="details">The details of the error.</param>
            private static void _LoadErrorScene(string message, string details) {
                // Check if the error scene is loaded.
                Scene errorScene = SceneManager.GetSceneByName(
                    name: "Error"
                ); 
                if (!errorScene.IsValid()) {
                    // Create the error scene.
                    errorScene = SceneManager.CreateScene(
                        sceneName: "Error"
                    );
                    
                    // Try to unload the current scene.
                    if (ApplicationView._msTheatreScene.IsValid()) {
                        SceneManager.UnloadSceneAsync(scene: ApplicationView._msTheatreScene);
                    }
                } else {
                    // Delete the root game objects.
                    foreach (GameObject gameObject in errorScene.GetRootGameObjects()) {
                        UnityEngine.Object.Destroy(obj: gameObject);
                    }
                }
                
                // Set it as active.
                SceneManager.SetActiveScene(scene: errorScene);
                
                // Create the root game object.
                ApplicationView.Root = new GameObject(
                    name: "Root",
                    components: new []{ typeof(ApplicationController) }
                ).GetComponent<ApplicationController>();
                
                // Catch any exception thrown by the _CreateSpectator method.
                try {
                    // Create the spectator.
                    ApplicationView._CreateSpectator();
                    
                    // Create the error canvas.
                    Canvas canvas = new GameObject(
                        name: "Canvas",
                        components: new []{ typeof(Canvas), typeof(CanvasScaler) }
                    ).GetComponent<Canvas>();
                    
                    // Set the canvas up.
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = ApplicationView.WorldCamera;
                    canvas.planeDistance = 10f;
                    canvas.sortingLayerID = SortingLayer.NameToID(name: "GUI");
                    
                    // Set the canvas scaler up.
                    CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
                    scaler.referenceResolution = new Vector2(x: 1920, y: 1080);
                    
                    // Create the error icon.
                    Image errorIcon = new GameObject(
                        name: "Error Icon",
                        components: new []{ typeof(Image) }
                    ).GetComponent<Image>();
                    // Attach it to the canvas.
                    errorIcon.transform.SetParent(parent: canvas.transform, worldPositionStays: false);
                    
                    // Prepare the error image.
                    if (ApplicationState.Own != null) errorIcon.sprite = ApplicationState.Own.ErrorIcon;
                    errorIcon.type = Image.Type.Simple;
                    errorIcon.preserveAspect = true;
                    
                    // Set the image's rect up.
                    RectTransform errorIconRect = errorIcon.GetComponent<RectTransform>();
                    errorIconRect.anchorMin = Vector2.up;
                    errorIconRect.anchorMax = Vector2.one;
                    errorIconRect.pivot = new Vector2(x: 0.5f, y: 1f);
                    errorIconRect.sizeDelta = new Vector2(x: 0f, y: 250f);
                    errorIconRect.anchoredPosition3D = Vector3.zero;
                    
                    // Create the error message.
                    Text errorMessage = new GameObject(
                        name: "Error Message",
                        components: new []{ typeof(Text) }
                    ).GetComponent<Text>();
                    // Attach it to the canvas.
                    errorMessage.transform.SetParent(parent: canvas.transform, worldPositionStays: false);
                    
                    // Set the message of the error.
                    errorMessage.text = $"Une erreur est survenue durant la partie: \n{message}";
                    errorMessage.alignment = TextAnchor.MiddleCenter;
                    errorMessage.fontSize = 64;
                    errorMessage.font = Font.CreateDynamicFontFromOSFont(fontname: "Futura", size: 64);
                    
                    // Set the text's rect up.
                    RectTransform errorMessageRect = errorMessage.GetComponent<RectTransform>();
                    errorMessageRect.anchorMin = Vector2.zero;
                    errorMessageRect.anchorMax = Vector2.right;
                    errorMessageRect.pivot = new Vector2(x: 0.5f, y: 0f);
                    errorMessageRect.sizeDelta = new Vector2(x: 0f, y: 500f);
                    errorMessageRect.anchoredPosition3D = Vector3.zero;
                    
                } catch (ApplicationCreationException) {
                    // TODO: Panic.
                    Debug.LogError(message: "Something terrible has happened ...");
                }
            }
    // --- /Methods ---
}
}