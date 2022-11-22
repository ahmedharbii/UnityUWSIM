using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

public class XYZTranslationRandomizerTag : RandomizerTag
{
    private Vector3 originalTranslation;

    private void Start() //automatically called once
    {
        originalTranslation = transform.position;
    }

    public void SetXYZTranslation(float xTranslation, float zTranslation)
    {
        transform.position = new Vector3(xTranslation, originalTranslation.y, zTranslation);
    }
}

// https://docs.unity3d.com/ScriptReference/Transform.html