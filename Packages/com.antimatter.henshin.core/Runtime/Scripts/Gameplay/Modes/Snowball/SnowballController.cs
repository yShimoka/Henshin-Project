// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Directions.Scene;
using Henshin.Runtime.Gameplay.Components;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Modes.Snowball {

/// <summary>
/// Controller class used to manipulate the <see cref="SnowballState"/> class.
/// Manages the behaviour of the Holes gameplay.
/// </summary>
public static class SnowballController {
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
                // Loop through all the actors and search for the text boxes.
                foreach (ActorState actorState in SceneState.Current.ActorList) {
                    // Check if the actor has TextBox components.
                    TextBoxComponent[] textBoxes = actorState.Instance.GetComponentsInChildren<TextBoxComponent>();
                    if (textBoxes.Length > 0) {
                        // Loop through the text boxes.
                        foreach (TextBoxComponent textBox in textBoxes) {
                            // Check the tag.
                            if (textBox.CompareTag(tag: "TextBox_Original")) {
                                SnowballState.OriginalComponent = textBox;
                            } else if (textBox.CompareTag(tag: "TextBox_Translated")) {
                                SnowballState.TranslatedComponent = textBox;
                            }
                        }
                    } 
                    // Check if the actor has a tool box component.
                    else if (actorState.Instance.GetComponentInChildren<ToolBoxComponent>() is ToolBoxComponent toolBox) {
                        SnowballState.ToolboxComponent = toolBox;
                    }
                }
                
                // Check if the objects are set.
                if (SnowballState.OriginalComponent == null) {
                    ApplicationView.Error(message: "There is no original text box actor.");
                } else if (SnowballState.TranslatedComponent == null) {
                    ApplicationView.Error(message: "There is no translated text box actor.");
                } else if (SnowballState.ToolboxComponent == null) {
                    ApplicationView.Error(message: "There is no tool box actor.");
                } else {
                    // Enable all the objects.
                    SnowballState.OriginalComponent.transform.parent.gameObject.SetActive(value: true);
                    SnowballState.TranslatedComponent.transform.parent.gameObject.SetActive(value: true);
                    SnowballState.ToolboxComponent.transform.parent.gameObject.SetActive(value: true);
                    
                    // Load the act in the xml controller.
                    XmlController.SelectText(
                        actIndex: actIndex, 
                        sceneIndex: sceneIndex, 
                        gameplayIndex: gameplayIndex
                    );
                    
                    // Set the text index.
                    XmlController.TextIndex = -1;
                    // Load the first text.
                    SnowballController._LoadNextText();
                }
            }
            
            /// <summary>
            /// Starts the current gameplay.
            /// Calls the specified callback once the gameplay is over.
            /// </summary>
            public static void Play() {
                // Check if there are holes to fill.
                if (SnowballState.Words.Length > 0) {
                    // Clear the counter.
                    SnowballState.CompletedWords = 0;
                    
                    // Enable all the target words.
                    foreach (DropTargetComponent word in SnowballState.Words) {
                        word.enabled = true;
                    }
                } else {
                    ApplicationView.Error(message: "Tried to play a gameplay without text to fill.");
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Callback triggered every time a draggable component finishes.
            /// </summary>
            private static void _ActionCallback() {
                // Increment the counter.
                SnowballState.CompletedWords++;
                
                // Check if all words are found.
                if (SnowballState.CompletedWords >= SnowballState.Words.Length) {
                    // Load the next text.
                    SnowballController._LoadNextText();
                    
                    // Call the callback.
                    if (GameplayState.Callback != null) {
                        GameplayState.Callback.Invoke();
                    } else {
                        // Throw an error.
                        ApplicationView.Error(message: "There is no text to manipulate in this snowball gameplay !");
                    }
                }
            }
            
            private static void _LoadNextText() {
                string original;
                string translated;
                try {
                    // Get the next text.
                    XmlController.TextIndex++;
                    
                    // Get the original text from the controller.
                    original = XmlController.Original;
                    
                    // Get the translated text from the controller.
                    translated = XmlController.Translated[0];
                } catch (Exception) {
                    // Do nothing.
                    return;
                }
                
                // Clear the tool box.
                SnowballState.ToolboxComponent.Clear();
                // Parse the original text.
                SnowballState.OriginalComponent.ParseText(text: original);
                // Parse the translated text.
                SnowballState.TranslatedComponent.ParseText(text: translated);
                
                // Generate the options.
                foreach (string option in XmlController.Options) {
                    // Create the source object.
                    if (DraggableComponent.Instantiate(
                        identifier: option,
                        callback: SnowballController._ActionCallback,
                        canBeWrong: true,
                        createDuplicate: false,
                        size: SnowballState.OriginalComponent.GetTextSize(text: option, width: float.PositiveInfinity),
                        toolbox: SnowballState.ToolboxComponent
                    ) is DraggableComponent component) {
                        // Enable the component.
                        component.enabled = true;
                    }
                }
                
                // Get all the words to fill.
                SnowballState.Words = SnowballState.TranslatedComponent.GetComponentsInChildren<DropTargetComponent>();
                foreach (DropTargetComponent dropTargetComponent in SnowballState.Words) {
                    dropTargetComponent.enabled = false;
                }
            }
    // --- /Methods ---
}
}