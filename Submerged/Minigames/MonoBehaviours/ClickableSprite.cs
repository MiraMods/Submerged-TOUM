using System;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace Submerged.Minigames.MonoBehaviours;

[RegisterInIl2Cpp]
public class ClickableSprite(nint ptr) : MonoBehaviour(ptr)
{
    public Action onDown;
    public Action onDrag;
    public Action onEnter;
    public Action onExit;

    public Action onOver;

    public Action<Collider2D> onTriggerEnter;
    public Action<Collider2D> onTriggerExit;
    public Action<Collider2D> onTriggerStay;
    public Action onUp;
    public Action onUpAsButton;

    protected Controller controller = new Controller();
    protected Camera mainCam;

    private Collider2D _collider2D;
    private bool _isHovered;

    public virtual void Awake()
    {
        if (!mainCam)
            mainCam = Camera.main;
        controller.mainCam = mainCam;
        _collider2D = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        controller.Update();

        if (controller.mainCam != mainCam)
            controller.mainCam = mainCam;

        // Hover Logic
        bool currentlyHovered = controller.CheckHover(_collider2D);
        if (currentlyHovered && !_isHovered)
        {
            _isHovered = true;
            onEnter?.Invoke();
        }
        else if (!currentlyHovered && _isHovered)
        {
            _isHovered = false;
            onExit?.Invoke();
        }

        if (currentlyHovered)
        {
            onOver?.Invoke();
        }

        // Drag Logic
        DragState state = controller.CheckDrag(_collider2D);
        switch (state)
        {
            case DragState.TouchStart:
                onDown?.Invoke();
                break;
            case DragState.Dragging:
                onDrag?.Invoke();
                break;
            case DragState.Released:
                onUp?.Invoke();
                if (currentlyHovered || controller.CheckHover(_collider2D))
                {
                    onUpAsButton?.Invoke();
                }
                break;
        }
    }

    #region Events

    public virtual void OnTriggerEnter2D(Collider2D collider) => onTriggerEnter?.Invoke(collider);

    public virtual void OnTriggerStay2D(Collider2D collider) => onTriggerStay?.Invoke(collider);

    public virtual void OnTriggerExit2D(Collider2D collider) => onTriggerExit?.Invoke(collider);

    #endregion
}
