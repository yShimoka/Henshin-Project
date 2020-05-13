// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;
using UnityEngine;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Directions.Scene;
using Henshin.Runtime.Gameplay.Components;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Modes.Holes {

/// <summary>
/// Controller class used to manipulate the <see cref="HolesState"/> class.
/// Manages the behaviour of the Holes gameplay.
/// </summary>
public static class HolesController {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares the holes gameplay.
            /// Generates all the components of the toolbox and prepares the text box object.
            /// </summary>
            /// <param name="actIndex">The index of the act to load.</param>
            /// <param name="gameplayIndex">The index of the gameplay to load.</param>
            /// <param name="sceneIndex"></param>
            public static void Prepare(int actIndex, int gameplayIndex, int sceneIndex) {
                // Prepare the text box component variables.
                TextBoxComponent originalContainer = null;
                TextBoxComponent translatedContainer = null;
                ToolBoxComponent toolbox = null;

                // Prepare the string variables.
                string translated;
                
                // Loop through all the actors and search for the text boxes.
                foreach (ActorState actorState in SceneState.Current.ActorList) {
                    // Check if the actor has TextBox components.
                    TextBoxComponent[] textBoxes = actorState.Instance.GetComponentsInChildren<TextBoxComponent>();
                    if (textBoxes.Length > 0) {
                        // Loop through the text boxes.
                        foreach (TextBoxComponent textBox in textBoxes) {
                            // Check the tag.
                            if (textBox.CompareTag(tag: "TextBox_Original")) {
                                originalContainer = textBox;
                            } else if (textBox.CompareTag(tag: "TextBox_Translated")) {
                                translatedContainer = textBox;
                            }
                        }
                    } 
                    // Check if the actor has a tool box component.
                    else if (actorState.Instance.GetComponentInChildren<ToolBoxComponent>() is ToolBoxComponent toolBox) {
                        toolbox = toolBox;
                    }
                }
                
                // Check if the objects are set.
                if (originalContainer == null) {
                    ApplicationView.Error(message: "There is no original text box actor.");
                } else if (translatedContainer == null) {
                    ApplicationView.Error(message: "There is no translated text box actor.");
                } else if (toolbox == null) {
                    ApplicationView.Error(message: "There is no tool box actor.");
                } else {
                    // Enable all the objects.
                    toolbox.transform.parent.gameObject.SetActive(value: true);
                    originalContainer.transform.parent.gameObject.SetActive(value: true);
                    translatedContainer.transform.parent.gameObject.SetActive(value: true);

                    string original;
                    try {
                        // Load the act in the xml controller.
                        XmlController.SelectText(actIndex: actIndex, sceneIndex: sceneIndex, gameplayIndex: gameplayIndex);
                        
                        // Get the original text from the controller.
                        original = XmlController.Original;
                        
                        // Get the translated text from the controller.
                        translated = XmlController.Translated[0];
                    } catch (Exception e) {
                        // Log an error to the user.
                        ApplicationView.Error(
                            message: "An error was thrown when loading xml contents.",
                            details: $"Error message: {e.Message}"
                        );
                        return;
                    }
                    
                    // Clear the tool box.
                    toolbox.Clear();
                    // Parse the original text.
                    originalContainer.ParseText(text: original);
                    // Parse the translated text.
                    translatedContainer.ParseText(text: translated);
                    
                    // Generate the options.
                    foreach (string option in XmlController.Options) {
                        // Create the source object.
                        if (DraggableComponent.Instantiate(
                            identifier: option,
                            callback: HolesController._ActionCallback,
                            canBeWrong: HolesState.CanBeWrong,
                            createDuplicate: HolesState.CreateDuplicates,
                            size: translatedContainer.GetTextSize(text: option, width: float.PositiveInfinity),
                            toolbox: toolbox
                        ) is DraggableComponent component) {
                            // Enable the component.
                            component.enabled = true;
                        }
                    }
                    
                    // Get all the words to fill.
                    HolesState.Words = translatedContainer.GetComponentsInChildren<DropTargetComponent>();
                    foreach (DropTargetComponent dropTargetComponent in HolesState.Words) {
                        dropTargetComponent.enabled = false;
                    }
                    // Clear the counter.
                    HolesState.CompletedWords = 0;
                }
            }
            
            /// <summary>
            /// Starts the current gameplay.
            /// Calls the specified callback once the gameplay is over.
            /// </summary>
            public static void Play() {
                // Check if there are holes to fill.
                if (HolesState.Words.Length > 0) {
                    // Enable all the holes.
                    foreach (DropTargetComponent word in HolesState.Words) {
                        word.enabled = true;
                    }
                } else {
                    // Throw an error.
                    ApplicationView.Error(message: "There is no text to manipulate in this holes gameplay !");
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Callback triggered every time a draggable component finishes.
            /// </summary>
            private static void _ActionCallback() {
                // Increment the finished counter.
                HolesState.CompletedWords++;
                
                // Check if all actions are complete.
                if (HolesState.CompletedWords >= HolesState.Words.Length) {
                    // Call the stored callback.
                    if (GameplayState.Callback != null) {
                        GameplayState.Callback.Invoke();
                    } else {
                        ApplicationView.Error(message: "The gameplay is finished but it had nothing to callback to.");
                    }
                }
            }
    // --- /Methods ---
}
}