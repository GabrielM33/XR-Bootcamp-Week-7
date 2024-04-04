using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTagLimitedSocketInteractor : XRSocketInteractor
{
    public string interactableTag;
    
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag(interactableTag);
    }
    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.CompareTag(interactableTag);
    }
}
