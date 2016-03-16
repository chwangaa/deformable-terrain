using System;
using System.Collections.Generic;
using System.Diagnostics;
using Improbable.Unity.Input.Sources;
using Improbable.Unity.Input.Storage;
using UnityEngine;
using Debug = UnityEngine.Debug;
using log4net;

namespace Improbable.Unity.Input.Recording
{
    public class InputRecorder
    {
    	private static readonly ILog LOGGER = LogManager.GetLogger(typeof(InputRecorder));
        public List<InputEvent> CapturedInput { get; private set; }

        private bool recording;
        private readonly IInputSource unityInputSource;
        private readonly Stopwatch stopwatch;
        private const float TOLLERANCE = 0.001f;

        private readonly List<string> axisToRecord = new List<string> { "Vertical", "Horizontal", "Mouse X", "Mouse Y" };
        private readonly List<string> buttonsToRecord = new List<string> { "Fire1" };
        private readonly List<int> buttonIdentifiers = new List<int> { 0, 1, 2 };

        public InputRecorder(IInputSource inputSource)
        {
            unityInputSource = inputSource;
            CapturedInput = new List<InputEvent>();
            stopwatch = new Stopwatch();
        }

        public void StartRecording()
        {
            if (!recording)
            {
                stopwatch.Start();
                recording = true;
            }
        }

        public void StopRecording()
        {
            if (recording)
            {
                stopwatch.Stop();
                stopwatch.Reset();
                recording = false;
            }
        }

        public void TakeInputSample()
        {
            if (recording)
            {
                var thisFramesTime = stopwatch.ElapsedMilliseconds;
                RecordAxisInputs(thisFramesTime);
                RecordKeyInputs(thisFramesTime);
                RecordMouseButtonInputs(thisFramesTime);
                RecordButtonInputs(thisFramesTime);    
            }
            
        }

        private void RecordAxisInputs(long thisFramesTime)
        {
            foreach (var axisName in axisToRecord)
            {
                try
                {
                    var inputValue = unityInputSource.GetAxis(axisName);
                    if (System.Math.Abs(inputValue) > TOLLERANCE)
                    {
                        CapturedInput.Add(InputEventFactory.CreateAxisInputEvent(InputType.Axis,
                                                                                 axisName,
                                                                                 inputValue,
                                                                                 thisFramesTime));
                    }

                    var rawInputValue = unityInputSource.GetAxisRaw(axisName);
                    if (System.Math.Abs(rawInputValue) > TOLLERANCE)
                    {
                        CapturedInput.Add(InputEventFactory.CreateAxisInputEvent(InputType.AxisRaw,
                                                                                 axisName,
                                                                                 rawInputValue,
                                                                                 thisFramesTime));
                    }
                }
                catch (UnityException)
                {
                    LOGGER.Error("Axis: " + axisName + " does not exist in unity input and cannot be recorded.");
                }
            }
        }

        private void RecordKeyInputs(long thisFramesTime)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                try
                {
                    if (unityInputSource.GetKey(keyCode))
                    {
                        CapturedInput.Add(InputEventFactory.CreateKeyInputEvent(InputType.Key,
                                                                                keyCode,
                                                                                thisFramesTime));
                    }

                    if (unityInputSource.GetKeyDown(keyCode))
                    {
                        CapturedInput.Add(InputEventFactory.CreateKeyInputEvent(InputType.KeyDown,
                                                                                keyCode,
                                                                                thisFramesTime));
                    }

                    if (unityInputSource.GetKeyUp(keyCode))
                    {
                        CapturedInput.Add(InputEventFactory.CreateKeyInputEvent(InputType.KeyUp,
                                                                                keyCode,
                                                                                thisFramesTime));
                    }
                }
                catch (UnityException)
                {
                    LOGGER.Error("Key: " + keyCode + " does not exist in unity input and cannot be recorded.");
                }
            }
        }

        private void RecordMouseButtonInputs(long thisFramesTime)
        {
            foreach (int buttonIdentifier in buttonIdentifiers)
            {
                if (unityInputSource.GetMouseButton(buttonIdentifier))
                {
                    CapturedInput.Add(InputEventFactory.CreateMouseButtonInputEvent(InputType.MouseButton,
                                                                                    buttonIdentifier,
                                                                                    thisFramesTime));
                }

                if (unityInputSource.GetMouseButtonDown(buttonIdentifier))
                {
                    CapturedInput.Add(InputEventFactory.CreateMouseButtonInputEvent(InputType.MouseButtonDown,
                                                                                    buttonIdentifier,
                                                                                    thisFramesTime));
                }

                if (unityInputSource.GetMouseButtonUp(buttonIdentifier))
                {
                    CapturedInput.Add(InputEventFactory.CreateMouseButtonInputEvent(InputType.MouseButtonUp,
                                                                                    buttonIdentifier,
                                                                                    thisFramesTime));
                }
            }
        }

        private void RecordButtonInputs(long thisFramesTime)
        {
            foreach (var buttonName in buttonsToRecord)
            {
                try
                {
                    if (unityInputSource.GetButton(buttonName))
                    {
                        CapturedInput.Add(InputEventFactory.CreateButtonInputEvent(InputType.Button,
                                                                                   buttonName,
                                                                                   thisFramesTime));
                    }


                    if (unityInputSource.GetButtonDown(buttonName))
                    {
                        CapturedInput.Add(InputEventFactory.CreateButtonInputEvent(InputType.ButtonDown,
                                                                                   buttonName,
                                                                                   thisFramesTime));
                    }

                    if (unityInputSource.GetButtonUp(buttonName))
                    {
                        CapturedInput.Add(InputEventFactory.CreateButtonInputEvent(InputType.ButtonUp,
                                                                                   buttonName,
                                                                                   thisFramesTime));
                    }
                }
                catch (UnityException)
                {
                    LOGGER.Error("Axis: " + buttonName + " does not exist in unity input and cannot be recorded.");
                }
            }
        }
    }
}