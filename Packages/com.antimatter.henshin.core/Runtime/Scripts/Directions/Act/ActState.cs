// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using Henshin.Runtime.Directions.Scene;
using JetBrains.Annotations;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Act {

/// <summary>
/// State class used to describe the state of an act.
/// These instances are stored and serialized in the <see cref="Application.ApplicationState"/> object.
/// </summary>
[Serializable]
public class ActState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Unique Identifier -
            /// <summary>
            /// Random hash assigned to the act.
            /// This is used in equality comparisons.
            /// </summary>
            public int Hash;
            
            // - Parameters -
            /// <summary>
            /// List of all the scenes in the current act.
            /// </summary>
            public SceneState[] SceneList;
            
        // -- Public Attributes --
            // - Identifiers -
            /// <summary>
            /// Index of the act in the state's list.
            /// </summary>
            [NonSerialized]
            public int Index;
            
            /// <summary>
            /// Accessor to the identifier of this act.
            /// </summary>
            public string Identifier => $"Act - {this.Index + 1}";
            
            // - Runtime Parameters -
            /// <summary>
            /// Reference to the owner of this act.
            /// </summary>
            [NonSerialized]
            public ApplicationState Owner;
            
            /// <summary>
            /// Index of the scene that is currently playing.
            /// </summary>
            [NonSerialized]
            public int CurrentSceneIndex;
            
            // - Static References -
            /// <summary>
            /// Reference to the act that is currently being played.
            /// </summary>
            [NonSerialized]
            public static ActState Current;


            // - Helper Properties -
            /// <summary>
            /// Helper accessor to the current scene object.
            /// </summary>
            [CanBeNull]
            public SceneState CurrentScene {
                get {
                    // Check if the scene index is valid.
                    if (this.CurrentSceneIndex >= 0 && this.CurrentSceneIndex < this.SceneList.Length) {
                        // Return the scene instance.
                        return this.SceneList[this.CurrentSceneIndex];
                    } else {
                        // Return a null.
                        return null;
                    }
                }
            }
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Assigns the value of the <see cref="Hash"/> field.
            /// </summary>
            public ActState() { this.Hash = ApplicationController.Random.Next(); }
            
        // -- Comparison --
            /// <summary>
            /// Checks if two <see cref="ActState"/> objects are the same.
            /// Compare both of their <see cref="Hash"/> values together.
            /// </summary>
            /// <param name="obj">The object to compare with.</param>
            /// <returns>True if both objects are <see cref="ActState"/>s with the same <see cref="Hash"/></returns>
            public override bool Equals(object obj) {
                // Check if the object is an act state.
                if (!(obj is ActState other)) return false;
                
                // Compare both of their hashes.
                return this.Hash == other.Hash;
            }

            /// <summary>
            /// Returns the hash of this object.
            /// </summary>
            /// <returns>The value of the <see cref="Hash"/> attribute.</returns>
            public override int GetHashCode() {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                return this.Hash;
            }
    // --- /Methods ---
}
}