// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Gameplay.Components.Answer;
using Henshin.Runtime.Gameplay.Components.Textbox;
using UnityEngine;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Toolbox {

/// <summary>
/// Controller class used to manipulate the <see cref="ToolboxState"/>.
/// </summary>
[RequireComponent(requiredComponent: typeof(RectTransform))]
public class ToolboxController: MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// Reference to the container that will hold the <see cref="AnswerController"/> instances. 
            /// </summary>
            public RectTransform Container;
            
        // -- Public Attributes --
            /// <summary>
            /// Reference to the state of this controller.
            /// </summary>
            [NonSerialized]
            public ToolboxState State;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Event triggered right after the textbox instance is created.
            /// </summary>
            private void Awake() {
                // Create a new State for the toolbox.
                this.State = new ToolboxState(owner: this);
            }
            
            // - Instance Management -
            /// <summary>
            /// Instantiates a new <see cref="ToolboxController"/> in the scene.
            /// Uses the prefab found in <see cref="GameplayState.ToolboxPrefab"/>.
            /// </summary>
            /// <param name="parent">The <see cref="Transform"/> to which the new instance will be attached.</param>
            /// <exception cref="MissingComponentException">
            /// The <see cref="GameplayState.ToolboxPrefab"/> has no <see cref="ToolboxController"/> component on it.
            /// </exception>
            /// <exception cref="MissingReferenceException">
            /// The <see cref="GameplayState.ToolboxPrefab"/> is not set.
            /// </exception>
            public static void Instantiate(Transform parent) {
                // Seek the prefab in the current gameplay state.
                if (GameplayState.Own.ToolboxPrefab != null) {
                    // Instantiate the prefab.
                    GameObject owner = Object.Instantiate(
                        original: GameplayState.Own.ToolboxPrefab,
                        parent: parent,
                        position: Vector3.zero,
                        rotation: Quaternion.identity
                    );
                    // Reset its position to ensure that it is at (0, 0, 0).
                    owner.transform.localPosition = Vector3.zero;
                    // Set its name.
                    owner.name = "Textbox";
                    
                    // Check if the answer controller is set.
                    if (!(owner.GetComponent<ToolboxController>() is ToolboxController component)) {
                        throw new MissingComponentException(
                            message:"The specified ToolboxPrefab does not have a ToolboxController component."
                        );
                    }
                    
                    // Set the position of the object.
                    component.State.Transform.anchoredPosition = Vector2.right * component.State.Transform.sizeDelta;
                    ToolboxView.Hide(callback: null, time: 0);
                } else {
                    throw new MissingReferenceException(
                        message: "Tried to instance a new ToolboxController but no " +
                                 "ToolboxPrefab was specified in the GameplayState instance."
                    );
                }
            }
            
    // --- /Methods ---
}
}