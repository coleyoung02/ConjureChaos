using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using UnityEngine.SocialPlatforms;

public class DiscordController : MonoBehaviour
{
    private const long CLIENT_ID = 1346000285901783072;
    private Discord.Discord discord;

    [Space]
    private string details = "Conjuring Chaos";
    [Space]
    private string largeImage = "cccow";
    private string largeText = "Conjure Chaos";

    private long time;

    private bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            discord = new Discord.Discord(CLIENT_ID, (UInt64)Discord.CreateFlags.NoRequireDiscord);
        }
        catch
        {
        }
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    void OnApplicationQuit()
    {
        StopAllCoroutines();
        if (discord != null)
        {
            discord.Dispose();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (discord == null)
        {
            if (!waiting)
            {
                waiting = true;
                StartCoroutine(ReTryDiscordConnect());
            }
        }
        else
        {
            try
            {
                MainUpdate();
                discord.RunCallbacks();
            }
            catch
            {
                discord = null;
            }
        }
    }

    private IEnumerator ReTryDiscordConnect()
    {
        yield return new WaitForSecondsRealtime(10f);
        try
        {
            discord = new Discord.Discord(CLIENT_ID, (UInt64)Discord.CreateFlags.NoRequireDiscord);
        }
        catch
        {
        }
        waiting = false;
    }

    private void MainUpdate()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            Details = details,
            Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
            Timestamps =
                {
                    Start = time
                }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Discord.Result.Ok) Debug.LogError("Failed connecting to Discord!");
        });


    }
}