using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

public class YRotationRandomizerTag : RandomizerTag {
  private Vector3 originalRotation;

  private void Start() // automatically called once
  {
    originalRotation = transform.eulerAngles;
  }

  public void SetYRotation(float yRotation) {
    transform.eulerAngles =
        new Vector3(originalRotation.x, yRotation, originalRotation.z);
  }
}