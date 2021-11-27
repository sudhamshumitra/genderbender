using UnityEngine;

public enum EInterest
{
    NONE,
    
    SKIING,
    BARS,
    BOARD_GAMES,
    HIKING_TRIPS,
    CONFIDENCE,
    
    MAX,
}

public enum EBasics
{
    NONE,
    
    ACTIVE,
    SOCIALLY,
    NEVER,
    
    MAX,
}

public class ProfileData
{
    public string Name;
    public string About;
    public EInterest[] Interests;
    public EBasics[] Basics;
}
