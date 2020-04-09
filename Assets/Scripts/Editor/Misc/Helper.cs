// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Linq;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Misc {

/// <summary>
/// Miscellaneous helper class.
/// </summary>
public static class Helper {
    // ---  Methods ---
        // -- Public Methods --
            // - Project Handling -
            /// <summary>
            /// Creates the requested folder.
            /// Checks if the folder creation is required.
            /// </summary>
            /// <param name="folderPath">The path to the folder to create.</param>
            public static void CreateFolder(string folderPath) {
                // Split the path.
                string[] folders = folderPath.Split('/');
                
                // Loop through the folders.
                string parentFolder = "";
                foreach (string folder in folders) {
                    // Check if the folder exists.
                    if (!AssetDatabase.IsValidFolder(path: parentFolder)) {
                        // Create the folder.
                        AssetDatabase.CreateFolder(parentFolder: parentFolder, newFolderName: folder);
                    }
                    
                    // Update the parent folder value.
                    parentFolder = string.IsNullOrEmpty(value: parentFolder) ? folder : parentFolder + "/" + folder;
                }
            }
            
            /// <summary>
            /// Creates a new physical asset object.
            /// Creates the <see cref="folder"/> folder if need be.
            /// </summary>
            /// <param name="obj">The object to create an asset for.</param>
            /// <param name="folder">The folder that will hold the object.</param>
            public static void CreateAsset(Object obj, string folder) {
                // Create the folder if necessary.
                Helper.CreateFolder(folderPath: $"Assets/{folder}");
                
                // Create the asset.
                AssetDatabase.CreateAsset(asset: obj, path: $"Assets/{folder}/{obj.name}.asset");
            }
            
            /// <summary>
            /// Loads all the assets of the specified type with the specified parameters.
            /// </summary>
            /// <param name="name">(Optional) The name of the asset searched.</param>
            /// <param name="filter">(Optional) Additional filter parameters to the asset search.</param>
            /// <param name="baseFolders">(Optional) A list of folder to search into.</param>
            /// <typeparam name="T">The type of the asset to load.0</typeparam>
            /// <returns></returns>
            public static T[] LoadAssets<T>(string name = "", string filter = "", params string[] baseFolders) where T : Object {
                // Prepend "Assets/" to all the base folders.
                baseFolders = baseFolders.Select(selector: folder => $"Assets/{folder}").ToArray();
                
                // Get the asset guids with the specified filter.
                string[] guids = AssetDatabase.FindAssets(filter: $"{name} t:{typeof(T).Name} {filter}", searchInFolders: baseFolders);
                
                // Return the assets.
                return guids.Select(selector: AssetDatabase.GUIDToAssetPath).Select(selector: AssetDatabase.LoadAssetAtPath<T>).ToArray();
            }
            
            // - String Helpers -
            /// <summary>Prettifies a camelCase string.</summary>
            /// <param name="camel">The base string to prettify.</param>
            /// <returns>The prettified string.</returns>
            public static string PrettifyCamel(string camel) {
                // If the string is null or less than 1 letter, just return it.
                if (camel == null) { return null; }
                if (camel.Length < 2) { return camel; }
                
                // Capitalize the first character.
                string result = camel.Substring(startIndex: 0, length: 1).ToUpper();
                
                // Loop through the string.
                for (int i = 1; i < camel.Length; i++) {
                    // If the character is an uppercase character.
                    if (char.IsUpper(c: camel[index: i])) {
                        // Add a white space.
                        result += ' ';
                    }
                    
                    // Add the char to the result.
                    result += camel[index: i];
                }
                
                // Return the result.
                return result;
            }
        // -- Private Methods --
    // --- /Methods ---
}
}