// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using Henshin.Runtime.Gameplay.Components.Answer;
using Henshin.Runtime.Libraries;
using UnityEngine;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Target {

/// <summary>
/// Controller class used for the drop targets of the <see cref="AnswerController"/> objects.
/// </summary>
[AddComponentMenu(menuName: "Henshin/Gameplay/Target")]
public class TargetController: EmptyGraphic {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Stores the State of this TargetController instance.
            /// </summary>
            [NonSerialized]
            public TargetState State;
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Event triggered upon object instantiation.
            /// </summary>
            protected override void Awake() {
                // Load the state of the object.
                this.State = new TargetState(owner: this);
                
                // Enforce the size of the state.
                this.State.Transform.sizeDelta = GameplayState.AnswerObjectSize;
            }
            
            /// <summary>
            /// Event triggered on every frame.
            /// Updates the rect of the state.
            /// </summary>
            private void Update() {
                // Get the position and size of the rect.
                this.State.WorldRect.position = this.transform.position;
                this.State.WorldRect.size = ApplicationView.CanvasToWorld(point: this.State.Transform.sizeDelta);
            }
            
        // -- Public Methods --
            // - Initialization -
            // - Instantiation -
            /// <summary>
            /// Creates a new instance of the <see cref="TargetController"/> class.
            /// Uses the prefab found in <see cref="GameplayState.TextTargetPrefab"/>.
            /// </summary>
            /// <param name="parent">The <see cref="Transform"/> to which the new instance will be attached.</param>
            /// <returns>The new <see cref="TargetController"/> instance.</returns>
            public static TargetState Instantiate(Transform parent) {
                // Check if the prefab reference is set.
                if (GameplayState.Own.TextTargetPrefab != null) {
                    // Create the instance in the scene.
                    GameObject owner = Object.Instantiate(
                        original: GameplayState.Own.TextTargetPrefab,
                        parent: parent,
                        position: Vector3.zero,
                        rotation: Quaternion.identity 
                    );
                    // Set its position to be truly (0, 0, 0).
                    owner.transform.localPosition = Vector3.zero;
                    // Set its name.
                    owner.name = "Target Instance";
                    
                    // Try to load the target controller on the new instance.
                    if (owner.GetComponent<TargetController>() is TargetController controller) {
                        // Return the controller instance.
                        return controller.State;
                    } else {
                        // Raise an error.
                        ApplicationView.Error(message: "The GameplayState's TextTargetPrefab has no TargetController component.");
                        return null;                    
                    }
                } else {
                    // Raise an error.
                    ApplicationView.Error(message: "The GameplayState's TextTargetPrefab is not set.");
                    return null;                    
                }
            }
            
        // -- Private Methods --
    // --- /Methods ---
}
}