using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Zone { Street, DanceClub, Garden, Mansion, MansionBackyard, Roof }
public class ZoneTransitionManager : MonoBehaviour, IRechargeable
{
    ZoneExclusiveLoop[] zoneLoops;
    [SerializeField] AudioSource bossMusic;
    readonly List<AudioSource> enemiesAudioSources = new();
    Zone currentZone;

    private void Start()
    {
        zoneLoops = FindObjectsByType<ZoneExclusiveLoop>(FindObjectsSortMode.InstanceID);
        Zone startZone = GameObject.FindGameObjectWithTag("StartingBonfire").GetComponent<Bonfire>().Zone;
        foreach (ZoneExclusiveLoop zone in zoneLoops)
        {
            if (zone.Zone != startZone)
            {
                zone.Stop();
            }
        }

        foreach (var enemy in FindObjectsOfType<Enemy>())
            if (enemy.TryGetComponent(out AudioSource audioSource)) enemiesAudioSources.Add(audioSource);
    }

    public void EnterZone(Zone zone)
    {
        foreach (ZoneExclusiveLoop zoneLoop in zoneLoops)
        {
            if (zoneLoop.Zone == zone)
                zoneLoop.StartFadeIn();
            else
                zoneLoop.StartFadeOut();
        }
        currentZone = zone;
    }

    public void FadeEverythingOut()
    {
        foreach (ZoneExclusiveLoop zoneLoop in zoneLoops)
            zoneLoop.StartFadeOut();

        StartCoroutine(AudioFadeUtils.FadeOut(bossMusic, 1, true));

        foreach (AudioSource audioSource in enemiesAudioSources)
            StartCoroutine(AudioFadeUtils.FadeOut(audioSource, 1, true));

    }

    public void FadeEnemiesIn()
    {
        foreach (AudioSource audioSource in enemiesAudioSources)
            StartCoroutine(AudioFadeUtils.FadeIn(audioSource, 1));
    }

    public void FadeNonBossesOut()
    {
        foreach (AudioSource audioSource in enemiesAudioSources)
        {
            if (audioSource.TryGetComponent(out BossHealth _)) continue;
            StartCoroutine(AudioFadeUtils.FadeOut(audioSource, 1, true));
        }
    }

    public void Recharge(RechargeType rechargeType = RechargeType.Respawn)
    {
        FadeEnemiesIn();
    }

    public void FadeCurrentZone()
    {
        foreach (ZoneExclusiveLoop zoneLoop in zoneLoops)
        {
            if (zoneLoop.Zone == currentZone)
                zoneLoop.StartFadeOut();

        }
    }
}
