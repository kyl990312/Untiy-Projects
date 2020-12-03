
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
        Skill,
        Fall,
        Dead
    }
}

