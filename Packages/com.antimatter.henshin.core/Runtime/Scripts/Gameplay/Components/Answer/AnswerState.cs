// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Gameplay.Components.Target;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Answer {

/// <summary>
/// State class used to represent the state of an answer.
/// </summary>
public class AnswerState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Parameters -
            /// <summary>
            /// Value of this answer.
            /// </summary>
            [NotNull]
            public string Value;
            
            // - Components -
            /// <summary>
            /// Reference to the original parent of this answer.
            /// </summary>
            [NotNull]
            public readonly Transform Parent;
            
            /// <summary>
            /// Reference to the rect transform of the answer.
            /// </summary>
            [NotNull]
            public readonly RectTransform Transform;
            
            /// <summary>
            /// Reference to the controller that owns this state.
            /// </summary>
            [NotNull]
            public readonly AnswerController Controller;
            
            /// <summary>
            /// Reference to the text component found on the game object.
            /// </summary>
            [CanBeNull]
            public Text Text;
            
            // - References -
            /// <summary>
            /// List of all the targets that this <see cref="AnswerState"/> could be placed on.
            /// Loaded on the <see cref="AnswerController.OnBeginDrag"/> call.
            /// </summary>
            public TargetController[] PossibleTargets;
            
            /// <summary>
            /// Reference to the image component found on the game object.
            /// </summary>
            [CanBeNull]
            public Image Image;
            
            /// <summary>
            /// Callback method that is invoked when the answer is placed down.
            /// </summary>
            [CanBeNull]
            public UnityAction Callback;
            
            // - Computed Values -
            /// <summary>
            /// Rect of this answer, in world coordinates.
            /// </summary>
            public Rect WorldRect = new Rect();
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="AnswerState"/> instance.
            /// </summary>
            /// <param name="owner">The owner of this answer state.</param>
            public AnswerState(AnswerController owner) {
                // Store the controller reference.
                this.Controller = owner;
                
                // Get the rect transform of the answer.
                if (owner.GetComponent<RectTransform>() is RectTransform rectTransform) {
                    this.Transform = rectTransform;
                } else {
                    throw new MissingComponentException(message: "This AnswerState's owner has no RectTransform !");
                }
                
                // Get the parent of the controller.
                this.Parent = this.Controller.transform.parent;
                
                // Load a default string value. Used for debugging.
                this.Value = "__undefined_client__";
            }
    // --- /Methods ---
}
}