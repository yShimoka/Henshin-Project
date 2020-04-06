// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Directions {

/// <summary>
/// 
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Henshin/Scene/Act", fileName = "ACT_ActName", order = 200)]
public class Act: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// Unique identifier of the <see cref="Act"/> instance.
            /// Used  primarily for debugging purposes.
            /// </summary>
            public string identifier;
            
            /// <summary>List of all the scenes in this act.</summary>
            public List<Scene> scenes;
            
        // -- Public Attributes --
            /// <summary>Reference to the <see cref="Act"/> that is currently being played.</summary>
            public static Act Current { get; private set; }
            
        // -- Private Attributes --
            /// <summary>List of all the <see cref="Scene"/>'s <see cref="GameObject"/> instances.</summary>
            private List<GameObject> _mSceneObjects;
            
            /// <summary>Index of the scene that is currently playing.</summary>
            private int _mCurrentSceneIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares all the <see cref="Scene"/>s of this <see cref="Act"/>.
            /// </summary>
            /// <param name="theatreRoot">The theatre's root <see cref="Transform"/>.</param>
            public void Prepare(Transform theatreRoot) {
                // Create the scene object list.
                this._mSceneObjects = new List<GameObject>();
                
                // Loop through each of the scene's game objects..
                foreach (GameObject sceneRoot in this.scenes.Select(selector: scene => new GameObject{ name = $"{scene.identifier}" })) {
                    // Set the parent of the scene root.
                    sceneRoot.transform.SetParent(parent: theatreRoot, worldPositionStays: false);
                    
                    // Disable the scene.
                    sceneRoot.SetActive(value: false);
                    
                    // Add it to the list.
                    this._mSceneObjects.Add(item: sceneRoot);
                }
            }
            
            /// <summary>Plays the <see cref="Act"/> instance.</summary>
            public void Play() {
                // Store the current act reference.
                Act.Current = this;
                
                // Reset the scene counter.
                this._mCurrentSceneIndex = 0;
                
                // Ensure that there is at least one scene object.
                if (this.scenes.Count > 0) {
                    // Show the first scene.
                    this._mSceneObjects[index: 0].SetActive(value: true);
                    // Prepare the first scene.
                    this.scenes[index: 0].Prepare(sceneRoot: this._mSceneObjects[index: 0].transform);
                    
                    // Play the first scene.
                    this.scenes[index: 0].Play();
                } else {
                    // Log a warning.
                    Debug.LogWarning(message: $"There is no Scenes in the \"#{this.identifier}\" Act !");
                    
                    // Advance onto the next act.
                    this.Advance();
                }
            }
            
            /// <summary>Advances onto the next <see cref="Scene"/> in the list.</summary>
            public void NextScene() {
                // Disable the current scene.
                this._mSceneObjects[index: this._mCurrentSceneIndex].SetActive(value: false);
                // Increment the scene counter.
                this._mCurrentSceneIndex++;
                
                // Check if there is another scene to play.
                if (this._mCurrentSceneIndex < this.scenes.Count) {
                    // Enable the next scene.
                    this._mSceneObjects[index: this._mCurrentSceneIndex].SetActive(value: true);
                    // Prepare the next scene.
                    this.scenes[index: this._mCurrentSceneIndex].Prepare(
                        sceneRoot: this._mSceneObjects[index: this._mCurrentSceneIndex].transform
                    );
                    
                    // Play the next scene.
                    this.scenes[index: this._mCurrentSceneIndex].Play();
                } else {
                    // Advance onto the next act.
                    this.Advance();
                }
            }
            
            /// <summary>Advances onto the next <see cref="Act"/> of the game.</summary>
            public void Advance() {
                // Remove the reference to the current act.
                Act.Current = null;
                
                // TODO: Advance to the next act.
            }
            
        // -- Private Methods --
    // --- /Methods ---
}
}