using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    AudioClip[] _effectGrabPieceArray;

    int grabPieceIndex = 0;


    public void StartMusicEnd()
    {
        _audioSourceMusic.clip = _musicEnd;
        _audioSourceMusic.Play();
    }

    public void GrabPiece()
    {
        _audioSourceEffects.PlayOneShot(_effectGrabPieceArray[grabPieceIndex]);

        grabPieceIndex++; // Move to the next sound
        if (grabPieceIndex >= _effectGrabPieceArray.Length) // If at the end of the array, wrap to start
        {
            grabPieceIndex = 0;
        }    
        
    }

    public void DropPieceToBoard()
    {
        _audioSourceEffects.PlayOneShot(_effectGrabPieceArray[grabPieceIndex]);

        grabPieceIndex++; // Move to the next sound
        if (grabPieceIndex >= _effectGrabPieceArray.Length) // If at the end of the array, wrap to start
        {
            grabPieceIndex = 0;
        }    
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

    public AudioSource musicSource; // Assign this in the inspector
    public AudioSource effectsSource; // Assign this in the inspector

    public Image musicImage; // Reference to the SpriteRenderer component for music
    public Image effectsImage; // Reference to the SpriteRenderer component for effects

    public Sprite muteSprite; // Sprite to display when muted
    public Sprite unmuteSprite; // Sprite to display when not muted

    // Mute Music
    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute; // Toggle the mute state of music
        // Update the sprite based on the musicSource's mute state
        musicImage.sprite = musicSource.mute ? muteSprite : unmuteSprite;
    }

    // Mute Sound Effects
    public void ToggleEffectsMute()
    {
        effectsSource.mute = !effectsSource.mute; // Toggle the mute state of effects
        // Update the sprite based on the musicSource's mute state
        effectsImage.sprite = effectsSource.mute ? muteSprite : unmuteSprite;
    }

}
