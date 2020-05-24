// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using System.Linq;
using Henshin.Runtime.Application;
using Henshin.Runtime.Gameplay.Components.Textbox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Selectable = Henshin.Runtime.Libraries.Selectable;

namespace Henshin.Runtime.Gameplay.Modes {

/// <summary>
/// Handles "Comparison" gameplays.
/// </summary>
public static class Comparison {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Callback called when the valid action is selected.
            /// </summary>
            public static UnityAction ValidCallback;
            
            /// <summary>
            /// Callback called when the invalid action is selected.
            /// </summary>
            public static UnityAction InvalidCallback;
            
            /// <summary>
            /// Flag set if the valid action should play.
            /// Otherwise trigger the invalid action by default.
            /// </summary>
            public static bool ValidInitialState;
            
            /// <summary>
            /// Flag set if the gameplay is in correction mode.
            /// </summary>
            public static bool IsInCorrection;
        
        // -- Private Attributes --
            /// <summary>
            /// Flag set if the user is playing an animation.
            /// </summary>
            private static bool _msIsPlaying = false;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Loads the data found in the current controller index.
            /// </summary>
            public static void Load() {
                // Load the textbox.
                TextboxView.Parse(index: GameplayState.Own.CurrentIndex);
                
                // Set the flag.
                Comparison.ValidInitialState = ApplicationController.Random.NextDouble() > 0.5;
                Comparison.IsInCorrection = false;
            }
            
            /// <summary>
            /// Binds the the valid and invalid boxes.
            /// </summary>
            public static void Play() {
                // Get the clickable objects.
                Selectable valid   = TextboxState.Instance.Translated.Find(n: "Valid")?.GetComponent<Selectable>();
                Selectable invalid = TextboxState.Instance.Translated.Find(n: "Invalid")?.GetComponent<Selectable>();
                
                // Check if the both of them are set.
                if (valid != null && invalid != null) {
                    // Reset the font styles.
                    foreach (Text text in TextboxState.Instance.ValidTexts.Union(second: TextboxState.Instance.InvalidTexts)) {
                        text.fontStyle = FontStyle.Normal;
                    }
                    
                    // Set the selected flag.
                    valid.Selected = TextboxState.Instance.ValidArrow.enabled;
                    invalid.Selected = TextboxState.Instance.InvalidArrow.enabled;
                    
                    // Bind the click callbacks.
                    valid.OnClick   = () => Comparison._OnClick(
                        replay: Comparison.ValidCallback, own: valid, other: invalid
                    );
                    invalid.OnClick = () => Comparison._OnClick(
                        replay: Comparison.InvalidCallback, own: invalid, other: valid
                    );
                } else {
                    ApplicationView.Error(message: "There are no Valid or Invalid clickable elements in the scene !");
                }
            }
            
            /// <summary>
            /// Hides the invalid text.
            /// </summary>
            public static void Correct() {
                // Get the invalid object.
                Selectable invalid = TextboxState.Instance.Translated.Find(n: "Invalid")?.GetComponent<Selectable>();
                
                // Disable it.
                if (invalid != null) invalid.gameObject.SetActive(value: false);
                
                // Show the valid texts.
                foreach (Text text in TextboxState.Instance.ValidTexts) {
                    text.fontStyle = FontStyle.Italic;
                }
                
                // Set the correction flag.
                Comparison.IsInCorrection = true;
                
                // Trigger the valid action.
                Comparison.ValidCallback?.Invoke();
            }
            
        // -- Private Methods --
            private static void _OnClick(UnityAction replay, Selectable own, Selectable other) {
                // Remove the selectable methods.
                own.OnClick = null;
                other.OnClick = null;
                
                // Check if the selectable is selected.
                if (own.Selected) {
                    // Select the own object.
                    GameplayState.Own.Callback?.Invoke();
                } else {
                    // Call the callback.
                    replay?.Invoke();
                }
            }
    // --- /Methods ---
}
}