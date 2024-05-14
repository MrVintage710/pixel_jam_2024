using System.Dynamic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve waveThreatCurve;

    [SerializeField]
    float maximumSpawnInterval = 3; //in seconds
    [SerializeField]
    float spawnDistance = 10; //in meters

    [SerializeField]
    float waveInterval = 5; //Rest time between waves.

    //Properties
    public WavePhase CurrentPhase { get; private set; }

    //Private Variables
    private int currentWave = 0;
    private float intensityModifier; //I'm going to try sqrt of current wave.
    private float waveStartTime = 0;
    private float unspentThreatPoints;

    public void Start()
    {
        
    }

    public void Update()
    {
        //Behavior depends on what phase the manager is in.
        switch (CurrentPhase)
        {
            case WavePhase.Rest:
                if(Time.time > waveStartTime)
                {
                    BeginWave();
                }
                break;
            case WavePhase.Active:
                ProcessThreat();
                break;
            case WavePhase.End:
                if (true) //Todo: wait for all enemies to die.
                {
                    Debug.Log($"Wave {currentWave} complete.");
                    CurrentPhase = WavePhase.Rest;
                    waveStartTime = Time.time + waveInterval;
                }
                break;
            default:
                waveStartTime = Time.time;
                BeginWave();
                break;
        }
    }

    private void BeginWave()
    {
        currentWave++;
        Debug.Log($"Wave {currentWave} has begun.");
        CurrentPhase = WavePhase.Active;
        intensityModifier = Mathf.Sqrt(currentWave);
        unspentThreatPoints = 0; //These should already be 0 or close to.
    }

    private void ProcessThreat()
    {
        float threatPerSecond = intensityModifier * waveThreatCurve.Evaluate(Time.time - waveStartTime);
        if(threatPerSecond <= 0)
        {
            //The wave has reached its climax. Spend all threat points and move to ending.
            SpawnEnemyGroup(Mathf.FloorToInt(unspentThreatPoints));
            unspentThreatPoints = 0;
            CurrentPhase = WavePhase.End;
        }
        else
        {
            unspentThreatPoints += Time.deltaTime * threatPerSecond;

            if (unspentThreatPoints >= maximumSpawnInterval * threatPerSecond)
            {
                 //Spend Points
                int pointsToSpend = Mathf.FloorToInt(Random.Range(1, unspentThreatPoints));
                unspentThreatPoints -= pointsToSpend;
                SpawnEnemyGroup(pointsToSpend);
            }
        }



    }

    #region Spawning
    public void SpawnEnemyGroup(int pointsToSpend)
    {
        //Choose Approach Vector
        float spawnAreaRadius = Mathf.Sqrt(pointsToSpend); //Area scales approximately with number of enemies.
        Vector2 spawnAreaCenter = ((spawnDistance + spawnAreaRadius) * Random.insideUnitCircle.normalized) + GameManager.playerPosition; //Choose a point on the spawn ring.

        //Spwan Enemies
        for(int i = 0; i < pointsToSpend; i++)
        {
            Vector2 spawnPosition = spawnAreaCenter + (spawnAreaRadius * Random.insideUnitCircle); //Choose a random spot in the spawn area. Boid spacing should do the rest.
            SpawnAnEnemy(spawnPosition);
        }
        //Debug.Log($"{pointsToSpend} enemies spawned.");
    }

    public void SpawnAnEnemy(Vector2 spawnPosition)
    {
        GameManager.SpawnEnemy(this, 15 * intensityModifier, 5.0f * intensityModifier, Vector2.zero, spawnPosition);
    }
    #endregion

    public enum WavePhase
    {
        None,
        Active,
        End,
        Rest
    }
}

