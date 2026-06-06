using UnityEngine;

// Скрипт для сундука с хранилищем предметов
public class Chest : MonoBehaviour
{
    // Ссылка на UI панель сундука (устанавливается в инспекторе)
    public GameObject chestUI;

    // Флаг, находится ли игрок рядом с сундуком
    private bool isPlayerNear = false;

    // Метод Update вызывается каждый кадр
    private void Update()
    {
        // Проверяем, если игрок рядом и нажал клавишу E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            // Если UI сундука назначен, переключаем его видимость
            if (chestUI != null)
            {
                bool isActive = chestUI.activeSelf;
                chestUI.SetActive(!isActive);
            }
            else
            {
                Debug.LogWarning("UI сундука не назначено в инспекторе!");
            }
        }
    }

    // Метод вызывается, когда другой коллайдер входит в триггер
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошел именно игрок (по тегу)
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    // Метод вызывается, когда другой коллайдер выходит из триггера
    private void OnTriggerExit(Collider other)
    {
        // Проверяем, что вышел именно игрок
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            // Автоматически закрываем UI сундука, если игрок ушел
            if (chestUI != null && chestUI.activeSelf)
            {
                chestUI.SetActive(false);
            }
        }
    }
}
