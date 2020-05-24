// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay.Components.Answer;
using Henshin.Runtime.Gameplay.Components.Target;
using Henshin.Runtime.Gameplay.Components.Textbox;
using Henshin.Runtime.Gameplay.Components.Toolbox;
using UnityEngine;

namespace Henshin.Runtime.Gameplay.Modes {

/// <summary>
/// Handles "fill-the-gaps" gameplays.
/// </summary>
public static class Gaps {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>
            /// List of all the <see cref="TargetController"/> generated on the <see cref="Load"/> call.
            /// </summary>
            private static TargetController[] _msTargetControllers;
            
            /// <summary>
            /// Counter of all the answer placed.
            /// </summary>
            private static int _msPlacedAnswers;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Loads the data found in the current controller index.
            /// </summary>
            public static void Load() {
                // Load the text and tool boxes.
                TextboxView.Parse(index: GameplayState.Own.CurrentIndex);
                ToolboxView.Parse(index: GameplayState.Own.CurrentIndex);
                
                // Get all the generated targets.
                Gaps._msTargetControllers = TextboxState.ControllerInstance.GetComponentsInChildren<TargetController>(); 
            }
            
            /// <summary>
            /// Enables the generated <see cref="AnswerController"/> instances.
            /// </summary>
            public static void Play() {
                // If there is no answer to place.
                if (Gaps._msTargetControllers.Length == 0) {
                    // Call the callback.
                    GameplayState.Own.Callback?.Invoke();
                } else {
                    // Clear the counter.
                    Gaps._msPlacedAnswers = 0;
                    
                    // Get all the answers in the session.
                    AnswerController[] answers = ToolboxState.ControllerInstance.GetComponentsInChildren<AnswerController>();
                    
                    // Enable them and set their callbacks.
                    foreach (AnswerController controller in answers) {
                        controller.enabled = true;
                        controller.State.Callback = Gaps._OnAnswerPlaced;
                    }
                }
            }
            
            /// <summary>
            /// Corrects the gaps targets.
            /// </summary>
            public static void Correct() {
                // Clear the toolbox.
                ToolboxView.Clear();
                
                // Set the values of all the valid answers.
                foreach (TargetController controller in Gaps._msTargetControllers) {
                    AnswerView.UpdateText(answer: controller.State.PlacedAnswer, text: controller.State.Value);
                    
                    // TEMP: Show that these are the correct answers.
                    if (controller.State.PlacedAnswer.Image != null)
                        controller.State.PlacedAnswer.Image.color = Color.green;
                    if (controller.State.PlacedAnswer.Text != null)
                        controller.State.PlacedAnswer.Text.fontStyle = FontStyle.Italic;
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Callback called when an answer is placed.
            /// </summary>
            private static void _OnAnswerPlaced(AnswerController answer, TargetController target) {
                // Increment the valid answer count.
                Gaps._msPlacedAnswers++;
                
                // Check if the counter reached the required number of instances.
                if (Gaps._msPlacedAnswers >= Gaps._msTargetControllers.Length) {
                    // Call the callback.
                    if (GameplayState.Own.Callback != null) {
                        GameplayState.Own.Callback.Invoke();
                    } else {
                        ApplicationView.Error(message: "The Gaps gameplay finished without a callback to invoke !");
                    }
                }
            }
    // --- /Methods ---
}
}