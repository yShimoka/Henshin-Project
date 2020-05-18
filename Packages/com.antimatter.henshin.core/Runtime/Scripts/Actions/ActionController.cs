// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Henshin.Runtime.Directions.Scene;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions {

/// <summary>
/// Controller class used to manipulate <see cref="ActionState"/> objects.
/// This class should be overriden to implement custom action behaviours.
/// </summary>
public abstract class ActionController {
    // ---  SubObjects --
        // -- Public Classes --
            /// <summary>
            /// Attribute used to determine which <see cref="ActionState"/>
            /// class is used by the <see cref="ActionController"/> class.
            /// </summary>
            [AttributeUsage(validOn: AttributeTargets.Class, Inherited = false)]
            public class ActionControllerTypeAttribute: Attribute {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Reference to the type of the state.
                        /// </summary>
                        public readonly Type StateType;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        /// <summary>
                        /// Class constructor.
                        /// Creates a new instance of the <see cref="ActionControllerTypeAttribute"/>.
                        /// </summary>
                        /// <param name="stateType">The state type that is represented.</param>
                        public ActionControllerTypeAttribute(Type stateType) {
                            this.StateType = stateType;
                        }
                // --- /Methods ---
            }
            
            /// <summary>
            /// Class used to determine the category of the <see cref="ActionState"/> class.
            /// Refers to the 
            /// </summary>
            public class ActionControllerCategoryAttribute: Attribute {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Category of the state object.
                        /// </summary>
                        public readonly EActionCategory Category;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        /// <summary>
                        /// Class constructor.
                        /// Creates a new instance of the <see cref="ActionControllerCategoryAttribute"/>.
                        /// </summary>
                        /// <param name="category">The category of the action.</param>
                        public ActionControllerCategoryAttribute(EActionCategory category) {
                            this.Category = category;
                        }
                // --- /Methods ---
            }
            
        // -- Public Enums --
            /// <summary>
            /// List of all the categories of actions.
            /// </summary>
            public enum EActionCategory {
                Start, End, Scene, Actor, Gameplay
            }
    // --- /SubObjects --
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Reference to the <see cref="ActionState"/> manipulated by this controller.
            /// </summary>
            public ActionState State;
            
        // -- Protected Attributes --
        // -- Private Attributes --
            // - Caches -
            /// <summary>
            /// Enumeration of all the <see cref="ActionController"/> child types.
            /// </summary>
            private static Type[] _msActionControllerTypes;
            
            /// <summary>
            /// Dictionary of all the constructor objects for the specified action types.
            /// This is used to avoid having to search through the entire assembly
            /// every time the <see cref="CreateController"/> method is called.
            /// </summary>
            private static readonly Dictionary<string, Tuple<ConstructorInfo, ConstructorInfo>> _msSTATE_CONSTRUCTORS = 
                new Dictionary<string, Tuple<ConstructorInfo, ConstructorInfo>>();
                
            // - Indices -
            /// <summary>
            /// Index of the object that is currently being deserialized.
            /// Used by <see cref="NextSerializedData{TDataType}"/>.
            /// </summary>
            private int _mCurrentSerializedObject;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - Serialization Events -
            /// <summary>
            /// Serializes the specified <see cref="ActionState"/>.
            /// Sets the values of the <see cref="ActionState.ChildrenIndexList"/> array.
            /// </summary>
            /// <param name="owner">The owner of this action.</param>
            /// <param name="action">The action object to serialize.</param>
            public static ActionState Serialize(SceneState owner, ActionState action) {
                // Clear the list of parameters.
                action.Parameters = new List<string>();
                // Serialize the parameters of the action.
                ActionController controller = ActionController
                    .CreateController(controller: action.ActionControllerName, state: action);
                controller.SaveParameters();
                
                // Return the new state object.
                return controller.State;
            }
            
            /// <summary>
            /// Deserializes the specified <see cref="ActionState"/>.
            /// Sets the values of the <see cref="ActionState.ChildrenList"/> array.
            /// </summary>
            /// <param name="owner">The owner of this action.</param>
            /// <param name="action">The action object to deserialize.</param>
            public static ActionState Deserialize(SceneState owner, ActionState action) {
                // Deserialize the parameters of the controller.
                ActionController controller = ActionController
                    .CreateController(controller: action.ActionControllerName, state: action);
                controller.LoadParameters();
                // Load the state of the new controller.
                action = controller.State;
                    
                // Return the new state.
                return action;
            }
            
            /// <summary>
            /// Loads all the children references.
            /// </summary>
            public static void LoadChildren(SceneState owner, ActionState action) {
                // Recreate the child list.
                action.ChildrenList = new List<ActionState>();
                
                // Loop through the children indices.
                foreach (int childIndex in action.ChildrenIndexList) {
                    // Ensure that the index is valid.
                    if (childIndex < 0 || childIndex >= owner.ActionList.Count) {
                        // Log an error.
                        Debug.LogError(
                            message: "Action Deserialization: Found an action with an invalid child index\n"
                                   + $"Child index was #{childIndex}, the owner's action list " +
                                     $"is {owner.ActionList.Count} items long."
                        );
                    } else {
                        // Get the child at the specified index.
                        ActionState child = owner.ActionList[index: childIndex];
                        
                        // Add the child to the list of children.
                        action.ChildrenList.Add(item: child);
                        
                        // Increment the child's parent counter.
                        child.ParentCount++;
                    }
                }
            }
            
            // - Controller Manipulation -
            /// <summary>
            /// Applies the specified action state.
            /// Starts the action chain from the specified starting point.
            /// </summary>
            /// <param name="state">The state of the </param>
            public static void Apply(ActionState state) {
                // Check if the state is valid.
                if (state != null) {
                    // Increment the state's counter.
                    state.ParentFinishedCounter++;
                    
                    // Check if the counter is greater than the number of parents.
                    if (state.ParentFinishedCounter >= state.ParentCount) {
                        // Create the state's controller.
                        ActionController controller = ActionController.CreateController(
                            controller: state.ActionControllerName, 
                            state: state
                        );
                        controller.LoadParameters();
                        
                        // Wait 1 frame to ensure that the chain does not overflow the stack.
                        Application.ApplicationController.OnNextTick.AddListener(
                            call: _ => { controller.Apply(); }
                        );
                    }
                }
            }
            
            /// <summary>
            /// Creates a new <see cref="ActionController"/> instance.
            /// </summary>
            /// <param name="state">The state of the controller. If null, creates a new state instance as well.</param>
            /// <typeparam name="TControllerType">The type of the controller to create.</typeparam>
            /// <returns>The created controller instance.</returns>
            public static TControllerType CreateController<TControllerType>(ActionState state = null) 
                where TControllerType: ActionController {
                // Get the controller type's state class.
                Type stateClass =
                    typeof(TControllerType)
                    .GetCustomAttributes(attributeType: typeof(ActionControllerTypeAttribute), inherit: false)
                    .Cast<ActionControllerTypeAttribute>()
                    .Select(selector: attribute => attribute.StateType)
                    .FirstOrDefault();
                    
                // Check if a state was specified.
                if (state != null) {
                    // Ensure that the state class is valid.
                    if (stateClass == state.GetType()) {
                        // Call the controller constructor.
                        return ActionController.CreateController(
                            controller: state.ActionControllerName, 
                            state: state
                        ) as TControllerType;
                    } else {
                        // Throw an error.
                        throw new InvalidOperationException(
                            message: $"Could not create the ActionController instance of {typeof(TControllerType).Name},"
                                   + $"The state class was not correct: \"{stateClass?.Name}\" vs {state.GetType()}"
                        );
                    }
                } else {
                    // Call the controller constructor.
                    return ActionController.CreateController(
                        controller: typeof(TControllerType).FullName
                    ) as TControllerType;
                }
            }
            
            // - Helper Methods -
            /// <summary>
            /// Returns the <see cref="EActionCategory"/> of the specified <see cref="ActionState"/>.
            /// </summary>
            /// <param name="action">The action to get the category of.</param>
            /// <returns>The category of the specified action's controller.</returns>
            public static EActionCategory GetCategory([NotNull]ActionState action) {
                // Check if the action's controller type is set.
                if (action.ControllerType == null) {
                    
                }
                
                // Find the attribute on the controller type.
                ActionControllerCategoryAttribute attribute = action.ControllerType
                    .GetCustomAttributes(attributeType: typeof(ActionControllerCategoryAttribute), inherit: false)
                    .Cast<ActionControllerCategoryAttribute>()
                    .ElementAtOrDefault(index: 0);
                
                // If no element was found.
                if (attribute == null) {
                    throw new InvalidOperationException(
                        message: $"The action controller {action.ActionControllerName} does not have a category."
                    );
                }
                
                // Return the attribute.
                return attribute.Category;
            }
            
        // -- Protected Methods --
            // - Play Handlers -
            /// <summary>
            /// Applies the action.
            /// This should be overloaded on every action object.
            /// </summary>
            protected abstract void Apply();
            
            /// <summary>
            /// Marks the action as being finished.
            /// Applies all the children actions.
            /// </summary>
            protected virtual void Finish() {
                // Check if there are children in the list.
                if (this.State.ChildrenList.Count == 0) {
                    // Log a warning.
                    Debug.LogWarning(message: "There was an action in the tree that had no children.");
                }
                
                // Loop through the children actions.
                foreach (ActionState actionState in this.State.ChildrenList) {
                    // Apply the child action.
                    ActionController.Apply(state: actionState);
                }
            }
            
            // - Serialization -
            /// <summary>
            /// Adds the specified data to the serialized array.
            /// </summary>
            /// <param name="data">The data to add to the array.</param>
            /// <typeparam name="TDataType">The type of the data object.</typeparam>
            protected void AddSerializedData<TDataType>(TDataType data) {
                // Convert the data to a string object.
                string serialized = Convert.ToString(value: data, provider: CultureInfo.InvariantCulture);
                
                // Store the data in the store.
                this.State.Parameters.Add(item: serialized);
            }
            
            /// <summary>
            /// Returns the next element in the serialized list.
            /// </summary>
            /// <typeparam name="TDataType">The expected type of the data.</typeparam>
            protected TDataType NextSerializedData<TDataType>() {
                // Check if the index is valid.
                if (this._mCurrentSerializedObject < 0 || this._mCurrentSerializedObject >= this.State.Parameters.Count) {
                    throw new IndexOutOfRangeException(message: $"There is no parameter at index {this._mCurrentSerializedObject}");
                }
                // Get the current data from the serialized list.
                string serialized = this.State.Parameters[index: this._mCurrentSerializedObject];
                
                // Increment the counter.
                this._mCurrentSerializedObject++;
                
                // Check if the data type is an enumerator.
                if (typeof(TDataType).IsEnum) {
                    // Convert to the target enum.
                    return (TDataType)Enum.Parse(enumType: typeof(TDataType), value: serialized);
                } else {
                    // Convert the data to the specified data type.
                    return (TDataType)Convert.ChangeType(
                        value: serialized, 
                        conversionType: typeof(TDataType), 
                        provider: CultureInfo.InvariantCulture
                    );
                }
            }
            
            /// <summary>
            /// Method called right before serialization.
            /// Stores the parameters into the <see cref="ActionState.Parameters"/> array.
            /// </summary>
            protected abstract void SaveParameters();
            
            /// <summary>
            /// Method called right after construction.
            /// Loads the parameters specified in the <see cref="ActionState.Parameters"/> array.
            /// </summary>
            protected abstract void LoadParameters();
            
        // -- Private Methods --
            /// <summary>
            /// Creates a new <see cref="ActionController"/> instance.
            /// </summary>
            /// <param name="controller">The name of the controller for the specified action.</param>
            /// <param name="state">Optional state object to inject in the controller.</param>
            /// <returns>The created <see cref="ActionController"/> instance.</returns>
            public static ActionController CreateController(string controller, ActionState state = null) {
                // Search in the dictionary first.
                if (ActionController._msSTATE_CONSTRUCTORS.ContainsKey(key: controller)) {
                    // Invoke the controller constructor.
                    ActionController action = ActionController._msSTATE_CONSTRUCTORS[key: controller].Item1
                        .Invoke(parameters: new object[] {})
                        as ActionController;
                    
                    // Check if the state's type is already valid.
                    if (state != null && state.GetType() == ActionController._msSTATE_CONSTRUCTORS[key: controller].Item2.DeclaringType) {
                        // Store the state.
                        action.State = state;
                    } else {
                        // Invoke the state constructor.
                        action.State = ActionController._msSTATE_CONSTRUCTORS[key: controller].Item2
                            .Invoke(parameters: new object[] {})
                            as ActionState;
                            
                        // Check if a state was specified.
                        if (state != null) {
                            action.State.Parameters = state.Parameters;
                            action.State.ChildrenList = state.ChildrenList;
                            
                            // Create the children list array.
                            action.State.ChildrenIndexList = state.ChildrenIndexList;
                        }
                        action.State.ActionControllerName = controller;
                        action.State.ControllerType = action.GetType();
                    }

                    // Return the action.
                    return action;
                }
                
                // If the list of action controllers is empty.
                if (ActionController._msActionControllerTypes == null) {
                    // Load the action controller child classes.
                    ActionController._msActionControllerTypes = typeof(ActionController)
                        .Assembly
                        .GetTypes()
                        .Where(predicate: type => type.IsSubclassOf(c: typeof(ActionController)))
                        .ToArray();
                }
                
                // Find the action type that has the specified state type.
                Type actionType = ActionController._msActionControllerTypes
                    .FirstOrDefault(predicate: childType => childType.FullName == controller);
                
                // Check if the action type was found.
                if (actionType == null) {
                    string actionList = string.Join(
                        separator: ",", 
                        values: ActionController._msActionControllerTypes.Select(selector: type => type.Name)
                    );
                    // Throw an error.
                    throw new InvalidOperationException(
                        message: $"Could not find an ActionController for the type {controller},"
                               + $"Searched through the list: ({actionList})"
                    );
                } else {
                    // Search for the constructor.
                    ConstructorInfo controllerConstructor = actionType.GetConstructor(
                        types: new Type[]{}
                    );
                    // Check if the controller could be instantiated.
                    if (!(controllerConstructor?.Invoke(parameters: new object[] {}) is ActionController controllerInstance)) {
                        // Throw an error.
                        throw new InvalidOperationException(
                            message: $"Could not find an constructor for the action controller {actionType.Name}"
                        );
                    } else {
                        // Find the state constructor.
                        ConstructorInfo stateConstructor = actionType
                            .GetCustomAttributes(attributeType: typeof(ActionControllerTypeAttribute), inherit: false)
                            .Cast<ActionControllerTypeAttribute>()
                            .ElementAtOrDefault(index: 0)
                            ?.StateType
                            .GetConstructor(types: new Type[] {});
                        
                        // Check if the state constructor was found.
                        if (stateConstructor == null) throw new InvalidOperationException(
                            message: $"Could not find a state for the controller type {controller}"
                        );
                        
                        // Store the constructors in the dictionary.
                        ActionController._msSTATE_CONSTRUCTORS.Add(
                            key: controller, 
                            value: new Tuple<ConstructorInfo, ConstructorInfo>(
                                item1: controllerConstructor,
                                item2: stateConstructor
                            )
                        );
                        
                        // Store the state of the controller.
                        controllerInstance.State = stateConstructor.Invoke(parameters: new object[] {}) as ActionState;
                        
                        // If a state was specified.
                        if (state != null) {
                            // Copy the state info.
                            controllerInstance.State.ChildrenIndexList = state.ChildrenIndexList;
                            controllerInstance.State.Parameters = state.Parameters;
                            controllerInstance.State.ChildrenList = state.ChildrenList;
                        }
                        controllerInstance.State.ControllerType = actionType;
                        controllerInstance.State.ActionControllerName = controller;
                        
                        // Return the controller instance.
                        return controllerInstance;
                    }
                }
            }
    // --- /Methods ---
}
}