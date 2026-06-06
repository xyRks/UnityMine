using UnityEngine;
using System.IO;

/// <summary>
/// Менеджер сохранений (PC Standalone).
/// Сохраняет данные в локальный файл через Application.persistentDataPath.
/// </summary>
public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    void Awake()
    {
        // Путь для сохранения данных на жестком диске ПК (надежно)
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log("SaveManager: Файл сохранения находится по пути: " + saveFilePath);
    }

    /// <summary>
    /// Сохраняет здоровье игрока.
    /// </summary>
    /// <param name="healthComponent">Компонент здоровья игрока.</param>
    public void SaveHealth(Health healthComponent)
    {
        if (healthComponent == null) return;

        // Простой пример сохранения данных через JSON
        SaveData data = new SaveData();
        data.playerHealth = healthComponent.currentHealth;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Игра сохранена.");
    }

    /// <summary>
    /// Загружает здоровье игрока.
    /// </summary>
    /// <param name="healthComponent">Компонент здоровья игрока, куда загрузить данные.</param>
    public void LoadHealth(Health healthComponent)
    {
        if (healthComponent == null) return;

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            healthComponent.currentHealth = data.playerHealth;
            // Обновляем UI после загрузки
            if (healthComponent.OnHealthChanged != null)
            {
                healthComponent.OnHealthChanged.Invoke(healthComponent.currentHealth, healthComponent.maxHealth);
            }

            Debug.Log("Игра загружена. Здоровье: " + data.playerHealth);
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден!");
        }
    }

    // Класс для сериализации данных
    [System.Serializable]
    private class SaveData
    {
        public int playerHealth;
    }
}
