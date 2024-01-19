using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource _audioSourceMusic;

    [SerializeField]
    AudioSource _audioSourceEffects;

    [SerializeField]
    AudioClip _musicMenu;

    [SerializeField]
    AudioClip _musicGame;

    [SerializeField]
    AudioClip _musicEnd;

    [SerializeField]
    AudioClip _effectGrabPiece;

    [SerializeField]
    AudioClip _effectDrop;

    [SerializeField]
    AudioClip _effectUseBonusTile;

    [SerializeField]
    AudioClip _effectSwallow;

    [SerializeField]
    AudioClip _effectTimesUp;

    public void StartMusicEnd()
    {
        _audioSourceMusic.clip = _musicEnd;
        _audioSourceMusic.Play();
    }

    public void GrabPiece()
    {
        _audioSourceEffects.PlayOneShot(_effectGrabPiece);
    }

    public void DropPieceToBoard()
    {
        _audioSourceEffects.PlayOneShot(_effectDrop);
    }

    public void BonusTile()
    {
        _audioSourceEffects.PlayOneShot(_effectUseBonusTile);
    }

    public void Swallow()
    {
        _audioSourceEffects.PlayOneShot(_effectSwallow);
    }

    public void TimeUp()
    {
        _audioSourceEffects.PlayOneShot(_effectTimesUp);
    }

    public void StopMusic()
    {
        _audioSourceMusic.Stop();
    }
}
