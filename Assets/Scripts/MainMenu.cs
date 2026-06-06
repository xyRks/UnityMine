using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управление Главным Меню (PC Standalone).
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Tooltip("Имя сцены игры для загрузки")]
    public string gameSceneName = "GameScene";

    /// <summary>
    /// Метод для кнопки Play
    /// </summary>
    public void PlayGame()
    {
        Debug.Log("Загрузка игры...");
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Метод для кнопки Quit. Выходит из приложения на ПК.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");

        // Закрываем приложение
        Application.Quit();

        // Если мы находимся в редакторе Unity, останавливаем режим Play
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
