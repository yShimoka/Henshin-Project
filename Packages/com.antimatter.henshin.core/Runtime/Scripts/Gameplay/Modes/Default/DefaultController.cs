// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Directions.Scene;
using Henshin.Runtime.Gameplay.Components;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Modes.Default {

/// <summary>
/// </summary>
public static class DefaultController {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares the snowball gameplay.
            /// Generates all the components of the toolbox and prepares the text box object.
            /// </summary>
            /// <param name="actIndex">The index of the act to load.</param>
            /// <param name="gameplayIndex">The index of the gameplay to load.</param>
            /// <param name="sceneIndex"></param>
            public static void Prepare(int actIndex, int gameplayIndex, int sceneIndex) {
                TextBoxComponent original = null;
                TextBoxComponent translated = null;
                
                // Loop through all the actors and search for the text boxes.
                foreach (TextBoxComponent textBox in SceneState.Current.ActorList
                    .Select(selector: actorState => actorState.Instance.GetComponentsInChildren<TextBoxComponent>())
                    .Where(predicate: textBoxes => textBoxes.Length > 0)
                    .SelectMany(selector: textBoxes => textBoxes)
                ) {
                    // Check the tag.
                    if (textBox.CompareTag(tag: "TextBox_Original")) {
                        original = textBox;
                    } else if (textBox.CompareTag(tag: "TextBox_Translated")) {
                        translated = textBox;
                    }
                }
                
                // Check if the objects are set.
                if (original == null) {
                    ApplicationView.Error(message: "There is no original text box actor.");
                } else if (translated == null) {
                    ApplicationView.Error(message: "There is no translated text box actor.");
                } else {
                    // Enable all the objects.
                    original.transform.parent.gameObject.SetActive(value: true);
                    translated.transform.parent.gameObject.SetActive(value: true);
                    
                    // Load the act in the xml controller.
                    XmlController.SelectText(
                        actIndex: actIndex, 
                        sceneIndex: sceneIndex, 
                        gameplayIndex: gameplayIndex
                    );
                    
                    // Load the original and translated texts.
                    original.ParseText(text: XmlController.Original);
                    translated.ParseText(text: XmlController.Translated[0]);
                }
            }
    // --- /Methods ---
}
}