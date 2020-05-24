// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Collections;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Application;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Scene {

/// <summary>
/// Static class used to manipulate the view of the scene.
/// Creates the actors and updates the background.
/// </summary>
public static class SceneView {
    // ---  Attributes ---
        // -- Public Constants --
            /// <summary>
            /// Time taken for the background to be revealed.
            /// </summary>
            public const float REVEAL_TIME = 2;
    // --- /Attributes ---
        
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares the specified scene's view.
            /// Creates the actors and updates the background.
            /// </summary>
            /// <param name="scene">The scene object that must be prepared.</param>
            /// <param name="callback">Callback triggered once the scene is set up.</param>
            public static void Prepare([NotNull] SceneState scene, UnityAction callback) {
                // Prepare the callback.
                UnityAction afterHide = () => {
                    // Delete all the previous actors.
                    ActorView.ClearActors();
                    
                    // Create all the scene's actors.
                    foreach (ActorState actorState in scene.ActorList) {
                        ActorView.InstantiateActor(actor: actorState);
                    }
                    
                    // Update the view's background.
                    ApplicationView.Background.sprite = scene.Background;
                    
                    // Invoke the callback.
                    callback.Invoke();
                    
                    // Reveal the background.
                    SceneView.Reveal(callback: () => {});
                };
                
                // Check if the background is set.
                if (SceneState.Current != null) {
                    // Hide the background.
                    SceneView.Hide(callback: afterHide);
                } else {
                    // Reveal the actors directly.
                    afterHide.Invoke();
                }
            }
            
            /// <summary>
            /// Reveals the background of the scene.
            /// </summary>
            /// <param name="callback">The callback to call once the background is visible.</param>
            public static void Reveal(UnityAction callback) {
                ApplicationView.Root.StartCoroutine(routine: SceneView._Reveal(callback: callback, reveal: true));
            }
            
            /// <summary>
            /// Hides the background of the scene.
            /// </summary>
            /// <param name="callback">The callback called once the operation is finished.</param>
            public static void Hide(UnityAction callback) {
                ApplicationView.Root.StartCoroutine(routine: SceneView._Reveal(callback: callback, reveal: false));
            }
            
        // -- Private Methods --
            /// <summary>
            /// Slowly reveals the scene images over a few seconds.
            /// </summary>
            /// <param name="callback">The callback called once the operation is finished.</param>
            /// <param name="reveal">If true, reveals the background. Otherwise hide it.</param>
            private static IEnumerator _Reveal(UnityAction callback, bool reveal = true) {
                // Create a new timer object.
                float timer = reveal ? 0 : SceneView.REVEAL_TIME;
                
                // Loop until the reveal time has elapsed.
                while (reveal ? timer <= SceneView.REVEAL_TIME : timer >= 0) {
                    // Increment the timer.
                    timer += reveal ? Time.deltaTime : -Time.deltaTime;
                    
                    // Get the normalized time.
                    float normBg = timer / SceneView.REVEAL_TIME;
                    
                    // Get the background color.
                    Color bgColor = Color.white;
                    
                    // Update the alpha value of the color.
                    bgColor.a = Mathf.Clamp(value: normBg, min: 0, max: 1);
                    
                    // Update the color of the background.
                    ApplicationView.Background.color = bgColor;
                    
                    // Wait for the next frame.
                    yield return new WaitForFixedUpdate();
                }
                
                // Call the callback.
                callback.Invoke();
            }
    // --- /Methods ---
}
}