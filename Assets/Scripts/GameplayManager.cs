using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    int CubesCount = 500;
    [SerializeField]
    GameObject cubePrefab;
    [SerializeField]
    GameObject gameOverCanvas;

    float actorSize = 1f;

    public void SetCubes(int amount) { CubesCount = amount; }

    bool[,] slots;
    int slotsX;
    int slotsY;

    public static GameplayManager Instance;
    public static System.Action<Actor> actorDestroyed;
    public static System.Action<Actor> actorSpawned;


    void Start()
    {
        actorDestroyed += OnActorDestroyed;
        actorSpawned += OnActorSpawned;
        Instance = this;

    }

    public void StartGame()
    {
        actors = new List<Actor>();
        GenerateSlotsArray();
        SpawnCubes();
    }


    public void SpawnCubes()
    {
        for (int i = 0; i < CubesCount; i++)
        {
            SpawnCubeAtRandomLocation();
        }
    }


    void SpawnCubeAtRandomLocation()
    {
        Vector3 randomPosition = RandomLocation();
        Instantiate(cubePrefab).transform.position = randomPosition;
    }



    void GenerateSlotsArray()
    {
        float boardX = 30;
        float boardY = 30;
        slotsX = (int)(boardX / actorSize);
        slotsY = (int)(boardY / actorSize);
        slots = new bool[slotsX, slotsY];

        for (int i = 0; i < slotsX; i++)
            for (int j = 0; j < slotsY; j++)
                slots[i, j] = true;
    }

    Vector3 RandomLocation()
    {
        float randomX = Random.Range(0, 30);
        float randomY = Random.Range(0, 30);

        if (slots[(int)randomX, (int)randomY])
        {
            slots[(int)randomX, (int)randomY] = false;
            return new Vector3((int)randomX, 0, randomY) + new Vector3(-15, 0, -15);
        }
        else
        {

            return RandomLocation();
        }
    }

    public void RespawnMe(Actor toRespawn)
    {
        StartCoroutine(Respawner(toRespawn));
    }

    IEnumerator Respawner(Actor toRespawn)
    {
        yield return new WaitForSeconds(2f);
        toRespawn.Activate();
    }



    List<Actor> actors = new List<Actor>();

    void OnActorDestroyed(Actor actor)
    {
        if (actors.Contains(actor))
            actors.Remove(actor);
        Debug.Log($"Actors alive: {actors.Count}");
        if (actors.Count == 1)
            Win();
    }

    void Win()
    {
        Destroy(actors[0]);
        gameOverCanvas.SetActive(true);
    }

    void OnActorSpawned(Actor actor)
    {
        actors.Add(actor);
    }
}
