using System;

[Serializable]
public struct PlayerData
{
    public PlayerMovementData MovementData;
}

[Serializable]
public struct PlayerMovementData
{
    public float MoveSpeed;
    public float JumpForce;
}