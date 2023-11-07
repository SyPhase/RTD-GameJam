using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] bool _isUnobstructed = true;
    [SerializeField] bool _isNotOnCooldown = true;

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
        if (_isUnobstructed && _isNotOnCooldown)
        {
            StartCoroutine(SpawnCooldown());

            return true;
        }

        return false;
    }

    IEnumerator SpawnCooldown()
    {
        _isNotOnCooldown = false;

        yield return new WaitForSeconds(2f);

        _isNotOnCooldown = true;
    }
}