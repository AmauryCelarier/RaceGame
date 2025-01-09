using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class RaceData
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Quaternion> rotations = new List<Quaternion>();
    public List<float> timestamps = new List<float>();
}