using System;
using UnityEngine;

[Serializable]
public struct ProfileImage
{
    public string profileText;
    public Sprite content;
    [Range(5, 50)]
    public int position;
    public Vector4 xywh;
}