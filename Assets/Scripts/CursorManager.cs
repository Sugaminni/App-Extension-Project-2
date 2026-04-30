using UnityEngine;

public static class CursorManager
{
    public static bool IsUIOpen { get; private set; } = true;

    public static void SetUIMode()
    {
        IsUIOpen = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public static void SetGameplayMode()
    {
        IsUIOpen = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}