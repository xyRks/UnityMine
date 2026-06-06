using UnityEngine;
using UnityEngine.EventSystems;

// Компонент для слота инвентаря (или хотбара), который принимает предметы
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Проверяем, есть ли в этом слоте уже предмет (дети)
        if (transform.childCount == 0)
        {
            // Пытаемся получить компонент перетаскиваемого предмета
            DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

            // Если мы действительно перетащили предмет
            if (draggableItem != null)
            {
                // Устанавливаем этому предмету нового родителя - этот слот
                draggableItem.parentAfterDrag = transform;
            }
        }
    }
}
