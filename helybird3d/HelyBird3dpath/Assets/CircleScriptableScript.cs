using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Circle")]
public class CircleScriptableScript : ScriptableObject
{
    public GameObject[] meshes;
    public float[] rotationZ;
}
