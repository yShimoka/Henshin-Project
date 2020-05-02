// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

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
                /// ID of the background layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                // ReSharper disable once MemberHidesStaticFromOuterClass
                public static readonly int Background = UnityEngine.SortingLayer.NameToID(name: "Background");
                
                /// <summary>
                /// ID of the middleground layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                public static readonly int Middleground = UnityEngine.SortingLayer.NameToID(name: "Middleground");
                
                /// <summary>
                /// ID of the foreground layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                public static readonly int Foreground = UnityEngine.SortingLayer.NameToID(name: "Foreground");
                
                /// <summary>
                /// ID of the GUI layer.
                /// </summary>
                // ReSharper disable once InconsistentNaming
                // ReSharper disable once MemberHidesStaticFromOuterClass
                public static readonly int GUI = UnityEngine.SortingLayer.NameToID(name: "Foreground");
            }
        // -- Private Classes --
            /// <summary>
            /// Exception thrown if any problem arose during the creation of the application.
            /// Thrown and caught within the <see cref="ApplicationView.Initialize"/> method.
            /// </summary>
            private class ApplicationCreationException: System.Exception {
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
            public static Application.ApplicationController Root;
            
            /// <summary>
            /// Application stage instance.
            /// All rendered object of the play should be held within this transform.
            /// </summary>
            public static UnityEngine.Transform Stage;
            
            /// <summary>
            /// Application background element.
            /// Image that can be set to anything and will always be rendered behind the main scene.
            /// </summary>
            public static UnityEngine.UI.Image Background;
            
            /// <summary>
            /// Application GUI root.
            /// All elements of the graphical user interface should be rendered below this transform.
            /// </summary>
            // ReSharper disable once InconsistentNaming
            public static UnityEngine.Transform GUI;
            
            /// <summary>
            /// Reference to the <see cref="UnityEngine.Camera"/> that renders the whole application.
            /// </summary>
            public static UnityEngine.Camera WorldCamera;
            
        // -- Private Attributes --
            // - Scene Info -
            /// <summary>
            /// Scene object containing a handler to the current theatre scene.
            /// </summary>
            private static UnityEngine.SceneManagement.Scene _msTheatreScene;
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
                } catch (System.NullReferenceException exception) {
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
                    UnityEditor.EditorUtility.DisplayDialog(
                        title: "An error was thrown during the application's runtime.",
                        message: $"Message: {message}\n\nError details: {details}",
                        ok: "Close" 
                    );
                    // Show an error in the console.
                    UnityEngine.Debug.LogError(message: $"Error thrown: {message}");
                    
                    // Stop the runtime execution.
                    //UnityEditor.EditorApplication.ExitPlaymode();
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
                UnityEngine.SceneManagement.Scene[] loadedScenes = 
                    new UnityEngine.SceneManagement.Scene[UnityEngine.SceneManagement.SceneManager.sceneCount];
                // Get all the currently loaded scenes.
                for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++) {
                    loadedScenes[i] = UnityEngine.SceneManagement.SceneManager.GetSceneAt(index: i);
                }
                
                // Create the theatre scene.
                ApplicationView._msTheatreScene = UnityEngine.SceneManagement.SceneManager.CreateScene(
                    sceneName: "Henshin - Theatre",
                    parameters: new UnityEngine.SceneManagement.CreateSceneParameters {
                        localPhysicsMode = UnityEngine.SceneManagement.LocalPhysicsMode.Physics2D
                    }
                );
                // Set the scene as the currently active one.
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene: ApplicationView._msTheatreScene);
                
                // Unload all the current scenes.
                foreach (UnityEngine.SceneManagement.Scene scene in loadedScenes) {
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene: scene);
                }
                
                // Create the theatre root.
                UnityEngine.GameObject root = new UnityEngine.GameObject(
                    name: "Root", 
                    components: new []{ typeof(Application.ApplicationController )}
                );
                // Load the application controller game object.
                ApplicationView.Root = root.GetComponent<Application.ApplicationController>();
            }
            
            /// <summary>
            /// Creates the camera spectator.
            /// Creates a new <see cref="UnityEngine.Camera"/> instance in the scene.
            /// </summary>
            private static void _CreateSpectator() {
                // Create the spectator game object.
                ApplicationView.WorldCamera = new UnityEngine.GameObject(
                    name: "Spectator", 
                    components: new [] { typeof(UnityEngine.Camera)}
                ).GetComponent<UnityEngine.Camera>();
                
                // Attach the spectator to the root.
                ApplicationView.WorldCamera.transform.SetParent(p: ApplicationView.Root.transform);
                
                // Setup the camera.
                ApplicationView.WorldCamera.orthographic = true;
                if (Application.ApplicationState.Own != null)
                    ApplicationView.WorldCamera.backgroundColor = Application.ApplicationState.Own.ClearColor;
                ApplicationView.WorldCamera.clearFlags = UnityEngine.CameraClearFlags.Color;
                ApplicationView.WorldCamera.orthographicSize = 1f;
                ApplicationView.WorldCamera.farClipPlane = 15f;
                ApplicationView.WorldCamera.nearClipPlane = 5f;
                
                // Move the camera backwards by 10 units.
                ApplicationView.WorldCamera.transform.position = new UnityEngine.Vector3(x: 0f, y: 0f, z: -10f);
            }
            
            /// <summary>
            /// Creates the stage root for the theatre.
            /// </summary>
            private static void _CreateStage() {
                // Create the stage root.
                ApplicationView.Stage = new UnityEngine.GameObject(
                    name: "Stage", 
                    components: new []{ typeof(UnityEngine.Canvas), typeof(UnityEngine.UI.CanvasScaler) }
                ).transform;
                // Attach it to the scene root.
                ApplicationView.Stage.SetParent(p: ApplicationView.Root.transform);
                
                // Set the canvas up.
                UnityEngine.Canvas canvas = ApplicationView.Stage.GetComponent<UnityEngine.Canvas>();
                canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = ApplicationView.WorldCamera;
                canvas.planeDistance = 10f;
                canvas.sortingLayerID = UnityEngine.SortingLayer.NameToID(name: "Middleground");
                
                // Set the canvas scaler up.
                UnityEngine.UI.CanvasScaler scaler = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
                scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.Shrink;
                scaler.referenceResolution = new UnityEngine.Vector2(x: 1920, y: 1080);
                
                // Add the stage background element.
                ApplicationView.Background = new UnityEngine.GameObject(
                    name: "Background", 
                    components: new []{ typeof(UnityEngine.UI.Image), typeof(UnityEngine.Canvas) }
                ).GetComponent<UnityEngine.UI.Image>();
                // Attach it to the stage.
                ApplicationView.Background.transform.SetParent(
                    parent: ApplicationView.Stage.transform,
                    worldPositionStays: false
                );
                
                // Set the image up.
                ApplicationView.Background.preserveAspect = true;
                
                // Set the parameters of the background's rect.
                UnityEngine.RectTransform bgRect = ApplicationView.Background.GetComponent<UnityEngine.RectTransform>();
                bgRect.anchorMin = UnityEngine.Vector2.zero;
                bgRect.anchorMax = UnityEngine.Vector2.one;
                bgRect.pivot = new UnityEngine.Vector2(x: 0.5f, y: 0.5f);
                bgRect.anchoredPosition3D = UnityEngine.Vector3.zero;
                bgRect.sizeDelta = UnityEngine.Vector2.zero;
                
                // Set the parameters of the background's canvas.
                UnityEngine.Canvas bgCanvas = ApplicationView.Background.GetComponent<UnityEngine.Canvas>();
                bgCanvas.overrideSorting = true;
                bgCanvas.sortingLayerID = UnityEngine.SortingLayer.NameToID(name: "Background");
            }
            
            /// <summary>
            /// Creates the GUI root for the theatre.
            /// </summary>
            private static void _CreateGUI() {
                // Create the GUI root.
                ApplicationView.GUI = new UnityEngine.GameObject(
                    name: "GUI", 
                    components: new []{ typeof(UnityEngine.Canvas), typeof(UnityEngine.UI.CanvasScaler) }
                ).transform;
                // Attach it to the scene root.
                ApplicationView.GUI.SetParent(p: ApplicationView.Root.transform);
                
                // Set the canvas up.
                UnityEngine.Canvas canvas = ApplicationView.GUI.GetComponent<UnityEngine.Canvas>();
                canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = ApplicationView.WorldCamera;
                canvas.planeDistance = 10f;
                canvas.sortingLayerID = UnityEngine.SortingLayer.NameToID(name: "GUI");
                
                // Set the canvas scaler up.
                UnityEngine.UI.CanvasScaler scaler = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
                scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.Shrink;
                scaler.referenceResolution = new UnityEngine.Vector2(x: 1920, y: 1080);
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
                UnityEngine.SceneManagement.Scene errorScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(
                    name: "Error"
                ); 
                if (!errorScene.IsValid()) {
                    // Create the error scene.
                    errorScene = UnityEngine.SceneManagement.SceneManager.CreateScene(
                        sceneName: "Error"
                    );
                    
                    // Try to unload the current scene.
                    if (ApplicationView._msTheatreScene.IsValid()) {
                        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene: ApplicationView._msTheatreScene);
                    }
                } else {
                    // Delete the root game objects.
                    foreach (UnityEngine.GameObject gameObject in errorScene.GetRootGameObjects()) {
                        UnityEngine.Object.Destroy(obj: gameObject);
                    }
                }
                
                // Set it as active.
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene: errorScene);
                
                // Create the root game object.
                ApplicationView.Root = new UnityEngine.GameObject(
                    name: "Root",
                    components: new []{ typeof(Application.ApplicationController) }
                ).GetComponent<Application.ApplicationController>();
                
                // Catch any exception thrown by the _CreateSpectator method.
                try {
                    // Create the spectator.
                    ApplicationView._CreateSpectator();
                    
                    // Create the error canvas.
                    UnityEngine.Canvas canvas = new UnityEngine.GameObject(
                        name: "Canvas",
                        components: new []{ typeof(UnityEngine.Canvas), typeof(UnityEngine.UI.CanvasScaler) }
                    ).GetComponent<UnityEngine.Canvas>();
                    
                    // Set the canvas up.
                    canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = ApplicationView.WorldCamera;
                    canvas.planeDistance = 10f;
                    canvas.sortingLayerID = UnityEngine.SortingLayer.NameToID(name: "GUI");
                    
                    // Set the canvas scaler up.
                    UnityEngine.UI.CanvasScaler scaler = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
                    scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.Shrink;
                    scaler.referenceResolution = new UnityEngine.Vector2(x: 1920, y: 1080);
                    
                    // Create the error icon.
                    UnityEngine.UI.Image errorIcon = new UnityEngine.GameObject(
                        name: "Error Icon",
                        components: new []{ typeof(UnityEngine.UI.Image) }
                    ).GetComponent<UnityEngine.UI.Image>();
                    // Attach it to the canvas.
                    errorIcon.transform.SetParent(parent: canvas.transform, worldPositionStays: false);
                    
                    // Prepare the error image.
                    if (ApplicationState.Own != null) errorIcon.sprite = ApplicationState.Own.ErrorIcon;
                    errorIcon.type = UnityEngine.UI.Image.Type.Simple;
                    errorIcon.preserveAspect = true;
                    
                    // Set the image's rect up.
                    UnityEngine.RectTransform errorIconRect = errorIcon.GetComponent<UnityEngine.RectTransform>();
                    errorIconRect.anchorMin = UnityEngine.Vector2.up;
                    errorIconRect.anchorMax = UnityEngine.Vector2.one;
                    errorIconRect.pivot = new UnityEngine.Vector2(x: 0.5f, y: 1f);
                    errorIconRect.sizeDelta = new UnityEngine.Vector2(x: 0f, y: 250f);
                    errorIconRect.anchoredPosition3D = UnityEngine.Vector3.zero;
                    
                    // Create the error message.
                    UnityEngine.UI.Text errorMessage = new UnityEngine.GameObject(
                        name: "Error Message",
                        components: new []{ typeof(UnityEngine.UI.Text) }
                    ).GetComponent<UnityEngine.UI.Text>();
                    // Attach it to the canvas.
                    errorMessage.transform.SetParent(parent: canvas.transform, worldPositionStays: false);
                    
                    // Set the message of the error.
                    errorMessage.text = $"Une erreur est survenue durant la partie: \n{message}";
                    errorMessage.alignment = UnityEngine.TextAnchor.MiddleCenter;
                    errorMessage.fontSize = 64;
                    errorMessage.font = UnityEngine.Font.CreateDynamicFontFromOSFont(fontname: "Futura", size: 64);
                    
                    // Set the text's rect up.
                    UnityEngine.RectTransform errorMessageRect = errorMessage.GetComponent<UnityEngine.RectTransform>();
                    errorMessageRect.anchorMin = UnityEngine.Vector2.zero;
                    errorMessageRect.anchorMax = UnityEngine.Vector2.right;
                    errorMessageRect.pivot = new UnityEngine.Vector2(x: 0.5f, y: 0f);
                    errorMessageRect.sizeDelta = new UnityEngine.Vector2(x: 0f, y: 500f);
                    errorMessageRect.anchoredPosition3D = UnityEngine.Vector3.zero;
                    
                } catch (ApplicationCreationException) {
                    // TODO: Panic.
                    UnityEngine.Debug.LogError(message: "Something terrible has happened ...");
                }
            }
    // --- /Methods ---
}
}