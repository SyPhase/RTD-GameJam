using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] List<BotLogic> _bots = new List<BotLogic>();
    [SerializeField] List<Spawner> _spawnPoints = new List<Spawner>();

    [SerializeField] GameObject _botPrefab;
    [SerializeField] bool _canExpand = false;
    [SerializeField] int _botsToPool = 10;

    void Start()
    {
        for (int i = 0; i < _botsToPool; i++)
        {
            // Make new Bots as children of this
            GameObject currentBot = Instantiate(_botPrefab, transform);

            // Bots Start Disabled
            currentBot.SetActive(false);

            // Add each Bot to the list
            _bots.Add(currentBot.GetComponent<BotLogic>());
        }

        StartCoroutine(SpawnBots(6));
    }

    IEnumerator SpawnBots(int numberOfBotsToSpawn)
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < numberOfBotsToSpawn; i++)
        {
            // Wait before activating the next bot
            yield return new WaitForSeconds(1f);

            // Get a free spawn point's index
            int spawnIndex = GetFreeSpawnPoint();

            // If no spawns are free, try again
            if (spawnIndex < 0)
            {
                i--;
                continue;
            }

            // Get a bot from the pool
            BotLogic currentBot = GetPooledBot();

            // If all bots are in play, stop spawning bots
            if (currentBot == null)
            {
                yield break;
            }

            // Move currentBot to the free spawn
            currentBot.transform.position = _spawnPoints[spawnIndex].transform.position;

            // Choose a random direction for the bot to go
            float direction;
            if (Random.value > 0.5f)
            {
                direction = 1f;
            }
            else
            {
                direction = -1f;
            }
            currentBot.SetTargetDirection(direction);

            // Activate the bot
            currentBot.gameObject.SetActive(true);
        }
    }

    int GetFreeSpawnPoint()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i].IsAvailable())
            {
                return i;
            }
        }

        return -1;
    }

    public BotLogic GetPooledBot()
    {
        // Return Bots if NOT active
        for (int i = 0; i < _bots.Count; i++)
        {
            if (!_bots[i].gameObject.activeInHierarchy)
            {
                return _bots[i];
            }
        }

        // if all pooled objects in use, check if can expand pool
        if (_canExpand)
        {
            // Create new Bot as child of something
            GameObject currentBot = Instantiate(_botPrefab, transform);

            // Bots Start Disabled
            currentBot.SetActive(false);

            // Add new Bot to the list
            _bots.Add(currentBot.GetComponent<BotLogic>());

            return _bots[_bots.Count];
        }
        else
        {
            return null;
        }
    }

    public void DisableBots()
    {
        for (int i = 0; i < _bots.Count; i++)
        {
            if (_bots[i].gameObject.activeInHierarchy)
            {
                _bots[i].gameObject.SetActive(false);
            }
        }
    }
}