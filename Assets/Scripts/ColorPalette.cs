using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New ColorPalette", menuName ="New ColorPalette/palette")]
public class ColorPalette : ScriptableObject
{
    public Color[] brickColor = new Color[5];
    public Color damageColor = new Color();

}
