// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Textbox {

/// <summary>
/// Controller class used to manipulate the game's Textbox.
/// There should only ever be ONE instance of the text box on the screen at any time.
/// </summary>
[RequireComponent(requiredComponent: typeof(RectTransform))]
public class TextboxController: MonoBehaviour {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Reference to the state of this controller.
            /// </summary>
            [NonSerialized]
            public TextboxState State;
    // --- /Attributes ---

    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Event triggered right after the textbox instance is created.
            /// </summary>
            private void Awake() {
                // Create a new State for the text box.
                this.State = new TextboxState(owner: this);
                
                // Get all the children Text components.
                RectTransform[] containers = this.GetComponentsInChildren<RectTransform>();
                
                // Seek the original text.
                if (containers
                    .FirstOrDefault(predicate: text => text.CompareTag(tag: "Original")) 
                    is RectTransform original
                ) {
                    // Load it in the state.
                    this.State.Original = original;
                } else {
                    // Log a warning.
                    Debug.LogWarning(
                        message: "Could not find a RectTransform component with the Original tag on this object's children." +
                                 "The game may not function as planned.",
                        context: this
                    );
                }
                
                // Seek the translated text.
                if (containers
                    .FirstOrDefault(predicate: text => text.CompareTag(tag: "Translated")) 
                    is RectTransform translated
                ) {
                    // Load it in the state.
                    this.State.Translated = translated;
                } else {
                    // Log a warning.
                    Debug.LogWarning(
                        message: "Could not find a RectTransform component with the Translated tag on this object's children." +
                                 "The game may not function as planned.",
                        context: this
                    );
                }
                
                // Seek a potential background.
                if (this.transform.Find(n: "Background").GetComponent<Image>() is Image background) {
                    this.State.Background = background;
                }
                
                // Seek a potential separator.
                if (this.transform.Find(n: "Separator").GetComponent<Image>() is Image separator) {
                    this.State.Separator = separator;
                }
            }
            
            // - Instance Management -
            /// <summary>
            /// Instantiates a new <see cref="TextboxController"/> in the scene.
            /// Uses the prefab found in <see cref="GameplayState.TextboxPrefab"/>.
            /// </summary>
            /// <param name="parent">The <see cref="Transform"/> to which the new instance will be attached.</param>
            /// <exception cref="MissingComponentException">
            /// The <see cref="GameplayState.TextboxPrefab"/> has no <see cref="TextboxController"/> component on it.
            /// </exception>
            /// <exception cref="MissingReferenceException">
            /// The <see cref="GameplayState.TextboxPrefab"/> is not set.
            /// </exception>
            public static void Instantiate(Transform parent) {
                // Seek the prefab in the current gameplay state.
                if (GameplayState.Own.TextboxPrefab != null) {
                    // Instantiate the prefab.
                    GameObject owner = Object.Instantiate(
                        original: GameplayState.Own.TextboxPrefab,
                        parent: parent,
                        position: Vector3.zero,
                        rotation: Quaternion.identity
                    );
                    // Reset its position to ensure that it is at (0, 0, 0).
                    owner.transform.localPosition = Vector3.zero;
                    // Set its name.
                    owner.name = "Textbox";
                    
                    // Check if the answer controller is set.
                    if (!(owner.GetComponent<TextboxController>() is TextboxController component)) {
                        throw new MissingComponentException(
                            message:"The specified TextboxPrefab does not have a TextboxController component."
                        );
                    }
                    
                    // Set the position of the object.
                    component.State.Transform.anchoredPosition = Vector2.up * 16;
                    TextboxView.Hide(callback: null, time: 0);
                } else {
                    throw new MissingReferenceException(
                        message: "Tried to instance a new TextboxController but no " +
                                 "TextboxPrefab was specified in the GameplayState instance."
                    );
                }
            }
            
        // -- Public Methods --
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}