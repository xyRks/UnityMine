using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Компонент для перетаскивания иконки предмета в инвентаре
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки UI")]
    public Image image; // Картинка предмета

    [HideInInspector]
    public Transform parentAfterDrag; // Родитель, куда вернется предмет после перетаскивания

    public void SetNewParent(Transform newParent)
    {
        parentAfterDrag = newParent;
    }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // Если CanvasGroup нет, добавляем его (нужен для пропуска лучей при перетаскивании)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Запоминаем текущего родителя
        parentAfterDrag = transform.parent;

        // Перемещаем предмет на самый верх, чтобы он рисовался поверх остальных элементов
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Отключаем блокировку лучей, чтобы предмет не мешал под собой искать слоты для дропа
        canvasGroup.blocksRaycasts = false;

        // Делаем немного прозрачным при перетаскивании (опционально)
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Перемещаем позицию предмета за курсором мыши
        rectTransform.anchoredPosition += eventData.delta / GetCanvasScale();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Возвращаем предмет в слот (новый или старый)
        transform.SetParent(parentAfterDrag);

        // Включаем блокировку лучей обратно
        canvasGroup.blocksRaycasts = true;

        // Возвращаем непрозрачность
        canvasGroup.alpha = 1f;
    }

    // Вспомогательный метод для получения масштаба канваса
    private float GetCanvasScale()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            return canvas.scaleFactor;
        }
        return 1f;
    }
}
