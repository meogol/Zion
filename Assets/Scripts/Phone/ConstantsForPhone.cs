using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ratios
{
    public const float OfficeAlfa = 2.46f;
    public const float OfficeBeta = 29.53f;
    public const float OfficeGamma = 2.38f;

    public const float CorridorAlfa = 2.77f;
    public const float CorridorBeta = 29.27f;
    public const float CorridorGamma = 2.48f;

    public const float IndustrialAlfa = 3.79f;
    public const float IndustrialBeta = 21.01f;
    public const float IndustrialGamma = 1.34f;
}

public enum TypeOfBuilding
{
    Office = 0,
    Corridor,
    Industrial
}
