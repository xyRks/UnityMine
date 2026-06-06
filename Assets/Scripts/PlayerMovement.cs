using UnityEngine;

/// <summary>
/// Скрипт перемещения игрока (PC Standalone).
/// Использует CharacterController и стандартный ввод Input.GetAxis.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Скорость ходьбы")]
    public float walkSpeed = 5f;

    [Tooltip("Скорость бега")]
    public float runSpeed = 8f;

    [Tooltip("Сила прыжка")]
    public float jumpHeight = 2f;

    [Tooltip("Гравитация")]
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("PlayerMovement: Не найден CharacterController!");
        }
    }

    void Update()
    {
        if (controller == null) return;

        // Проверка касания земли
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Небольшое значение, чтобы уверенно стоять на земле
        }

        // Получение ввода
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Направление движения относительно поворота игрока
        Vector3 move = transform.right * x + transform.forward * z;

        // Определение текущей скорости (бег при зажатом Shift)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Применение движения
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Применение гравитации
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
