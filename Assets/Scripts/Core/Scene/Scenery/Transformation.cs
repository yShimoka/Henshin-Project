// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Components.Scene.Scenery;
using Henshin.Core.Scene.Directions;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery {

/// <summary>
/// Individual transformation that can be applied to an <see cref="ActorComponent"/>.
/// Behaves similarly to a tree that allows parallel execution of multiple transformations.
/// </summary>
[Serializable]
public class Transformation {
    // ---  SubObjects ---
        // -- Public Enums --
            /// <summary>
            /// List of all the types of transformation that can be applied.
            /// This is used by the <see cref="Transformation"/> class to know what to update.
            /// </summary>
            [Flags]
            public enum ETransformationType {
                Colour             = 0x1, 
                Position           = 0x2, 
                Rotation           = 0x4,     
                Scale              = 0x8, 
                HorizontalFlip     = 0x10, 
                VerticalFlip       = 0x20, 
                Pose               = 0x40
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Tree Behaviour -
            /// <summary>
            /// List of all the child nodes of this <see cref="Transformation"/>.
            /// This is used to achieve parallelization of the <see cref="Transformation"/>s. 
            /// </summary>
            public List<Transformation> nodes;
            
            /// <summary>
            /// Count of all the parent <see cref="Transformation"/> nodes.
            /// This is used to trigger a <see cref="Transformation"/> only once all its parent are finished.
            /// </summary>
            public int parentCount;
            
            // - Transformation Parameters -
            /// <summary>
            /// Unique identifier of the <see cref="Transformation"/> instance.
            /// Used  primarily for debugging purposes.
            /// </summary>
            public string identifier;
            
            /// <summary><see cref="Actor"/> object that is being transformed in the <see cref="Scene"/>.</summary>
            public Actor actor;
            
            /// <summary>Defines the time (in seconds) that the <see cref="Transformation"/> takes.</summary>
            public float time;
            
            /// <summary>Defines the type of this <see cref="Transformation"/>.</summary>
            /// <seealso cref="ETransformationType"/>
            public ETransformationType type;
            
            /// <summary>Defines the target <see cref="Color"/> for this <see cref="Transformation"/>.</summary>
            public Color colour;
            
            /// <summary>Defines the target position for this <see cref="Transformation"/>.</summary>
            public Vector3 position;
            
            /// <summary>Defines the target rotation for this <see cref="Transformation"/>.</summary>
            public Quaternion rotation;
            
            /// <summary>Defines the target scale for this <see cref="Transformation"/>.</summary>
            public Vector3 scale;
            
            /// <summary>Defines if the <see cref="ActorComponent"/> should be flipped horizontally with this <see cref="Transformation"/>.</summary>
            public bool flipHorizontal;
            
            /// <summary>Defines if the <see cref="ActorComponent"/> should be flipped vertically with this <see cref="Transformation"/>.</summary>
            public bool flipVertical;
            
            /// <summary>Defines the new pose for the <see cref="ActorComponent"/> with this <see cref="Transformation"/>.</summary>
            public Sprite pose;
            
        // -- Public Attributes --
        // -- Private Attributes --
            // - Transformation Parameters -
            /// <summary>Stores the time elapsed since the <see cref="Transformation"/> was applied.</summary>
            /// <seealso cref="Apply"/>
            private float _mTimer;
            
            /// <summary>Stores a copy of the <see cref="ActorComponent"/>'s starting colour.</summary>
            private Color _mStartingColour;
            
            /// <summary>Stores a copy of the <see cref="ActorComponent"/>'s starting position.</summary>
            private Vector3 _mStartingPosition;
            
            /// <summary>Stores a copy of the <see cref="ActorComponent"/>'s starting rotation.</summary>
            private Quaternion _mStartingRotation;
            
            /// <summary>Stores a copy of the <see cref="ActorComponent"/>'s starting scale.</summary>
            private Vector3 _mStartingScale;
            
            // - Tree Behaviour -
            /// <summary>
            /// Counts the number of times that the <see cref="Apply"/> method was called.
            /// This is used with the <see cref="parentCount"/> parameter
            /// to trigger the <see cref="Transformation"/> only if all parent <see cref="Transformation"/>s are already over. 
            /// </summary>
            private int _mApplyCounter;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares the <see cref="Transformation"/> instance.
            /// </summary>
            /// <param name="instant">If set to true, this <see cref="Transformation"/> will happen instantly.</param>
            public void Prepare(bool instant = false) {
                // Reset the apply counter.
                this._mApplyCounter = 0;
                
                // If the instant flag is set.
                if (instant) {
                    // Set the target time to 0.
                    this.time = 0;
                }
                
                // Prepare the children transformations.
                foreach (Transformation node in this.nodes) { node.Prepare(); }
            }
            
            /// <summary>
            /// Starts applying the transformation to the specified <see cref="ActorComponent"/>.
            /// </summary>
            public void Apply() {
                // Ensure that the actor is set.
                if (this.actor == null) {
                    // Throw an exception.
                    throw new MissingPrefabException<Transformation>(attributeName: nameof(this.actor), containerIdentifier: this.identifier);
                }
                
                // Update the apply counter.
                this._mApplyCounter++;
                
                // Check if the counter reached the required number.
                if (this._mApplyCounter >= this.parentCount) {
                    // Clear the timer.
                    this._mTimer = 0;
                    
                    // Apply the instantaneous transformations.
                    this._ApplyInstantaneousTransformations();
                    
                    // Take a snapshot of the actor's state.
                    this._TakeActorStateSnapshot();
                    
                    // Start updating the object.
                    this.actor.ActorComponent.OnTick.AddListener(call: this._Update);
                }
            }
        // -- Protected Methods --
        // -- Private Methods --
            // - Events -
            /// <summary>
            /// Advances onto the children <see cref="Transformation"/> in the tree.
            /// </summary>
            private void _Advance() {
                // Remove the listener.
                this.actor.ActorComponent.OnTick.RemoveListener(call: this._Update);
                
                // If there is a next transformation in the chain.
                if (this.nodes.Count > 0) {
                    // Loop through the next transformations.
                    foreach (Transformation transformation in this.nodes) {
                        // Apply the next transformation.
                        transformation.Apply();
                    }
                } else {
                    // Check if the current line is set.
                    if (Line.Current.HasValue) {
                        // Tell the line that the transformations are over.
                        Line.Current.Value.Advance();
                    } else {
                        // Throw an exception.
                        throw new InvalidOperationException(message: $"Transformation \"{this.identifier}\" has completed but it is not part of a Line object !");
                    }
                }
            }
            
            /// <summary>
            /// Method called by the <see cref="ActorComponent.OnTick"/> event.
            /// Used to apply the transformations to the actor.
            /// Calls <see cref="_Advance"/> once the <see cref="_mTimer"/> reaches <see cref="time"/>.
            /// </summary>
            /// <param name="deltaTime">The time since the last frame.</param>
            private void _Update(float deltaTime) {
                // Update the timer.
                this._mTimer += deltaTime;
                
                // Compute the normalized time.
                float normTime = this.time == 0 ? 1 : this._mTimer / this.time;
                // Apply the interpolated transformations.
                this._ApplyInterpolatedTransformations(normalizedTime: normTime);
                
                // Check if the time was reached.
                if (this._mTimer > this.time) {
                    // Call the advance method.
                    this._Advance();
                }
            }
            
            // - Helpers -
            /// <summary>
            /// Takes a snapshot of the <see cref="ActorComponent"/>'s <see cref="Color"/> and <see cref="Transform"/>.
            /// </summary>
            private void _TakeActorStateSnapshot() {
                // Copy the actor's colour.
                this._mStartingColour = this.actor.ActorComponent.GetColour();
                
                // Copy the actor's position.
                this._mStartingPosition = this.actor.ActorComponent.GetPosition();
                // Copy the actor's rotation.
                this._mStartingRotation = this.actor.ActorComponent.GetRotation();
                // Copy the actor's scale.
                this._mStartingScale = this.actor.ActorComponent.GetScale();
            }
            
            // - Appliers -
            /// <summary>
            /// Applies all the transformation types that are instantaneous.
            /// For now, these are:
            ///     - <see cref="ETransformationType.Pose"/>
            ///     - <see cref="ETransformationType.HorizontalFlip"/>
            ///     - <see cref="ETransformationType.VerticalFlip"/>
            /// </summary>
            private void _ApplyInstantaneousTransformations() {
                // Check if the pose update was specified.
                if (this.type.HasFlag(flag: ETransformationType.Pose)) {
                    // Update the pose of the actor.
                    this.actor.ActorComponent.UpdatePose(to: this.pose);
                }
                
                // Check if the horizontal flip was specified.
                if (this.type.HasFlag(flag: ETransformationType.HorizontalFlip)) {
                    this.actor.ActorComponent.SetHorizontalFlip(value: this.flipHorizontal);
                }
                
                // Check if the vertical flip was specified.
                if (this.type.HasFlag(flag: ETransformationType.VerticalFlip)) {
                    this.actor.ActorComponent.SetVerticalFlip(value: this.flipVertical);
                }
            }
            
            /// <summary>
            /// Applies all the transformations types that are interpolated across the entire transformation duration.
            /// For now, these are:
            ///     - <see cref="ETransformationType.Colour"/>
            ///     - <see cref="ETransformationType.Position"/>
            ///     - <see cref="ETransformationType.Rotation"/>
            ///     - <see cref="ETransformationType.Scale"/>
            /// </summary>
            private void _ApplyInterpolatedTransformations(float normalizedTime) {
                // Check if the colour update flag was set.
                if (this.type.HasFlag(flag: ETransformationType.Colour)) {
                    // Lerp the colour from the current to the target.
                    Color current = Color.Lerp(a: this._mStartingColour, b: this.colour, t: normalizedTime);
                    
                    // Apply the colour.
                    this.actor.ActorComponent.SetColour(colour: current);
                }
                
                // Check if the position update was specified.
                if (this.type.HasFlag(flag: ETransformationType.Position)) {
                    // Lerp the position from the current to the target.
                    Vector3 current = Vector3.Lerp(a: this._mStartingPosition, b: this.position, t: normalizedTime);
                    
                    // Apply the position.
                    this.actor.ActorComponent.SetPosition(position: current);
                }
                // Check if the rotation update was specified.
                if (this.type.HasFlag(flag: ETransformationType.Rotation)) {
                    // Lerp the rotation from the current to the target.
                    Quaternion current = Quaternion.Lerp(a: this._mStartingRotation, b: this.rotation, t: normalizedTime);
                    
                    // Apply the rotation.
                    this.actor.ActorComponent.SetRotation(rotation: current);
                }
                // Check if the scale update was specified.
                if (this.type.HasFlag(flag: ETransformationType.Scale)) {
                    // Lerp the scale from the current to the target.
                    Vector3 current = Vector3.Lerp(a: this._mStartingScale, b: this.scale, t: normalizedTime);
                    
                    // Apply the scale.
                    this.actor.ActorComponent.SetScale(scale: current);
                }
            }
    // --- /Methods ---
}
}