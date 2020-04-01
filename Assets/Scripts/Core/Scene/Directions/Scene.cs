// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Core.Scene.Scenery;
using UnityEngine;


/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Directions {

/// <summary>
/// This is the structure defining the scenes of the 
/// </summary>
[Serializable]
public struct Scene {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// Unique identifier of the <see cref="Scene"/> instance.
            /// Used  primarily for debugging purposes.
            /// </summary>
            public string identifier;
            
            /// <summary>List of all the lines of this <see cref="Scene"/>.</summary>
            public List<Line> lines;
            
            /// <summary>List of all the <see cref="Actor"/>s of this <see cref="Scene"/>.</summary>
            public List<Actor> actors;
            
            /// <summary><see cref="Sprite"/> used as the background of this <see cref="Scene"/>.</summary>
            public Sprite background;
        
        // -- Public Attributes --
            /// <summary>Reference to the <see cref="Scene"/> object that is currently playing.</summary>
            public static Scene? Current { get; private set; }
            
        // -- Protected Attributes --
        // -- Private Attributes --    
            /// <summary>Index of the line that is currently playing.</summary>
            private int _mCurrentLineIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares the <see cref="Scene"/>.
            /// Instantiates the <see cref="actors"/> in the specified <see cref="sceneRoot"/>.
            /// Prepares the <see cref="lines"/>.
            /// </summary>
            public void Prepare(Transform sceneRoot) {
                // Create the background game object.
                GameObject bgObject = new GameObject{ name = "Background" };
                bgObject.transform.SetParent(parent: sceneRoot, worldPositionStays: false);
                
                // Add the sprite renderer to the object.
                SpriteRenderer bgRenderer = bgObject.AddComponent<SpriteRenderer>();
                // Set the renderer's image.
                bgRenderer.sprite = this.background;
                // Set the renderer's plane.
                bgRenderer.sortingLayerID = SortingLayer.NameToID(name: "Background");
                
                // Create an actor root game object.
                GameObject actorRoot = new GameObject { name = "ActorRoot" };
                actorRoot.transform.SetParent(parent: sceneRoot, worldPositionStays: false);
                
                // Loop through the actors.
                foreach (Actor actor in this.actors) {
                    // Initialize the actor.
                    actor.Initialize(actorRoot: actorRoot.transform);
                }
                
                // Loop through the lines.
                for (int i = 0; i < this.lines.Count; i++) {
                    // Prepare the line.
                    this.lines[index: i].Prepare(isFirst: i == 0);
                }
            }
            
            /// <summary>
            /// Starts playing the current <see cref="Scene"/>.
            /// </summary>
            public void Play() {
                // Store the reference to the current scene.
                Scene.Current = this;
                
                // Reset the line index value.
                this._mCurrentLineIndex = 0;
                
                // Check if there is at least one line.
                if (this.lines.Count > 0) {
                    // Play the first line.
                    this.lines[index: this._mCurrentLineIndex].Play();
                } else {
                    // Log a warning.
                    Debug.LogWarning(message: $"Scene \"#{this.identifier}\" has no Lines !");
                    
                    // Advance onto the next scene.
                    this.Advance();
                }
            }
            
            /// <summary>
            /// Advances onto the next <see cref="Line"/> of the <see cref="Scene"/>.
            /// TODO: Check if the GameDirector allows the scene to advance by itself.
            /// </summary>
            public void NextLine() {
                // Increment the line index.
                this._mCurrentLineIndex++;
                
                // Check if all the lines have been played.
                if (this._mCurrentLineIndex < this.lines.Count) {
                    // Play the next line.
                    this.lines[index: this._mCurrentLineIndex].Play();
                } else {
                    // Advance onto the next scene.
                    this.Advance();
                }
            }
            
            /// <summary>
            /// Advances onto the next <see cref="Scene"/>.
            /// </summary>
            public void Advance() {
                // Clear the current scene reference.
                Scene.Current = null;
                
                // Check if the current act is set.
                if (Act.Current.HasValue) {
                    // Advance onto the next scene.
                    Act.Current.Value.NextScene();
                } else {
                    // Throw an exception.
                    throw new InvalidOperationException(message: $"Scene \"{this.identifier}\" has finished but it is not part of an Act !");
                }
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}