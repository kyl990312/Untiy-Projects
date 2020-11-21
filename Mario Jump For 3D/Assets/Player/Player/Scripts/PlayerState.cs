
using System;

namespace Player.Scripts
{
    [Serializable]
    public enum PlayerState
    {
        Idle,
        Run,
        Dash,
        Jump,
        Attack,
        Fall,
        Dead
    }
}

