// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Components.Scene.Scenery;
using Henshin.Core.Scene.Directions;
using UnityEngine;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery {

/// <summary>
/// Class used to represent an individual actor used in a <see cref="Scene"/>.
/// Initializes the specified prefab when the <see cref="Scene"/> is created.
/// Stores a reference to the initialized actor in the <see cref="ActorComponent"/> property.
/// </summary>
[CreateAssetMenu(menuName = "Henshin/Scene/Actor", fileName = "SCENE_ACTOR_ActorName", order = 210)]
public class Actor: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// Unique identifier of this actor.
            /// Used only for debugging purposes.
            /// </summary>
            public string identifier;
            
            /// <summary>
            /// Reference to the prefab used by this actor.
            /// This prefab MUST have a <see cref="ActorComponent"/> somewhere.
            /// </summary>
            public GameObject actor;
            
        // -- Public Attributes --
            /// <summary>
            /// Reference to the instance of the <see cref="ActorComponent"/> currently in the scene.
            /// </summary>
            public ActorComponent ActorComponent { get; private set; }
            
            /// <summary>Reference to the default path to the actors objects.</summary>
            public const string DEFAULT_PATH = "Serialized/Actors";
            
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            /// <summary>
            /// Initializes the actor in the specified scene.
            /// This creates a new instance of the <see cref="actor"/> prefab.
            /// </summary>
            /// <param name="actorRoot">The scene to initialize the actor into.</param>
            /// <exception cref="MissingPrefabException{Actor}">If the <see cref="actor"/> attribute is set to null.</exception>
            /// <exception cref="MissingComponentException">If the <see cref="actor"/> prefab has no <see cref="Components.Scene.Scenery.ActorComponent"/> component.</exception>
            public void Initialize(Transform actorRoot) {
                // Check if the prefab instance is set.
                if (this.actor == null) {
                    // Throw an exception.
                    throw new MissingPrefabException<Actor>(attributeName: nameof(this.actor), containerIdentifier: this.identifier);
                }
                
                // Create a new instance of the actor prefab.
                GameObject actorInstance = Object.Instantiate(
                    original: this.actor,
                    position: Vector3.zero,
                    rotation: Quaternion.identity,
                    parent: actorRoot
                );
                
                // Get the ActorComponent from the instance.
                ActorComponent component = actorInstance.GetComponent<ActorComponent>();
                
                // If the component is not on the root-level object.
                if (component == null) {
                    // Search in the children.
                    component = actorInstance.GetComponentInChildren<ActorComponent>(includeInactive: true);
                }
                
                // If the component is still not found.
                if (component == null) {
                    // Throw an exception.
                    throw new MissingComponentException(message: $"There is no ActorComponent in \"#{this.identifier}\"'s actor prefab.");
                }
                
                // Store the component reference.
                this.ActorComponent = component;
            }
            
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}