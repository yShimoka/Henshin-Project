/*
 * Copyright © 2020 - Zimproov.
 */

using System;
using Henshin.Core.App;

/* Wrap the class within the local namespace. */
namespace Henshin.Core {

/// <summary>
/// Exception thrown if no <see cref="State"/> object is found within its resource folder.
/// </summary>
internal class MissingStateException: Exception {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new <see cref="MissingStateException"/> instance.
            /// Calls the base constructor.
            /// <seealso cref="Exception"/>
            /// </summary>
            /// <param name="message">The message that will be rendered to the user.</param>
            public MissingStateException(string message): base(message: message) {}
    // --- /Methods ---
}

/// <summary>
/// Exception thrown if the theatre scene is invalid.
/// </summary>
internal class InvalidTheatreException: Exception {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new <see cref="InvalidTheatreException"/> instance.
            /// Calls the base constructor.
            /// <seealso cref="Exception"/>
            /// </summary>
            /// <param name="message">The message that will be rendered to the user.</param>
            public InvalidTheatreException(string message): base(message: message) {}
    // --- /Methods ---
}


/// <summary>
/// Exception thrown if a prefab attribute is invalid.
/// </summary>
/// <typeparam name="TContainer"></typeparam>
internal class MissingPrefabException<TContainer>: Exception {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new <see cref="MissingPrefabException{TContainer}"/> instance.
            /// Calls the base constructor.
            /// <seealso cref="Exception"/>
            /// </summary>
            /// <param name="attributeName">The name of the missing attribute.</param>
            /// <param name="containerIdentifier">The identifier of the container object.</param>
            public MissingPrefabException(string attributeName, string containerIdentifier): base(
                message: $"The prefab attribute \"{attributeName}\" was not set in the \"#{containerIdentifier}\" {typeof(TContainer).FullName} instance."
            ) {}
    // --- /Methods ---
}

}