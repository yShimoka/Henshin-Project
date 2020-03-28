/*
 * Copyright © 2020 - Zimproov.
 */

using System;
using Henshin.Core.App;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor {

/// <summary>
/// Exception thrown if the requested asset could ne be found in the database.
/// </summary>
internal class AssetNotFoundException: Exception {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new <see cref="AssetNotFoundException"/> instance.
            /// Calls the base constructor.
            /// <seealso cref="Exception"/>
            /// </summary>
            /// <param name="message">The message that will be rendered to the user.</param>
            public AssetNotFoundException(string message): base(message: message) {}
    // --- /Methods ---
}

/// <summary>
/// Exception thrown if multiple instances of the asset were found in the database.
/// </summary>
internal class MultipleAssetException: Exception {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new <see cref="MultipleAssetException"/> instance.
            /// Calls the base constructor.
            /// <seealso cref="Exception"/>
            /// </summary>
            /// <param name="message">The message that will be rendered to the user.</param>
            public MultipleAssetException(string message): base(message: message) {}
    // --- /Methods ---
}

}