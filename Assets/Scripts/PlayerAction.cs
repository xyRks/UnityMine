using UnityEngine;

public enum ToolType
{
    None,
    Pickaxe,
    Axe,
    Sword,
    Bow
}

public class PlayerAction : MonoBehaviour
{
    [Tooltip("Камера игрока для пускания лучей (Raycast).")]
    public Camera playerCamera;

    [Tooltip("Текущий инструмент в руках.")]
    public ToolType currentTool = ToolType.None;

    [Tooltip("Дальность удара или выстрела.")]
    public float actionRange = 10f;

    [Tooltip("Урон от инструмента.")]
    public int toolDamage = 10;

    void Update()
    {
        // При нажатии левой кнопки мыши (0 - ЛКМ)
        if (Input.GetMouseButtonDown(0))
        {
            PerformAction();
        }
    }

    private void PerformAction()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Камера не назначена в PlayerAction!");
            return;
        }

        if (currentTool == ToolType.None)
        {
            return; // Нет инструмента - ничего не делаем
        }

        // Пускаем луч из центра экрана
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        // Если луч во что-то попал
        if (Physics.Raycast(ray, out hit, actionRange))
        {
            // Проверяем, какой у нас инструмент
            if (currentTool == ToolType.Pickaxe || currentTool == ToolType.Axe)
            {
                // Ищем компонент Resource на объекте, в который попали
                Resource resource = hit.collider.GetComponent<Resource>();
                if (resource != null)
                {
                    resource.TakeDamage(toolDamage);
                }
            }
            else if (currentTool == ToolType.Sword || currentTool == ToolType.Bow)
            {
                // Ищем компонент Enemy на объекте, в который попали
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(toolDamage);
                }
            }
        }
    }
}
