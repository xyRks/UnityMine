using System;
using System.IO;
using UnityEngine;

// Класс для хранения всех данных игры
[System.Serializable]
public class GameData
{
    // Позиция игрока в мире
    public Vector3 position;

    // Данные инвентаря (например, идентификаторы или названия предметов)
    public string[] inventory;

    // Состояние ресурсов
    public int gold;
    public int wood;
    public int stone;

    // Состояние карты (например, массив открытых зон)
    public bool[] mapZonesDiscovered;

    // Конструктор с начальными (пустыми) значениями
    public GameData()
    {
        position = Vector3.zero;
        inventory = new string[0];
        gold = 0;
        wood = 0;
        stone = 0;
        mapZonesDiscovered = new bool[0];
    }
}

// Менеджер сохранений (навешивается на объект в сцене)
public class SaveManager : MonoBehaviour
{
    // Текущие данные игры
    public GameData currentData = new GameData();

    // Название файла для сохранения в один слот
    private string saveFileName = "savegame.json";

    // Получить полный путь к файлу сохранения на жестком диске
    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }

    // Сохранить текущий прогресс в файл
    public void SaveGame()
    {
        string path = GetSavePath();

        try
        {
            // Преобразуем объект данных в JSON с форматированием для читаемости
            string json = JsonUtility.ToJson(currentData, true);

            // Записываем JSON-строку в локальный файл
            File.WriteAllText(path, json);

            Debug.Log("Игра сохранена. Путь: " + path);
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка при сохранении игры: " + e.Message);
        }
    }

    // Загрузить прогресс из файла
    public void LoadGame()
    {
        string path = GetSavePath();

        // Если файл существует, загружаем данные
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                currentData = JsonUtility.FromJson<GameData>(json);
                Debug.Log("Игра успешно загружена.");
            }
            catch (Exception e)
            {
                Debug.LogError("Ошибка при загрузке игры: " + e.Message);
            }
        }
        else
        {
            // Если файла нет, начинаем с чистого листа
            Debug.Log("Сохранение не найдено. Начинаем новую игру.");
            currentData = new GameData();
        }
    }

    // Сбросить прогресс и удалить файл сохранения
    public void ResetProgress()
    {
        string path = GetSavePath();

        // Удаляем файл сохранения, если он существует
        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
                Debug.Log("Файл сохранения удален.");
            }
            catch (Exception e)
            {
                Debug.LogError("Ошибка при удалении файла сохранения: " + e.Message);
            }
        }

        // Сбрасываем данные в памяти
        currentData = new GameData();
        Debug.Log("Прогресс игры полностью сброшен.");
    }
}
