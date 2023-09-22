using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    //album variables
    public Albums[] music;
    private int albumIndex;
    private int musicIndex;

    //Aduio variables
    public AudioMixer mixer;
    private AudioSource musicSource;
    private bool foundClip;

    //Slider variables
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambientSlider;

    //UI variables
    public RawImage albumImage;
    public TMP_Text albumText;
    public TMP_Text musicText;

    //Set the starting audioSource and play music
    private void Start()
    {
        musicSource = GetComponent<AudioSource>();

        foundClip = false;
        //Find the first available clip in the music list and set that to play
        foreach (Albums album in music)
        {
            foreach (AudioClip clip in album.albumMusic)
            {
                if (clip)
                {
                    musicSource.clip = clip;
                    foundClip = true;

                    albumText.text = album.albumName;
                    musicText.text = clip.name;
                    musicIndex = Array.IndexOf(album.albumMusic, clip);
                    break;
                }
            }
            if (foundClip)
            {
                albumImage.texture = album.albumCover.texture;
                albumIndex = Array.IndexOf(music, album);
                musicSource.Play();
                break;
            }
        }

        //Set the volume sliders & values
        if (PlayerPrefs.HasKey("masterVol"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVol");
            mixer.SetFloat("masterVol", (masterSlider.value / 100) * 80 - 80);
        }
        if (PlayerPrefs.HasKey("musicVol"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVol");
            mixer.SetFloat("musicVol", (musicSlider.value / 100) * 80 - 80);
        }
        if (PlayerPrefs.HasKey("sfxVol"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVol");
            mixer.SetFloat("sfxVol", (sfxSlider.value / 100) * 80 - 80);
        }
        if (PlayerPrefs.HasKey("ambientVol"))
        {
            ambientSlider.value = PlayerPrefs.GetFloat("ambientVol");
            mixer.SetFloat("ambientVol", (ambientSlider.value / 100) * 80 - 80);
        }
    }

    private void Update()
    {
        //If the music has stopped, play the next in the list
        if (!musicSource.isPlaying)
        {
            if (musicIndex >= music[albumIndex].albumMusic.Length - 1)
                SearchForMusic(true);
            else
                SearchForMusic(false);
        }

        //When a change in teh sliders occur, update the mix group to the same value
        masterSlider.onValueChanged.AddListener(delegate { AudioValueChange("masterVol", masterSlider); });
        musicSlider.onValueChanged.AddListener(delegate { AudioValueChange("musicVol", musicSlider); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioValueChange("sfxVol", sfxSlider); });
        ambientSlider.onValueChanged.AddListener(delegate { AudioValueChange("ambientVol", ambientSlider); });
    }

    //Change audio values function
    private void AudioValueChange(string mixerGroup, Slider slider)
    {
        mixer.SetFloat(mixerGroup, (slider.value / 100) * 80 - 80);
        PlayerPrefs.SetFloat(mixerGroup, slider.value);
    }

    //Search for music function
    private void SearchForMusic(bool albumEnd)
    {
        foundClip = false;
        //Check if we are at the end of the album
        if (albumEnd)
        {
            //If there are no more albums, search for a new album with music from the start of the music list.
            if (albumIndex >= music.Length - 1)
            {
                //Find the first available piece of music from the start of the music list
                foreach (Albums album in music)
                {
                    foreach (AudioClip clip in album.albumMusic)
                    {
                        if (clip)
                        {
                            musicSource.clip = clip;
                            foundClip = true;

                            albumText.text = album.albumName;
                            musicText.text = clip.name;
                            musicIndex = Array.IndexOf(album.albumMusic, clip);
                            break;
                        }
                    }
                    if (foundClip)
                    {
                        albumImage.texture = album.albumCover.texture;
                        albumIndex = Array.IndexOf(music, album);
                        musicSource.Play();
                        break;
                    }
                }
            }
            //If there are more albums, search for a new album with music from the next album in the music list
            else
            {
                //Find the next available piece of music from music list
                foreach (Albums album in music)
                {
                    if (Array.IndexOf(music, album) > albumIndex)
                    {
                        foreach (AudioClip clip in album.albumMusic)
                        {

                            if (clip)
                            {
                                musicSource.clip = clip;
                                foundClip = true;

                                albumText.text = album.albumName;
                                musicText.text = clip.name;
                                musicIndex = Array.IndexOf(album.albumMusic, clip);
                                break;
                            }
                        }
                        if (foundClip)
                        {
                            albumImage.texture = album.albumCover.texture;
                            albumIndex = Array.IndexOf(music, album);
                            musicSource.Play();
                            break;
                        }
                    }
                }
                //If a piece of music could not be found, search for the first available piece of music in the music list
                if (!foundClip)
                {
                    //Find the first available piece of music from the start of the music list
                    foreach (Albums album in music)
                    {
                        foreach (AudioClip clip in album.albumMusic)
                        {
                            if (clip)
                            {
                                musicSource.clip = clip;
                                foundClip = true;

                                albumText.text = album.albumName;
                                musicText.text = clip.name;
                                musicIndex = Array.IndexOf(album.albumMusic, clip);
                                break;
                            }
                        }
                        if (foundClip)
                        {
                            albumImage.texture = album.albumCover.texture;
                            albumIndex = Array.IndexOf(music, album);
                            musicSource.Play();
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            //Find the next available piece of music in the current album
            foreach (AudioClip clip in music[albumIndex].albumMusic)
            {
                if (Array.IndexOf(music[albumIndex].albumMusic, clip) > musicIndex)
                {
                    if (clip)
                    {
                        musicSource.clip = clip;
                        foundClip = true;

                        albumText.text = music[albumIndex].albumName; ;
                        musicIndex = Array.IndexOf(music[albumIndex].albumMusic, clip);
                        musicText.text = clip.name;
                        albumImage.texture = music[albumIndex].albumCover.texture;
                        break;
                    }
                }
            }
            //If a piece of music can not be found, search for a new piece of music from other albums
            if (!foundClip)
            {
                Debug.Log("No more music in the album");
                SearchForMusic(true);
            }
        }

        //Play the music source
        musicSource.Play();
    }
}

//Albums class
[Serializable]
public class Albums
{
    public string albumName;
    public Sprite albumCover;
    public AudioClip[] albumMusic;
}