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
            eventData.pointerDrag.SendMessage("SetNewParent", transform, SendMessageOptions.DontRequireReceiver);
        }
    }
}
