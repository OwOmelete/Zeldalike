using System;
public static class SoundEvents
{
    public static void PlayJump()
    {
        SoundManager.Instance.Play("jump");
    }

    public static void PlayExplosion()
    {
        SoundManager.Instance.Play("explosion");
    }

    public static void PlayButtonClick()
    {
        SoundManager.Instance.Play("button_click");
    }
}