using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CommonsHelper;
using CommonsPattern;

public class CursorCanvas : SingletonManager<CursorCanvas>
{
    /* External references */

    [Tooltip("Mouse image to use when not hovering mouse over interactable object")]
    public Image defaultCursorImage;
    
    [Tooltip("Mouse image to use when hovering mouse over interactable object")]
    public Image hoverCursorImage;
    
    
    /* Sibling components */
    
    private Canvas pointerCanvas;


    protected override void Init()
    {
        Debug.Assert(defaultCursorImage != null, "No defaultCursorImage defined", this);
        Debug.Assert(hoverCursorImage != null, "No hoverCursorImage defined", this);

        pointerCanvas = this.GetComponentOrFail<Canvas>();
    }

    public void SetCanvasEnable(bool canvasEnabled)
    {
        pointerCanvas.enabled = canvasEnabled;
    }
    
    public void SetCursorImage(bool isHovering)
    {
        // set Locked cursor image for default or hover
        defaultCursorImage.enabled = !isHovering;
        hoverCursorImage.enabled = isHovering;
    }
}
