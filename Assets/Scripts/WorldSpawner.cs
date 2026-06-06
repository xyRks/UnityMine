using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    [Header("Forest Biome Prefabs")]
    public GameObject forestTreePrefab;
    public GameObject forestStonePrefab;
    public GameObject forestOrePrefab;
    public GameObject forestHousePrefab;

    [Header("Meadow Biome Prefabs")]
    public GameObject meadowTreePrefab;
    public GameObject meadowStonePrefab;
    public GameObject meadowOrePrefab;
    public GameObject meadowHousePrefab;

    [Header("Spawn Settings")]
    public int numberOfObjectsToSpawn = 100;
    public float spawnAreaWidth = 200f;
    public float spawnAreaLength = 200f;

    void Start()
    {
        SpawnWorldObjects();
    }

    private void SpawnWorldObjects()
    {
        // Кэшируем Terrain.activeTerrain перед циклом для повышения производительности
        Terrain currentTerrain = Terrain.activeTerrain;

        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            // Случайный выбор биома (0 - лес, 1 - поляна)
            int biomeType = Random.Range(0, 2);

            // Случайный выбор типа объекта (0 - дерево, 1 - камень, 2 - руда, 3 - дом)
            int objectType = Random.Range(0, 4);

            GameObject prefabToSpawn = GetPrefab(biomeType, objectType);

            if (prefabToSpawn != null)
            {
                // Случайные координаты X и Z в пределах зоны спавна
                float x = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
                float z = Random.Range(-spawnAreaLength / 2f, spawnAreaLength / 2f);

                // Определение высоты Y с помощью закэшированного террейна
                float y = 0f;
                // Внедрена строгая Null-Safety для террейна
                if (currentTerrain != null)
                {
                    y = currentTerrain.SampleHeight(new Vector3(x, 0, z));
                }
                else
                {
                     // Fallback, если террейна нет, можно спавнить на высоте 0 или вывести предупреждение
                     Debug.LogWarning("WorldSpawner: Terrain.activeTerrain не найден. Объекты будут созданы на высоте Y=0.");
                }

                Vector3 spawnPosition = new Vector3(x, y, z);

                // Создание объекта, проверка Instantiate на null (хотя для GameObject это редкость, но требуется по ТЗ)
                GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                if (spawnedObject == null)
                {
                     Debug.LogError("WorldSpawner: Ошибка при создании объекта.");
                }
            }
        }
    }

    private GameObject GetPrefab(int biomeType, int objectType)
    {
        if (biomeType == 0) // Лес
        {
            switch (objectType)
            {
                case 0: return forestTreePrefab;
                case 1: return forestStonePrefab;
                case 2: return forestOrePrefab;
                case 3: return forestHousePrefab;
            }
        }
        else // Поляна
        {
            switch (objectType)
            {
                case 0: return meadowTreePrefab;
                case 1: return meadowStonePrefab;
                case 2: return meadowOrePrefab;
                case 3: return meadowHousePrefab;
            }
        }

        return null;
    }
}
