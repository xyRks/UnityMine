using UnityEngine;

// Компонент должен быть прикреплен к объекту с CharacterController
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Скорость ходьбы
    public float speed = 5f;

    // Сила прыжка
    public float jumpHeight = 2f;

    // Гравитация
    public float gravity = -9.81f;

    // Ссылка на компонент CharacterController
    private CharacterController controller;

    // Текущая скорость падения (для гравитации)
    private Vector3 velocity;

    // Проверка, стоит ли игрок на земле
    private bool isGrounded;

    void Start()
    {
        // Получаем компонент CharacterController при старте
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Проверяем, касается ли контроллер земли
        isGrounded = controller.isGrounded;

        // Если мы на земле и падаем, сбрасываем скорость падения
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Получаем ввод с клавиатуры (W, A, S, D или стрелочки)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Вычисляем направление движения
        Vector3 move = transform.right * x + transform.forward * z;

        // Двигаем игрока
        controller.Move(move * speed * Time.deltaTime);

        // Прыжок: если игрок на земле и нажал кнопку прыжка (пробел)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Формула для вычисления скорости прыжка
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Применяем гравитацию (увеличиваем скорость падения со временем)
        velocity.y += gravity * Time.deltaTime;

        // Двигаем игрока вниз (гравитация)
        controller.Move(velocity * Time.deltaTime);
    }
}
