using JetBrains.Annotations;
using System;
using UnityEngine;

namespace SoFarSoFun.Services
{
    public class InputService : MonoBehaviour
    {
        public event Action<float> OnHorizontalMouseInput;
        public event Action<Vector3> OnLMBClick;

        public bool IsInputEnabled { get; private set; } = true;//temp


        void Update()
        {
            HandleInputs();
        }


        private void HandleInputs()
        {
            if (IsInputEnabled)
            {
                //mouse move
                var xShift = Input.GetAxis("Horizontal");

                if (xShift != 0)
                    OnHorizontalMouseInput?.Invoke(xShift);

                //click
                if (Input.GetMouseButtonDown(0))
                    OnLMBClick?.Invoke(Input.mousePosition);
            }
        }

        public void SetInputActive(bool state)
        {
            IsInputEnabled = state;
        }
    }
}