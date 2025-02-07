using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager
    {
        public delegate void ClickDelegate(Vector3 worldPosition);
        public event ClickDelegate Clicked;
        
        private readonly BoardInputActions _boardInputActions;
        private readonly Camera _mainCamera;
        private bool _canClick = true;
        
        public InputManager()
        {
            _mainCamera = Camera.main;
            _boardInputActions = new BoardInputActions();
            _boardInputActions.Enable();
            _boardInputActions.BoardActionMap.Click.performed += ctx => OnClick();
        }
        
        public void CanClick(bool canClick)
        {
            _canClick = canClick;
        }
        
        private void OnClick()
        {
            if (!_canClick) return;
            
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
            Clicked?.Invoke(worldPosition);
        }
        
        ~InputManager()
        {
            _boardInputActions.Disable();
        }
    }
}