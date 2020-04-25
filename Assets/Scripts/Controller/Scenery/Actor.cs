// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using Henshin.State;
using Henshin.View;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Scenery {

/// <summary>
/// 
/// </summary>
public static class Actor {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            /// <summary>
            /// Instantiates a new actor gameobject in the scene.
            /// </summary>
            /// <param name="actor">The actor object to instantiate.</param>
            /// <param name="parent">The parent of the new actor instance.</param>
            public static void Instantiate(State.Scenery.Actor actor, Transform parent) {
                // Create a new instance of the actor's prefab.
                GameObject actorInstance = Object.Instantiate(
                    original: actor.actorPrefab,
                    parent: parent,
                    position: Vector3.zero,
                    rotation: Quaternion.identity
                );
                actorInstance.transform.localPosition = Vector3.back * 5;
                
                // Try to load the component from the actor.
                View.Scenery.Actor actorComponent = actorInstance.GetComponent<View.Scenery.Actor>();
                
                // If the loading failed, search in the children.
                if (actorComponent == null) {
                    actorComponent = actorInstance.GetComponentInChildren<View.Scenery.Actor>(includeInactive: true);
                }
                
                // If the loading failed still, throw an error.
                if (actorComponent == null) {
                    throw Application.Error(message: $"Actor {actor.name.Substring(startIndex: actor.name.LastIndexOf(value: '_') + 1)} does not have an Actor component.");
                }
                
                // Set the component in the actor.
                actor.ActorComponent = actorComponent;
                
                // Disable the GameObject.
                actorInstance.SetActive(value: false);
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}