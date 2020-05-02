// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;

/* Wrap the class within the local namespace. */
namespace Runtime.Actions {

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
            [System.AttributeUsageAttribute(validOn: System.AttributeTargets.Class, Inherited = false)]
            public class ActionControllerTypeAttribute: System.Attribute {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Reference to the type of the state.
                        /// </summary>
                        public readonly System.Type StateType;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        /// <summary>
                        /// Class constructor.
                        /// Creates a new instance of the <see cref="ActionControllerTypeAttribute"/>.
                        /// </summary>
                        /// <param name="stateType">The state type that is represented.</param>
                        public ActionControllerTypeAttribute(System.Type stateType) {
                            this.StateType = stateType;
                        }
                // --- /Methods ---
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
            private static System.Type[] _msActionControllerTypes;
            
            /// <summary>
            /// Dictionary of all the constructor objects for the specified action types.
            /// This is used to avoid having to search through the entire assembly
            /// every time the <see cref="_CreateActionController"/> method is called.
            /// </summary>
            private static System.Collections.Generic.Dictionary<string, System.Reflection.ConstructorInfo>
                _msStateConstructors = new System.Collections.Generic.Dictionary<string, System.Reflection.ConstructorInfo>();
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            // - Serialization Events -
            /// <summary>
            /// Serializes the specified <see cref="ActionState"/>.
            /// Sets the values of the <see cref="ActionState.ChildrenIndexList"/> array.
            /// </summary>
            /// <param name="owner">The owner of this action.</param>
            /// <param name="action">The action object to serialize.</param>
            public static void Serialize(Runtime.Directions.Scene.SceneState owner, ActionState action) {
                // Serialize the list of children.
                action.ChildrenIndexList = new int[action.ChildrenList.Count];
                for (int index = 0; index < action.ChildrenList.Count; index++) {
                    ActionState child = action.ChildrenList[index: index];
                    
                    // Get the index of the child.
                    int childIndex = System.Array.IndexOf(array: owner.ActionList, value: child);

                    // Ensure that the index is valid.
                    if (childIndex < 0 || childIndex > owner.ActionList.Length) {
                        // Log an error.
                        UnityEngine.Debug.LogError(message: $"There is an action that has an invalid child : {child}");
                    }

                    // Store the index in the list.
                    action.ChildrenIndexList[index] = childIndex;
                }
                
                // Clear the list of parameters.
                action.Parameters = new System.Collections.Generic.List<string>();
                // Serialize the parameters of the action.
                ActionController
                    ._CreateActionController(controller: action.ActionControllerName, state: action)
                    ._SaveParameters();
            }
            
            /// <summary>
            /// Deserializes the specified <see cref="ActionState"/>.
            /// Sets the values of the <see cref="ActionState.ChildrenList"/> array.
            /// </summary>
            /// <param name="owner">The owner of this action.</param>
            /// <param name="action">The action object to deserialize.</param>
            public static void Deserialize(Runtime.Directions.Scene.SceneState owner, ActionState action) {
                // Recreate the child list.
                action.ChildrenList = new System.Collections.Generic.List<ActionState>();
                
                // Loop through the children indices.
                foreach (int childIndex in action.ChildrenIndexList) {
                    // Ensure that the index is valid.
                    if (childIndex < 0 || childIndex > owner.ActionList.Length) {
                        // Log an error.
                        Runtime.Application.ApplicationView.Error(
                            message: "Action Deserialization: Found an action with an invalid child index",
                            details: $"Child index was #{childIndex}, the owner's action list " +
                                     $"is {owner.ActionList.Length} items long."
                        );
                    } else {
                        // Get the child at the specified index.
                        ActionState child = owner.ActionList[childIndex];
                        
                        // Add the child to the list of children.
                        action.ChildrenList.Add(item: child);
                        
                        // Increment the child's parent counter.
                        child.ParentCount++;
                    }
                }
                
                // Deserialize the parameters of the controller.
                ActionController
                    ._CreateActionController(controller: action.ActionControllerName, state: action)
                    ._LoadParameters();
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
                        ActionController controller = ActionController._CreateActionController(
                            controller: state.ActionControllerName, 
                            state: state
                        );
                        
                        // Wait 1 frame to ensure that the chain does not overflow the stack.
                        Runtime.Application.ApplicationController.OnNextTick.AddListener(
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
                System.Type stateClass =
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
                        return ActionController._CreateActionController(
                            controller: state.ActionControllerName, 
                            state: state
                        ) as TControllerType;
                    } else {
                        // Throw an error.
                        throw new System.InvalidOperationException(
                            message: $"Could not create the ActionController instance of {typeof(TControllerType).Name},"
                                   + $"The state class was not correct: \"{stateClass?.Name}\" vs {state.GetType()}"
                        );
                    }
                } else {
                    // Check if the state was created correctly.
                    if (stateClass
                        ?.GetConstructor(types: new System.Type[] {})
                        ?.Invoke(parameters: new object[] {}) 
                        is ActionState newState
                    ) {
                        // Set the state's controller name.
                        newState.ActionControllerName = typeof(TControllerType).FullName;
                        
                        // Call the controller constructor.
                        return ActionController._CreateActionController(
                            controller: newState.ActionControllerName,
                            state: newState
                        ) as TControllerType;
                    } else {
                        // Throw an error.
                        throw new System.InvalidOperationException(
                            message: $"Could not create the ActionController instance of {typeof(TControllerType).Name},"
                                   + $"The state class \"{stateClass?.Name}\" could not be instantiated."
                        );
                    }
                }
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
            protected void Finish() {
                // Check if there are children in the list.
                if (this.State.ChildrenList.Count == 0) {
                    // Log a warning.
                    UnityEngine.Debug.LogWarning(message: "There was an action in the tree that had no children.");
                }
                
                // Loop through the children actions.
                foreach (ActionState actionState in this.State.ChildrenList) {
                    // Apply the child action.
                    ActionController.Apply(state: actionState);
                }
            }
            
            // - Serialization -
            /// <summary>
            /// Method called right before serialization.
            /// Stores the parameters into the <see cref="ActionState.Parameters"/> array.
            /// </summary>
            protected abstract void _SaveParameters();
            
            /// <summary>
            /// Method called right after construction.
            /// Loads the parameters specified in the <see cref="ActionState.Parameters"/> array.
            /// </summary>
            protected abstract void _LoadParameters();
            
        // -- Private Methods --
            /// <summary>
            /// Creates a new <see cref="ActionController"/> instance.
            /// </summary>
            /// <param name="controller">The name of the controller for the specified action.</param>
            /// <param name="state">Optional state object to inject in the controller.</param>
            /// <returns>The created <see cref="ActionController"/> instance.</returns>
            private static ActionController _CreateActionController(string controller, ActionState state = null) {
                // Search in the dictionary first.
                if (ActionController._msStateConstructors.ContainsKey(key: controller)) {
                    // Invoke the constructor.
                    ActionController action = ActionController._msStateConstructors[key: controller]
                        .Invoke(parameters: new object[] {})
                        as ActionController;
                    
                    // Set the state of the action.
                    if (action != null) action.State = state;

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
                System.Type actionType = ActionController._msActionControllerTypes
                    .FirstOrDefault(predicate: childType => childType.FullName == controller);
                
                // Check if the action type was found.
                if (actionType == null) {
                    string actionList = string.Join(
                        separator: ",", 
                        values: ActionController._msActionControllerTypes.Select(selector: type => type.Name)
                    );
                    // Throw an error.
                    throw new System.InvalidOperationException(
                        message: $"Could not find an ActionController for the type {controller},"
                               + $"Searched through the list: ({actionList})"
                    );
                } else {
                    // Search for the constructor.
                    System.Reflection.ConstructorInfo constructor = actionType.GetConstructor(
                        types: new System.Type[]{}
                    );
                    // Check if the controller could be instantiated.
                    if (!(constructor?.Invoke(parameters: new object[] {}) is ActionController controllerInstance)) {
                        // Throw an error.
                        throw new System.InvalidOperationException(
                            message: $"Could not find an constructor for the action controller {actionType.Name}"
                        );
                    } else {
                        // Store the constructor in the dictionary.
                        ActionController._msStateConstructors.Add(key: controller, value: constructor);
                        
                        // Store the state of the controller.
                        controllerInstance.State = state;
                        
                        // Return the controller instance.
                        return controllerInstance;
                    }
                }
            }
    // --- /Methods ---
}
}