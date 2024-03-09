using UnityEngine;

namespace Autophobia.PlayerComponents
{
    public static class CursorChangeState
    {
        public static void SetState(bool isLocked, bool visible)
        {
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = visible;
        }
    }
}