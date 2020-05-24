// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

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
            /// <param name="text">[Optional]The text to set as the answer's value.</param>
            public static void UpdateText(AnswerState answer, string text = null) {
                // Check if the answer's Text component is set.
                if (answer.Text != null) {
                    // If a text was specified, set the value of the answer.
                    if (text != null) answer.Value = text;
                    
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