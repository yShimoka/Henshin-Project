// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Scene {

/// <summary>
/// Simple serializable class for the Start transformation controller.
/// Does not hold any data by itself.
/// </summary>
[Serializable]
public class Start: Transformation {
    public Start(Transformation from): base(@from: from) {}
    public Start() {}

}
}