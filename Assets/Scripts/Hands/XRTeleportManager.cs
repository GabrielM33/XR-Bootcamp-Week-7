using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
    public class XRTeleportManager : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractor _teleportController;
        [SerializeField] private XRBaseInteractor _mainController;
        
        [SerializeField] private Animator _handAnimator;
        [SerializeField] private GameObject _pointer;

        private void Start()
        {
            _teleportController.selectEntered.AddListener(MoveSelectionToMainController);
        }

        private void MoveSelectionToMainController(SelectEnterEventArgs arg0)
        {
            var interactable = arg0.interactable;
            if (interactable is BaseTeleportationInteractable) return;
            
            if(interactable) _teleportController.interactionManager.ForceSelect(_mainController, interactable);
        }

        private void Update()
        {
            _pointer.SetActive(_handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Point") && _handAnimator.gameObject.activeSelf);
        }
    }
}