using System.Collections.Generic;
using System;
using UnityEngine;

namespace DadVSMe.Inputs
{
    public static class InputManager
    {
        private static readonly Dictionary<Type, InputReaderBase> inputReaders = null;
        private static InputActions inputActions = null;
        private static Type currentInputReaderType = null;

        static InputManager()
        {
            inputActions = new InputActions();
            inputReaders = new Dictionary<Type, InputReaderBase>();
            currentInputReaderType = null;
        }

        public static void Initialize()
        {
            currentInputReaderType = null;
        }

        public static void Release()
        {
            foreach (var inputReader in inputReaders)
                inputReader.Value.Release();

            inputReaders.Clear();
            inputActions.Dispose();
            inputActions = null;
        }

        public static TInputReader GetInput<TInputReader>() where TInputReader : InputReaderBase
        {
            Type inputReaderType = typeof(TInputReader);
            if (inputReaders.TryGetValue(inputReaderType, out InputReaderBase inputReader) == false)
            {
                inputReader = ScriptableObject.CreateInstance<TInputReader>();
                inputReader.Initialize(inputActions);
                inputReaders.Add(inputReaderType, inputReader);
            }

            if(inputReaderType != currentInputReaderType)
            {
                if(currentInputReaderType != null)
                    inputReaders[currentInputReaderType].GetInputActionMap().Disable();
                
                currentInputReaderType = inputReaderType;
                inputReader.GetInputActionMap().Enable();
            }

            return inputReader as TInputReader;
        }

        // Legacy Input System
        public static bool GetKeyDown(KeyCode keyCode) => UnityEngine.Input.GetKeyDown(keyCode);
        public static bool GetKeyUp(KeyCode keyCode) => UnityEngine.Input.GetKeyUp(keyCode);
        public static bool GetKey(KeyCode keyCode) => UnityEngine.Input.GetKey(keyCode);
        public static bool GetButtonDown(string buttonName) => UnityEngine.Input.GetButtonDown(buttonName);
        public static bool GetButtonUp(string buttonName) => UnityEngine.Input.GetButtonUp(buttonName);
        public static bool GetButton(string buttonName) => UnityEngine.Input.GetButton(buttonName);

    }
}