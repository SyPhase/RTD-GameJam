using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BotManager : MonoBehaviour
{
    [SerializeField] int _botsToSpawn = 1;

    [SerializeField] List<BotLogic> _bots = new List<BotLogic>();
    [SerializeField] List<Spawner> _spawnPoints = new List<Spawner>();

    [SerializeField] GameObject _botPrefab;
    [SerializeField] bool _canExpand = false;
    [SerializeField] int _botsToPool = 10;

    bool _isSpawningBots = false;
    int _roundCount = 1;

    TMP_Text _roundText;

    void Start()
    {
        _roundText = FindObjectOfType<TMP_Text>();

        for (int i = 0; i < _botsToPool; i++)
        {
            // Make new Bots as children of this
            GameObject currentBot = Instantiate(_botPrefab, transform);

            // Bots Start Disabled
            currentBot.SetActive(false);

            // Add each Bot to the list
            _bots.Add(currentBot.GetComponent<BotLogic>());
        }

        StartCoroutine(SpawnBots(_botsToSpawn));
    }

    IEnumerator SpawnBots(int numberOfBotsToSpawn)
    {
        // Bot Spawning status to true
        _isSpawningBots = true;

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
                direction = 0.65f;
            }
            else
            {
                direction = -0.65f;
            }
            currentBot.SetTargetDirection(direction);

            // Choose random height for bot to focus
            float height;
            int randomValue = Random.Range(1, 4);
            if (randomValue == 1)
            {
                height = 2.5f;
            }
            else if(randomValue == 2)
            {
                height = 6.2f;
            }
            else
            {
                height = 9f;
            }
            currentBot.SetTargetHeight(height); // 2.5f, 6.2f, 9f

            // Activate the bot
            currentBot.gameObject.SetActive(true);
        }

        // Bot Spawning Status to false
        _isSpawningBots = false;

        // Increment number of bots next wave
        _botsToSpawn++;
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
        else // Cannot expand list and all bots are active
        {
            return null;
        }
    }

    public void TryStartNewWave()
    {
        // Check if bots are still being spawned
        if (_isSpawningBots)
        {
            return;
        }

        // Check if all bots are diabled
        for (int i = 0; i < _bots.Count; i++)
        {
            if (_bots[i].gameObject.activeInHierarchy)
            {
                return;
            }
        }

        // Add 1 to round count
        _roundCount++;
        _roundText.text = "Round: " + _roundCount;

        // If all bots are diabled, start new wave
        StartCoroutine(SpawnBots(_botsToSpawn));
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