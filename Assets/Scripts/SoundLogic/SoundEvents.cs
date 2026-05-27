using System;
public static class SoundEvents
{
    public static void PlaySFX(string sfx)
    {
        SoundManager.Instance.Play(sfx);
    }
}