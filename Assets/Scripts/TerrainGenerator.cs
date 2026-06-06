using UnityEngine;

// Обязательно наличие компонента Terrain на этом же объекте
[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{
    // Строковое значение для генерации уникального мира (сид)
    public string seed = "MyWorldSeed";

    // Масштаб шума, чем меньше, тем "крупнее" холмы
    public float scale = 20f;

    void Start()
    {
        GenerateTerrain();
    }

    // Метод генерации ландшафта на основе сида
    private void GenerateTerrain()
    {
        // Получаем компонент Terrain, прикрепленный к объекту
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        // Превращаем строковый сид в числовой хэш
        // Это число будет использоваться как сдвиг координат для шума Перлина
        int seedHash = seed.GetHashCode();
        float offsetX = seedHash % 100000f; // Ограничиваем значение для удобства
        float offsetY = (seedHash / 100000f) % 100000f; // Делаем сдвиг по другой оси

        // Получаем разрешение карты высот
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;

        // Создаем двумерный массив высот
        float[,] heights = new float[width, height];

        // Проходим по каждой точке карты высот
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Вычисляем координаты для шума Перлина со сдвигом от сида
                // Делим на width/height и умножаем на scale для настройки размера холмов
                float xCoord = (float)x / width * scale + offsetX;
                float yCoord = (float)y / height * scale + offsetY;

                // Получаем значение шума (от 0.0 до 1.0) и записываем в массив высот
                heights[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }

        // Применяем сгенерированные высоты к нашему Terrain
        terrainData.SetHeights(0, 0, heights);
    }
}
