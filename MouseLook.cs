using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Чувствительность мыши
    public float mouseSensitivity = 100f;

    // Ссылка на тело игрока (чтобы поворачивать его влево-вправо)
    public Transform playerBody;

    // Текущий угол поворота камеры вверх-вниз
    private float xRotation = 0f;

    void Start()
    {
        // Блокируем курсор мыши в центре экрана и прячем его
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Получаем движение мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Вычисляем угол поворота вверх-вниз (инвертируем mouseY)
        xRotation -= mouseY;
        // Ограничиваем поворот камеры, чтобы нельзя было смотреть за спину через верх или низ
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Применяем поворот камеры по оси X (вверх-вниз)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Поворачиваем всё тело игрока по оси Y (влево-вправо)
        // Проверяем, что ссылка на тело установлена
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
