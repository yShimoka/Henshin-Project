// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Henshin.Components.Scene.Scenery;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery.Transformation {

/// <summary>
/// Attribute used to find which transformation to deserialize in the <see cref="Base.Deserialize"/> method.
/// </summary>
[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class TransformationTypeAttribute: Attribute {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Reference to the serialized type of this class.</summary>
            public Type SerializedType;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Instantiate the attribute object.
            /// </summary>
            /// <param name="serializedType">The serialized type used for this class.</param>
            public TransformationTypeAttribute(Type serializedType) {
                this.SerializedType = serializedType;
            }
    // --- /Methods ---
}

/// <summary>
/// 
/// </summary>
public abstract class Base {
    // ---  Types ---
        // -- Public Types --
            /// <summary>
            /// Class used to represent a serialized <see cref="Base"/> object.
            /// Should be overloaded in every transformation child class.
            /// </summary>
            [Serializable]
            public abstract class Serialized {
                /// <summary>Unique identifier of the transformation.</summary>
                public string identifier;
                
                /// <summary>List of all the indices for this class's children.</summary>
                public List<int> nodeIndices;
                
                /// <summary>Reference to the Actor transformed.</summary>
                public Actor actor;
            }
    // --- /Types ---
    
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
            // - Debugging Parameters -
            /// <summary>Unique identifier of the transformation.</summary>
            public string Identifier;
            
            // - Tree Behaviour -
            /// <summary>List of all the children nodes.</summary>
            public List<Base> Nodes;
            
            /// <summary>Stores the number of parents of this transformation.</summary>
            public int ParentCount;
            
            // - Transformation Parameters -
            /// <summary>Reference to the Actor transformed.</summary>
            public Actor Actor;
            
        // -- Protected Attributes --
            /// <summary>Helper to access the actor component.</summary>
            protected ActorComponent ActorComponent => this.Actor.ActorComponent;
            
        // -- Private Attributes --
            /// <summary>Counts the number of times that the <see cref="Apply"/> method was called.</summary>
            private int _mApplyCalls;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            // - Transformation Methods -
            /// <summary>Applies the transformation.</summary>
            public void Apply() {
                // Increment the counter.
                this._mApplyCalls++;
                
                // If all parents are finished.
                if (this._mApplyCalls > this.ParentCount) {
                    // Apply the transformation.
                    this._ApplyTransformation();
                } 
            }
            
            // - Serialization -
            /// <summary>
            /// Serializes the object into a <see cref="Serialized"/> object.
            /// </summary>
            public Serialized Serialize(List<Base> unfoldedTree) {
                // Serialize the properties.
                Serialized serialized = this._Serialize();
                
                // Serialize all the nodes.
                foreach (Base node in this.Nodes) {
                    serialized.nodeIndices.Add(item: unfoldedTree.IndexOf(item: node));
                }
                
                // Return the serialized object.
                return serialized;            
            }
            
            /// <summary>
            /// Unfolds the reference tree into a simple list of <see cref="Base"/> elements.
            /// </summary>
            /// <param name="unfoldedList">List of all the <see cref="Base"/> transformations.</param>
            public void Unfold(List<Base> unfoldedList) {
                // Loop through the list of nodes in this tree.
                foreach (Base node in this.Nodes.Where(predicate: node => !unfoldedList.Contains(item: node))) {
                    // Unfold the node.
                    node.Unfold(unfoldedList: unfoldedList);
                }

                // Add the node to the list.
                unfoldedList.Add(item: this);
            }
            
            /// <summary>
            /// Folds the list of references into a tree.
            /// </summary>
            public static Base Fold(List<Serialized> unfoldedTree, List<Base> deserialized, int current = -1) {
                // If the current node is not set.
                if (current == -1) {
                    // Get the root of the tree.
                    current = unfoldedTree.Count - 1;
                }
                
                // Fold the current node.
                foreach (int index in unfoldedTree[index: current].nodeIndices) {
                    // Fold the node.
                    deserialized[index: current].Nodes.Add(item: Base.Fold(unfoldedTree: unfoldedTree, deserialized: deserialized, current: index));
                    deserialized[index: index].ParentCount += 1;
                }
                
                // Return the current node.
                return deserialized[index: current];
            }
            
            /// <summary>
            /// De-serializes the <see cref="Serialized"/> object.
            /// </summary>
            public static Base Deserialize(Serialized serialized) {
                // Get the type of the serialized object.
                Type serializedType = serialized.GetType();
                
                // Get the first class with the correct transformationType.
                Type serializedClass = Assembly
                    .GetAssembly(type: typeof(Base))
                    .GetTypes()
                    .FirstOrDefault(predicate: type => type
                        .GetCustomAttributes(attributeType: typeof(TransformationTypeAttribute))
                        .FirstOrDefault(predicate: attribute => (attribute as TransformationTypeAttribute).SerializedType == serializedType) 
                        != null
                    );
                    
                // If the class was found.
                if (serializedClass != null) {
                    // Create a new instance from the class.
                    Base instance = serializedClass.GetConstructor(types: new Type[] {}).Invoke(parameters: new object[] {}) as Base;
                    
                    // Call the deserialize method on the class.
                    instance._Deserialize(serialized: serialized);
                    
                    // Get the actor object.
                    instance.Actor = serialized.actor;
                    // Get the identifier.
                    instance.Identifier = serialized.identifier;
                    
                    // Return the instance.
                    return instance;
                } else {
                    throw new InvalidOperationException(message: $"Could not found a transformation serialized with a {serializedType.FullName} object.");
                }
            }
            
        // -- Protected Methods --
            // - Transformation Manipulation -
            /// <summary>Finish the transformation.</summary>
            protected virtual void _Finish() {
                // Loop through the children nodes.
                foreach (Base child in this.Nodes) {
                    // Apply the transformation.
                    this.ActorComponent.StartCoroutine(routine: this._ApplyOnNextUpdate(node: child));
                }
            }
            
            /// <summary>Starts to apply the transformation.</summary>
            protected abstract void _ApplyTransformation();
            
            // - Serialization -
            /// <summary>Serializes the base object.</summary>
            protected abstract Serialized _Serialize(Serialized current = null);
            
            /// <summary>Deserializes the base object.</summary>
            protected abstract void _Deserialize(Serialized serialized);
            
        // -- Private Methods --
            /// <summary>Waits for the next update before calling the <see cref="_ApplyTransformation"/> method.</summary>
            private IEnumerator<WaitForFixedUpdate> _ApplyOnNextUpdate(Base node) {
                // Wait for a fixed update.
                yield return new WaitForFixedUpdate();
                
                // Call the apply method.
                node._ApplyTransformation();
            }
    // --- /Methods ---
}
}