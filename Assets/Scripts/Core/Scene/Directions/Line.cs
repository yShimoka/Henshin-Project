// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Core.Scene.Scenery;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Directions {

/// <summary>
/// This is the structure that describes a single line of dialogue of the play.
/// This is analogous to lines in a theatre play but are not strictly the same.
/// Lines are used to describe a single action that takes part on the screen.
/// It is made of an <see cref="Actor"/> object and a list of <see cref="Transformation"/>.
/// It can also update the GUI text if needed.
/// </summary>
[Serializable]
public struct Line {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Line Info -
            /// <summary>
            /// Unique identifier of the <see cref="Line"/> instance.
            /// Used primarily for debugging purposes.
            /// </summary>
            public string identifier;
            
            /// <summary>First <see cref="Transformation"/> that should be applied in the <see cref="Line"/>.</summary>
            /// <seealso cref="Transformation"/>
            public Transformation rootTransformation;
            
        // -- Public Attributes --
            /// <summary>Static reference to the line currently being played.</summary>
            public static Line? Current { get; private set; }
            
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares the <see cref="Transformation"/> of this line.
            /// </summary>
            /// <param name="isFirst">Should be set if this is the first <see cref="Line"/> of the <see cref="Scene"/>.</param>
            public void Prepare(bool isFirst = false) {
                // Prepare the root node.
                this.rootTransformation.Prepare(instant: isFirst);
            }
            
            /// <summary>
            /// Starts the play of the line.
            /// Triggers the <see cref="Transformation"/> chain.
            /// </summary>
            public void Play() {
                // Store the current line.
                Line.Current = this;
                
                // Ensure that there is at least one transformation.
                if (this.rootTransformation != null) {
                    // Apply the first transformation to the actor.
                    this.rootTransformation.Apply(); 
                } else {
                    // Go to the next line.
                    this.Advance();
                }
            }
            
            /// <summary>
            /// Advances onto the next line.
            /// </summary>
            public void Advance() {
                // Remove the current line reference.
                Line.Current = null;
                
                // Check if the current scene is set.
                if (Scene.Current.HasValue) {
                    // Go to the next line.
                    Scene.Current.Value.NextLine();
                } else {
                    // Throw an exception.
                    throw new InvalidOperationException(message: $"Line \"#{this.identifier}\" has finished but it is not part of a Scene !");
                }
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}