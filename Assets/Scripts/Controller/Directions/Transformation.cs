// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using Henshin.Controller.Directions.Transformations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions {

/// <summary>
/// Attribute used to determine the state of a transformation controller.
/// </summary>
[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false)]
public class TransformationStateAttribute: Attribute {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Stores the type of the transformation state.</summary>
            public Type StateType;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="TransformationStateAttribute"/> instance.
            /// </summary>
            /// <param name="stateType">The type of the state for the qualified transformation class.</param>
            public TransformationStateAttribute(Type stateType) {
                // Ensure that the specified state type is a transformation state.
                if (stateType.IsSubclassOf(c: typeof(State.Directions.Transformation))) {
                    // Store the type.
                    this.StateType = stateType;
                } else {
                    // Throw an argument exception.
                    throw new ArgumentException(message: "The state type for a TransformationStateAttribute object MUST be a transformation state subclass !", paramName: nameof(stateType));
                }
            } 
    // --- /Methods ---
}

/// <summary>
/// 
/// </summary>
public abstract class Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            // - State Info -
            /// <summary>Stores a reference to this transformation's state.</summary>
            public State.Directions.Transformation State;
            
            // - Tree Behaviour -
            /// <summary>Reference to all the children node for this <see cref="Transformation"/>.</summary>
            public readonly List<Transformation> Nodes = new List<Transformation>();
            
            /// <summary>Stores the number of <see cref="Transformation"/> parents.</summary>
            [NonSerialized]
            public int ParentCount;
            
        // -- Private Attributes --
            /// <summary>Counter of all the parent that have declared the action as finished.</summary>
            private int _mParentFinished;
            
            /// <summary>Index of the current object in the state's encoded data.</summary>
            private int _mCurrentSerializationIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Transformation"/> instance with the specified state.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            protected Transformation(State.Directions.Transformation state) {
                // Store the state object.
                this.State = state;
                
                // Set the controller of the state object.
                state.controller = this.GetType();
            }
            
        // -- Public Methods --
            // - Transformation -
            /// <summary>
            /// Applies the transformation to the actor.
            /// Waits for all parents to call <see cref="Apply"/> before wrapping to <see cref="_Apply"/>.
            /// Waits for the next fixed update as well.
            /// </summary>
            public void Apply() {
                // Increment the parent counter.
                this._mParentFinished++;
                
                // If all parents have finished.
                if (this._mParentFinished >= this.ParentCount) {
                    // Wait for a fixed update.
                    Henshin.View.Application.ExecuteOnNextUpdate(method: this._Apply);
                }
            }
            
            // - Serialization -
            /// <summary>
            /// De-serializes the specified <see cref="Henshin.State.Directions.Transformation"/> object.
            /// </summary>
            /// <param name="serialized">The serialized object.</param>
            /// <returns>The de-serialized controller.</returns>
            public static Transformation Deserialize(State.Directions.Transformation serialized) {
                // Find the constructor of the controller.
                if (Transformation.CreateTransformation(type: serialized.controller, source: serialized) is Transformation controller) {
                    // Deserialize the controller.
                    controller._mCurrentSerializationIndex = -1;
                    controller._Deserialize();
                    // Return the instance.
                    return controller;
                } else {
                    // Throw an error
                    throw Application.Error(message: $"Could not deserialize controller type {serialized.controller?.Name}");
                }
            }
            
            /// <summary>
            /// Serializes the object into a serializable <see cref="Henshin.State.Directions.Transformation"/> object.
            /// </summary>
            /// <returns>The state of the controller.</returns>
            public Transformation Serialize() { this.State.encodedData.Clear(); this._Serialize(); return this; }
            
            /// <summary>
            /// Rebuilds the transformation tree from the specified transformation list.
            /// </summary>
            /// <param name="from">The list to unfold into a tree.</param>
            /// <returns>The root node of the tree.</returns>
            public static Transformation RebuildTree(List<Transformation> from) {
                // Seek the Start transformation in the tree.
                if (!(from.FirstOrDefault(predicate: transformation => transformation.State.controller == typeof(Henshin.Controller.Directions.Transformations.Scene.Start)) is Henshin.Controller.Directions.Transformations.Scene.Start start)) {
                    Debug.LogWarning(message: "Could not find a Start transformation when rebuilding a transformation tree.");
                    return null;
                }
                
                // Loop through the list.
                foreach (Transformation transformation in from) {
                    // Get the children of the transformation.
                    foreach (Transformation child in transformation.State.nodeIndices.Select(selector: index => @from[index: index])) {
                        // Increment its parent counter.
                        child.ParentCount++;
                        // Add the transformation at that index to the transformation nodes.
                        transformation.Nodes.Add(item: child);
                    }
                }
                
                // Return the start object.
                return start;
            }
            
            // - Object Manipulation -
            
            /// <summary>
            /// Creates a new transformation from the specified instance.
            /// </summary>
            /// <param name="type">The type of the expected transformation object.</param>
            /// <param name="source">The source data found in the base class.</param>
            /// <returns>The generated transformation object.</returns>
            public static Transformation CreateTransformation(Type type, State.Directions.Transformation source = null) {
                // Get the TransformationStateAttribute from the type.
                if (!(type.GetCustomAttributes(attributeType: typeof(TransformationStateAttribute), inherit: false) is TransformationStateAttribute[] attributes) || attributes.Length == 0) {
                    // Show an error.
                    throw Application.Error(message: $"Type {type.Name} does not implement the TransformationStateAttribute.");
                }
                
                // Get the type of the state for the controller.
                if (!(
                    attributes[0].StateType
                    .GetConstructor(types: source == null ? new Type[] { } : new[] { typeof(State.Directions.Transformation) })?
                    .Invoke(parameters: source == null ? new object[] { } : new object[] { source }) 
                    is State.Directions.Transformation state
                )) {
                    // Show an error.
                    throw Application.Error(message: $"Transformation State type {attributes[0].StateType?.Name} could not be instantiated.");
                }
                
                // Create a new transformation instance.
                if (!(
                    type
                    .GetConstructor(types: new[] { attributes[0].StateType })?
                    .Invoke(parameters: new object[] { state })
                    is Transformation controller
                )) {
                    // Show an error.
                    throw Application.Error(message: $"Transformation Controller type {type.Name} could not be instantiated.");
                }
                
                // Return the controller.
                return controller;
            }
            
            /// <summary>
            /// Creates a new Transformation object.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static T CreateTransformation<T>(State.Directions.Transformation source = null) where T : Transformation {
                // Wrap the base method.
                return (T) Transformation.CreateTransformation(type: typeof(T), source: source);
            }
            
        // -- Protected Methods --
            // - Transformation Manipulation -
            /// <summary>
            /// Applies the transformation to the specified <see cref="Henshin.State.Scenery.Actor"/>.
            /// This method does not get triggered before all of its parents called the <see cref="_Finish"/> method.
            /// </summary>
            protected abstract void _Apply();
            
            /// <summary>
            /// Marks the current transformation as finished.
            /// </summary>
            protected void _Finish() {
                // Go through all the children transformations.
                foreach (Transformation transformation in this.Nodes) {
                    // Apply the child.
                    transformation.Apply();
                }
            }
            
            /// <summary>Method called when the object is about to be serialized.</summary>
            protected virtual void _Serialize() {}
            
            /// <summary>Method called when the object is about to be deserialized.</summary>
            protected virtual void _Deserialize() {}
            
            /// <summary>Returns the next serialized string object.</summary>
            protected string _GetNextSerializedString() {
                this._mCurrentSerializationIndex++;
                return this.State.encodedData[index: this._mCurrentSerializationIndex];
            }
            
            /// <summary>Adds the string to the encoded array.</summary>
            protected void _AddSerializedString(string serialized) {
                this.State.encodedData.Add(item: serialized);
            }
    // --- /Methods ---
}
}