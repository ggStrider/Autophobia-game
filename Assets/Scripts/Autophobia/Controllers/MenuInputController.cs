using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using System;
using System.Collections.Generic;

using Autophobia.UIs;
using Autophobia.InvokeEvents;
using UnityEngine.Serialization;

namespace Autophobia.Controllers
{
    public class MenuInputController : MonoBehaviour
    {
        [SerializeField] private RandomSound _randomKeyboardButtonSound;

        [SerializeField] private List<Category> _categories;

        [SerializeField] private MenuCategories _currentCategory;
        
        [SerializeField] private float _changeSliderValueDelta = 0.1f;
        
        private int _currentButtonIndex;
        
        private enum MenuCategories
        {
            Main,
            Settings
        };
        
        private PlayerMap _playerMap;
        private Selectable _previousSetupObject;
        
        private readonly Color _selectedColor = new Color(1, 1, 1, 0.01f);
        private readonly Color _unselectedColor = new Color(1, 1, 1, 0);


#if UNITY_EDITOR
        private void Awake()
        {
            //_EnableMap();
        }
#endif
        private void OnDestroy()
        {
            _playerMap.UI.ArrowUp.started -= OnArrowUp;
            _playerMap.UI.ArrowDown.started -= OnArrowDown;

            _playerMap.UI.Enter.started -= OnSelect;
        }
        
        public void _EnableMap()
        {
            _playerMap = new PlayerMap();

            _playerMap.UI.ArrowUp.started += OnArrowUp;
            _playerMap.UI.ArrowDown.started += OnArrowDown;

            _playerMap.UI.Enter.started += OnSelect;
            
            _playerMap.Enable();
        }

        private void ToggleSideArrows(bool enable)
        {
            if (enable)
            {
                _playerMap.UI.ArrowRight.started += OnArrowRight;
                _playerMap.UI.ArrowLeft.started += OnArrowLeft;
            }
            else
            {
                _playerMap.UI.ArrowRight.started -= OnArrowRight;
                _playerMap.UI.ArrowLeft.started -= OnArrowLeft;
            }
        }

        private void OnArrowLeft(InputAction.CallbackContext obj)
        {
            var slider = GetSelectableObject().GetComponent<Slider>();
            slider.value -= _changeSliderValueDelta;

            if(slider.value <= slider.minValue) return;
            _randomKeyboardButtonSound._Play();
        }

        private void OnArrowRight(InputAction.CallbackContext obj)
        {
            var slider = GetSelectableObject().GetComponent<Slider>();
            slider.value += _changeSliderValueDelta;
            
            if(slider.value >= slider.maxValue) return;
            _randomKeyboardButtonSound._Play();
        }

        public void _DisableMap()
        {
            _playerMap.Disable();
        }

        public void _ChangeMenuCategory(int type)
        {
            _currentCategory = (MenuCategories)type;

            _previousSetupObject = GetSelectableObject();
            _currentButtonIndex = 0;
            
            ReColorUI();
            CheckIsSlider();
        }

        private void OnSelect(InputAction.CallbackContext obj)
        {
            _randomKeyboardButtonSound._Play();

            var objectEvent = _categories[(int)_currentCategory].ObjectsToSetup[_currentButtonIndex]
                .GetComponent<InvokeUnityEvent>();
            
            if(objectEvent == null) return;
            objectEvent.InvokeEvent();
        }

        private void OnArrowDown(InputAction.CallbackContext obj)
        {
            _previousSetupObject = GetSelectableObject();
            
            var currentMenu = _categories[(int)_currentCategory];
            _currentButtonIndex = (int)Mathf.Repeat(_currentButtonIndex + 1,
                currentMenu.ObjectsToSetup.Count);

            CheckIsSlider();
            ReColorUI();
            
            _randomKeyboardButtonSound._Play();
        }

        private void CheckIsSlider()
        {
            var currentSelectable = GetSelectableObject();

            if (currentSelectable is not Slider)
            {
                ToggleSideArrows(false);
                return;
            }
            
            ToggleSideArrows(true);
        }

        private void OnArrowUp(InputAction.CallbackContext obj)
        {
            _previousSetupObject = GetSelectableObject();
            
            var currentMenu = _categories[(int)_currentCategory];
            _currentButtonIndex = (int)Mathf.Repeat(_currentButtonIndex - 1,
                currentMenu.ObjectsToSetup.Count);

            CheckIsSlider();
            ReColorUI();
            
            _randomKeyboardButtonSound._Play();
        }

        private void ReColorUI()
        {
            var previousColorBlock = _previousSetupObject.colors;
            previousColorBlock.normalColor = _unselectedColor;

            _previousSetupObject.colors = previousColorBlock;
            
            var selectable = GetSelectableObject();
            if (selectable == null) return;
            
            var colorBlock = selectable.colors;
            colorBlock.normalColor = _selectedColor;

            selectable.colors = colorBlock;
        }

        private Selectable GetSelectableObject()
        {
            return _categories[(int)_currentCategory].ObjectsToSetup[_currentButtonIndex].GetComponent<Selectable>();
        }

        [Serializable]
        public class Category
        {
            [Tooltip("Buttons, sliders (objects which HAS component of BUTTON or SLIDER)")]
            [field: SerializeField] public List<GameObject> ObjectsToSetup { get; private set; }
        }
    }
}

/* 999 */