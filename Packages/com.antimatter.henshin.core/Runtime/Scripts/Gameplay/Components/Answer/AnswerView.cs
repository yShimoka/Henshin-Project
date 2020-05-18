// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using UnityEngine;

namespace Henshin.Runtime.Gameplay.Components.Answer {

/// <summary>
/// Class used to manipulate the view of the specified <see cref="AnswerState"/> instance. 
/// </summary>
public static class AnswerView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Updates the text that is drawn on the specified answer.
            /// Uses the value of the answer as the text to render.
            /// </summary>
            /// <param name="answer">The answer to update.</param>
            public static void UpdateText(AnswerState answer) {
                // Check if the answer's Text component is set.
                if (answer.Text != null) {
                    // Update its text.
                    answer.Text.text = answer.Value;
                    
                    // Update the object's size.
                    answer.Transform.sizeDelta = GameplayState.AnswerObjectSize;
                }
            }
            
        // -- Private Methods --
    // --- /Methods ---
}
}