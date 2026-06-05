using UnityEngine;
using UnityEngine.SceneManagement;

// Этот скрипт вешается на объект-портал или дверь (с коллайдером в режиме IsTrigger).
// При входе игрока в этот триггер загружается указанная сцена.
public class ScenePortal : MonoBehaviour
{
    // Имя сцены, которую нужно загрузить
    public string sceneToLoad;

    // Метод вызывается, когда другой объект входит в триггер
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в портал зашел именно игрок.
        // Для этого у объекта игрока должен быть тег "Player".
        if (other.CompareTag("Player"))
        {
            // Если имя сцены не пустое, загружаем её
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Имя сцены для загрузки не указано!");
            }
        }
    }
}
