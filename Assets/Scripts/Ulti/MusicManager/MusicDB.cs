using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDB : Singleton<MusicDB>
{
    public AudioClip Music_BG, Music_Shop;
    public AudioClip[] Music_Home;

    public AudioClip Music_Home_Random
    {
        get
        {
            return Music_Home[Random.Range(0, Music_Home.Length)];
        }
    }

    //public AudioClip Music_InGame, Music_Loading, SFX_button, Music_Lose, Music_Win;
    public AudioClip SFX_button, SFX_popup, SFX_Win1, SFX_Win2, SFX_Lose, SFX_Stun, SFX_Move, SFX_Elevator, SFX_Build_Finish;
    //public AudioClip SFX_EnemyDie;
}
