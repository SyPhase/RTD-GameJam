using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    List<BotLogic> _bots = new List<BotLogic>();

    void Start()
    {
        BotLogic[] bots = FindObjectsOfType<BotLogic>(true);

        foreach (BotLogic bot in bots)
        {
            _bots.Add(bot);
        };
    }
}