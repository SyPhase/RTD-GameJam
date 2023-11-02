using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    bool _isUnobstructed = true;
    bool _isNotOnCooldown = true;

    int _triggersEntered = 0;

    void OnTriggerEnter(Collider other)
    {
        _triggersEntered += 1;
        _isUnobstructed = false;
    }

    void OnTriggerExit(Collider other)
    {
        _triggersEntered -= 1;

        if (_triggersEntered == 0)
        {
            _isUnobstructed = true;
        }
    }

    public bool IsAvailable()
    {
        StartCoroutine(SpawnCooldown());

        return _isUnobstructed && _isNotOnCooldown;
    }

    IEnumerator SpawnCooldown()
    {
        _isNotOnCooldown = false;

        yield return new WaitForSeconds(2f);

        _isNotOnCooldown = true;
    }
}