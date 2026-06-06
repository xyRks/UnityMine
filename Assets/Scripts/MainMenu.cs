using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Скрипт для Главного Меню и UI элементов
public class MainMenu : MonoBehaviour
{
    // Аудио микшер для управления звуком
    public AudioMixer audioMixer;

    // UI элемент для отображения здоровья (HP Bar)
    public Slider hpBar;

    // Звуки для действий (добычи/ударов)
    public AudioClip mineSound;
    public AudioClip hitSound;
    public AudioSource audioSource;

    // Метод для продолжения игры
    public void Continue()
    {
        Debug.Log("Продолжение игры...");
        // Здесь можно добавить код для скрытия меню или загрузки сцены
    }

    // Метод для сохранения игры
    public void Save()
    {
        Debug.Log("Сохранение игры...");
        // Здесь будет код сохранения (например, через PlayerPrefs)
    }

    // Метод для открытия настроек
    public void Settings()
    {
        Debug.Log("Открытие настроек...");
        // Здесь код для активации панели настроек
    }

    // Метод для выхода из игры
    public void Exit()
    {
        Debug.Log("Выход из игры");
        // Закрываем приложение на ПК
        Application.Quit();
    }

    // Метод для изменения громкости звука через ползунок (Slider)
    // parameter volume: Значение громкости от ползунка
    public void SetVolume(float volume)
    {
        // Устанавливаем значение параметра громкости в AudioMixer
        // Предполагается, что параметр называется "MasterVolume"
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", volume);
        }
    }

    // Метод для обновления полоски здоровья (HP Bar)
    // parameter currentHp: Текущее значение здоровья
    // parameter maxHp: Максимальное значение здоровья
    public void UpdateHP(float currentHp, float maxHp)
    {
        if (hpBar != null)
        {
            // Рассчитываем процент здоровья и обновляем ползунок
            hpBar.value = currentHp / maxHp;
        }
    }

    // Метод для воспроизведения звука добычи
    public void PlayMineSound()
    {
        if (audioSource != null && mineSound != null)
        {
            audioSource.PlayOneShot(mineSound);
        }
    }

    // Метод для воспроизведения звука удара
    public void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}
