using Reactor.Utilities.Attributes;
using UnityEngine;

namespace Submerged.Minigames.MonoBehaviours;

[RegisterInIl2Cpp]
public class Draggable(nint ptr) : ClickableSprite(ptr)
{
    public bool forceStop;

    public bool dragging;
    public Vector3 initialPosition;

    private Vector2 _offset;

    public override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        onDown += OnDownHandler;
        onDrag += OnDragHandler;
        onUp += OnUpHandler;
    }

    private void OnDestroy()
    {
        onDown -= OnDownHandler;
        onDrag -= OnDragHandler;
        onUp -= OnUpHandler;
    }

    private void OnDownHandler()
    {
        _offset = (Vector2) transform.position - controller.DragStartPosition;
        dragging = true;
    }

    private void OnDragHandler()
    {
        if (forceStop) return;
        if (!dragging) return;

        Vector2 newPos = controller.DragPosition + _offset;
        transform.position = new Vector3(newPos.x, newPos.y, initialPosition.z);
    }

    private void OnUpHandler()
    {
        dragging = false;
    }
}
