using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerformanceChecker : MonoBehaviour
{
    [Header("Base Settings"), Tooltip("How frequently the values update. (Uses seconds as a value)")]
    [SerializeField] float updateFrequency;
    [Tooltip("How frequently the worst FPS value updates. (Uses seconds as a value)")]
    [SerializeField] float updateWorstFrequency;

    [Header("Extra Visuals")]
    [SerializeField] bool colorFPSBasedOnValue;
    [SerializeField] bool colorAvaregeBasedOnValue;
    [SerializeField] Gradient performanceGradient;

    [Header("UI Setup")]
    [SerializeField] TextMeshProUGUI currentFPSText;
    [SerializeField] TextMeshProUGUI avarageFPSText;
    [SerializeField] TextMeshProUGUI bestFPSText;
    [SerializeField] TextMeshProUGUI worstFPSText;

    float invisTimer = 0f;
    float invisWorstTimer = 0f;
    int invisPointer = 0;
    float bestFPS = 0f;
    //big value so it auto updates the first time the values get calculated
    float worstFPS = 10000f;
    float avarageFPS = 0f;
    //used for stroning recent values to calculate an avarage
    List<float> lastTenFPS = new List<float>();

    void Update()
    {
        invisWorstTimer += Time.deltaTime;
        invisTimer += Time.deltaTime;
        if (invisTimer >= updateFrequency)
        {
            if (invisWorstTimer >= updateWorstFrequency)
            {
                invisWorstTimer = 0;
                worstFPS = 10000;
            }

            invisTimer = 0;
            float currentFPS = 1f / Time.smoothDeltaTime;

            if (currentFPS > bestFPS)
            {
                bestFPS = currentFPS;
            }

            if (worstFPS > currentFPS)
            {
                worstFPS = currentFPS;
                invisWorstTimer = 0;
            }

            if (avarageFPSText != null)
            {
                if (lastTenFPS.Count == 10)
                {
                    lastTenFPS[invisPointer] = currentFPS;
                    invisPointer++;
                    if(invisPointer == 10)
                    {
                        invisPointer = 0;
                    }
                }
                else
                {
                    lastTenFPS.Add(currentFPS);
                }

                
                float temp = 0;
                foreach (var fpsCount in lastTenFPS)
                {
                    temp += fpsCount;
                }
                avarageFPS = temp / lastTenFPS.Count;                
            }

            if (colorFPSBasedOnValue)
            {
                currentFPSText.color = performanceGradient.Evaluate(GetFPSBetweenMinMax(currentFPS, worstFPS, bestFPS));
            }
            if (colorAvaregeBasedOnValue)
            {
                avarageFPSText.color = performanceGradient.Evaluate(GetFPSBetweenMinMax(avarageFPS, worstFPS, bestFPS));
            }


            if (currentFPSText != null)
            {
                currentFPSText.text = "FPS: " + currentFPS.ToString("F0");                
            }
            if (avarageFPSText != null)
            {
                avarageFPSText.text = "AVG: " + avarageFPS.ToString("F0");                
            }
            if (bestFPSText != null)
            {
                bestFPSText.text = "TOP: " + bestFPS.ToString("F0");
            }
            if (worstFPSText != null)
            {
                worstFPSText.text = "LOW: " + worstFPS.ToString("F0");
            }
        }
    }

    float GetFPSBetweenMinMax(float currentFPS, float lowestFPS, float biggestFPS)
    {
        if (currentFPS < lowestFPS)
        {
            currentFPS = lowestFPS;
        }
        else if (currentFPS > biggestFPS)
        {
            currentFPS = biggestFPS;
        }
        if (lowestFPS == 0 && biggestFPS == 0)
        {
            return 1f;
        }
        else
        {
            return (currentFPS - lowestFPS) / (biggestFPS - lowestFPS);
        }
    }  
}
