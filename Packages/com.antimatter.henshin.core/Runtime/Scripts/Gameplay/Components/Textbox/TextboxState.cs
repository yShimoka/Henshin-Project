// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Textbox {

/// <summary>
/// Class used to describe the state of the game's Textbox.
/// There should only ever be one text box in the application.
/// </summary>
public class TextboxState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the <see cref="TextboxController"/> that owns this state.
            /// </summary>
            public readonly TextboxController Controller;
            
            /// <summary>
            /// Reference to the textbox's rect transform.
            /// </summary>
            public RectTransform Transform;
            
            /// <summary>
            /// Reference to the container of the original text.
            /// This container will be filled by the <see cref="TextboxView.Parse"/> method.
            /// </summary>
            public RectTransform Original;
            
            /// <summary>
            /// List of all the valid texts.
            /// </summary>
            public Text[] ValidTexts = new Text[0];
            
            /// <summary>
            /// List of all the invalid texts.
            /// </summary>
            public Text[] InvalidTexts = new Text[0];
            
            /// <summary>
            /// </summary>
            public Image ValidArrow;
            
            /// <summary>
            /// </summary>
            public Image InvalidArrow;
            
            /// <summary>
            /// Reference to the container of the translated text.
            /// This container will be filled by the <see cref="TextboxView.Parse"/> method.
            /// </summary>
            public RectTransform Translated;
            
            /// <summary>
            /// Reference to the background image of the textbox.
            /// </summary>
            [CanBeNull]
            public Image Background;
            
            /// <summary>
            /// Reference to the separator image of the textbox.
            /// </summary>
            [CanBeNull]
            public Image Separator;
            
            // - Static References -
            /// <summary>
            /// Reference to the box instance in the application.
            /// This value should be set once during the entire game.
            /// </summary>
            public static TextboxState Instance { get; private set; }
            
            /// <summary>
            /// Static accessor to the <see cref="TextboxController"/> instance.
            /// Simply wraps to <see cref="Instance"/>.<see cref="Controller"/>.
            /// </summary>
            public static TextboxController ControllerInstance => TextboxState.Instance.Controller;
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="TextboxState"/> instance.
            /// Loads some parameters from the specified <see cref="owner"/>.
            /// </summary>
            /// <param name="owner">The <see cref="TextboxController"/> that owns this state.</param>
            /// <exception cref="InvalidOperationException">There already is an instance of the state.</exception>
            public TextboxState(TextboxController owner) {
                // Check if the instance is not already set.
                if (TextboxState.Instance != null) {
                    throw new InvalidOperationException(message: "Tried to instantiate a second TextboxState");
                }
                
                // Store the text box instance.
                TextboxState.Instance = this;
                this.Controller = owner;
                
                // Find the rect transform of the owner.
                this.Transform = owner.GetComponent<RectTransform>();
            }
    // --- /Methods ---
}
}