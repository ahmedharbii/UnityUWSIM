using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

public class CamRandomizerTag : RandomizerTag {
  private Vector3 originalTranslation;
  private Vector3 originalRotation;

  private void Start() // automatically called once
  {
    originalTranslation = transform.position;
    originalRotation = transform.eulerAngles;
  }

  public void SetCamTranslation(float xTranslation, float yTranslation) {
    transform.position =
        new Vector3(originalTranslation.x, yTranslation, originalTranslation.z);
    transform.eulerAngles =
        new Vector3(originalRotation.x, originalRotation.y, originalRotation.z);
  }
}

// https://docs.unity3d.com/ScriptReference/Transform.html