using UnityEngine;

/// <summary>
/// Скрипт обзора мышью для камеры от первого лица.
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Tooltip("Чувствительность мыши")]
    public float mouseSensitivity = 100f;

    [Tooltip("Ссылка на тело игрока для поворота по горизонтали")]
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // При старте скрываем курсор и блокируем его в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerBody == null)
        {
            Debug.LogError("MouseLook: Не назначена ссылка на playerBody!");
        }
    }

    void Update()
    {
        if (playerBody == null) return;

        // Получение ввода мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Вращение камеры по вертикали (ограничение от -90 до 90 градусов)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Поворот тела игрока по горизонтали
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
