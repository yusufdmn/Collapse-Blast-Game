using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager
    {
        public delegate void ClickDelegate(Vector3 worldPosition);
        public event ClickDelegate Clicked;
        
        private BoardInputActions _boardInputActions;
        
        public InputManager()
        {
            _boardInputActions = new BoardInputActions();
            _boardInputActions.Enable();
            _boardInputActions.BoardActionMap.Click.performed += ctx => OnClick();
        }
        
        private void OnClick()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
            Clicked?.Invoke(worldPosition);
        }
        
        ~InputManager()
        {
            _boardInputActions.Disable();
        }
    }
}