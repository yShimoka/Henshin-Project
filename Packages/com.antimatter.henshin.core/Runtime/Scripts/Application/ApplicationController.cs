// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;

/* Wrap the class within the local namespace. */
namespace Runtime.Application {

/// <summary>
/// Class used for the <see cref="ApplicationController"/>'s Tick delegates.
/// </summary>
public class TickEvent: UnityEngine.Events.UnityEvent<float> {}

/// <summary>
/// Controller class used to alter the state of the application.
/// </summary>
[UnityEngine.AddComponentMenu(menuName: "Henshin/Application Controller"), UnityEngine.DisallowMultipleComponent]
public class ApplicationController: UnityEngine.MonoBehaviour {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Event that is fired on every tick of the application.
            /// </summary>
            public static TickEvent OnTick = new TickEvent();
            
            /// <summary>
            /// Event that is fired on every tick of the application.
            /// The delegate gets cleared after each frame. 
            /// </summary>
            public static TickEvent OnNextTick = new TickEvent();
        // -- Private Attributes --
            /// <summary>
            /// Reference to the instance that exists in the scene.
            /// </summary>
            private static ApplicationController _msApplicationInstance;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Method called upon the start of the runtime application.
            /// Loads the current application state and runs it.
            /// </summary>
            [UnityEngine.RuntimeInitializeOnLoadMethodAttribute]
            private static void _Initialize() {
#if UNITY_EDITOR
                // Seek the debug application state.
                Application.ApplicationState.Own = UnityEngine.Resources
                    .LoadAll<Application.ApplicationState>(path: "")
                    .FirstOrDefault(predicate: state => state.IsDebugState);
#else
                // Seek the non-debug application state.
                Application.State.Own = UnityEngine.Resources
                    .LoadAll<Application.State>(path: "Data/States")
                    .FirstOrDefault(predicate: state => !state.IsDebugState);
#endif
                // Ensure that the application was found.
                if (ApplicationState.Own == null) {
                    ApplicationView.Error(message: "Could not find an application to load !");
                } else {
                    // Initialize the view.
                    Application.ApplicationView.Initialize();
                
#if UNITY_EDITOR
                    // Search for a debugged scene.
                    Runtime.Directions.Scene.SceneState debugged = ApplicationState.Own.ActList
                        .SelectMany(selector: act => act.SceneList)
                        .Where(predicate: scene => scene != null)
                        .FirstOrDefault(predicate: scene => scene.IsDebugScene);
                        
                    // If a debug scene was found.
                    if (debugged != null) {
                        // Play that scene.
                        Runtime.Directions.Scene.SceneController.Play(scene: debugged);
                        
                        // Stop the method.
                        return;
                    }
#endif
                    // Check if the act list is set.
                    if (ApplicationState.Own.CurrentAct != null) {
                        // Play the first act.
                        Runtime.Directions.Act.ActController.Play(act: ApplicationState.Own.CurrentAct);
                    } else {
                        // Throw an error.
                        ApplicationView.Error(message: "The application has no act to play !");
                    }
                }
            }
            
            /// <summary>
            /// Event fired when the instance of the controller is created.
            /// Stores the instance in the static reference. 
            /// </summary>
            private void Awake() { ApplicationController._msApplicationInstance = this; }
            
            /// <summary>
            /// Event fired on every frame of the application.
            /// Calls the <see cref="OnTick"/> and <see cref="OnNextTick"/> delegates.
            /// </summary>
            private void Update() {
                // CAll the ON_TICK events.
                ApplicationController.OnTick.Invoke(arg0: UnityEngine.Time.deltaTime);
                
                // Copy the ON_NEXT_TICK event.
                TickEvent nextTickCopy = ApplicationController.OnNextTick;
                // Clear the ON_NEXT_TICK delegate.
                ApplicationController.OnNextTick = new TickEvent();
                // Call the ON_NEXT_TICK copy.
                nextTickCopy.Invoke(arg0: UnityEngine.Time.deltaTime);
            }

        // -- Public Methods --
            // - Serialization Events -
            /// <summary>
            /// Serializes the specified state instance.
            /// Calls the <see cref="Runtime.Directions.Act.ActController.Serialize"/> method on all the acts.
            /// </summary>
            /// <param name="state">The state object to serialize.</param>
            public static void Serialize(ApplicationState state) {
                // Serialize all the acts.
                foreach (Runtime.Directions.Act.ActState actState in state.ActList) {
                    Runtime.Directions.Act.ActController.Serialize(owner: state, act: actState);
                }
            }
            
            /// <summary>
            /// Deserializes the specified state instance.
            /// Calls the <see cref="Runtime.Directions.Act.ActController.Deserialize"/> method on all the acts.
            /// </summary>
            /// <param name="state">The state object to deserialize.</param>
            public static void Deserialize(ApplicationState state) {
                // Deserialize all the acts.
                foreach (Runtime.Directions.Act.ActState actState in state.ActList) {
                    Runtime.Directions.Act.ActController.Deserialize(owner: state, act: actState);
                }
            }
            
            // - Delay Methods -
            /// <summary>
            /// Waits for the specified amount of frames.
            /// Calls the specified action after the specified frames have passed.
            /// </summary>
            /// <param name="frames">The number of frames to wait.</param>
            /// <param name="action">The action to call once the frames have passed.</param>
            public static void WaitForFrames(uint frames, UnityEngine.Events.UnityAction action) {
                // Start the wait coroutine.
                ApplicationController._msApplicationInstance.StartCoroutine(
                    // Call the instance's method.
                    routine: ApplicationController._msApplicationInstance._WaitForFrames(frames: frames, action: action)
                );
            }
            
            // - Act Manipulation -
            /// <summary>
            /// Advances onto the next act in the application's state.
            /// </summary>
            public static void NextAct() {
                // Check if the application instance is set.
                if (ApplicationState.Own != null) {
                    // Increment the act counter.
                    ApplicationState.Own.CurrentActIndex++;
                    
                    // Check if the act is valid.
                    if (ApplicationState.Own.CurrentAct != null) {
                        // Play the specified act.
                        Runtime.Directions.Act.ActController.Play(act: ApplicationState.Own.CurrentAct);
                    } else {
                        // STUB: Throw an error.
                        ApplicationView.Error(message: "Reached the end of the game !");
                    }
                } else {
                    // Throw an error.
                    ApplicationView.Error(message: "Tried to advance to the next act but no Application is loaded.");
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Waits for the specified amount of frames.
            /// Calls the specified action after the specified frames have passed.
            /// </summary>
            /// <param name="frames">The number of frames to wait.</param>
            /// <param name="action">The action to call once the frames have passed.</param>
            private System.Collections.IEnumerator _WaitForFrames(uint frames, UnityEngine.Events.UnityAction action) {
                // Wait for the specified number of frames.
                for (uint i = 0; i < frames; i++) {
                    yield return new UnityEngine.WaitForFixedUpdate();
                }
                
                // Invoke the action.
                action.Invoke();
            }
    // --- /Methods ---
}
}