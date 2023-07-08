using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

[Serializable]
// This will give the Randomizer a name in the UI which will be used when we add
// the Randomizer to our Pose Estimation Scenario
[AddRandomizerMenu("Perception/XYZ Translation Randomizer")]
public class XYZTranslationRandomizer
    : Randomizer // YRotationRandomizer class extends Randomizer, "Randomizer"
                 // is the base class for all randomizers
{
  // FloatParameter: contains a seeded random number generator, can be changed
  // from the editor
  public FloatParameter translationRange = new FloatParameter {
    value = new UniformSampler(-1.0f, 1.0f)
  }; // in range (0, 1)

  protected override void
  OnIterationStart() // Lifecycle method on all randomizers, called by the
                     // scenario every iteration
  {
    // tagManager: object available to every randomizer -> helps to find
    // GameObjects with this tag
    // Query tagManager to gather all references to the YRotationRandomizerTag
    // in the scenario
    IEnumerable<XYZTranslationRandomizerTag> tags =
        tagManager.Query<XYZTranslationRandomizerTag>();
    foreach (XYZTranslationRandomizerTag tag in tags) {
      float xTranslation =
          translationRange
              .Sample(); // gives us a random float in the specified range.
      float zTranslation =
          translationRange
              .Sample(); // gives us a random float in the specified range.

      // sets rotation
      tag.SetXYZTranslation(xTranslation, zTranslation);
    }
  }
}