using System.Collections.Generic;
using System.Diagnostics;
using Improbable.Unity.Input.Storage;
using UnityEngine;
using log4net;

namespace Improbable.Unity.Input.Sources
{
    public class PlaybackInputSource : IInputSource
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(PlaybackInputSource));
        public IEnumerable<InputEvent> CapturedInput { private get; set; }
        private bool playing;

        private readonly RecordedInputSource recordedInputSource;
        private readonly InputSourceManager inputSourceManager;
        private readonly Stopwatch stopwatch;

        public PlaybackInputSource(InputSourceManager inputSourceManager, IEnumerable<InputEvent> capturedInput)
        {
            CapturedInput = capturedInput;
            this.inputSourceManager = inputSourceManager;
            stopwatch = new Stopwatch();
            recordedInputSource = new RecordedInputSource(CapturedInput);
        }

        public void StartPlayback()
        {
            LOGGER.Info("Starting recorded input playback");
            if (!playing)
            {
                inputSourceManager.CurrentInputSource = this;
                stopwatch.Start();
                playing = true;
            }
        }

        public void StopPlayback()
        {
            LOGGER.Info("Starting recorded input playback");
            if (playing)
            {
                inputSourceManager.CurrentInputSource = new UnityInputSource();
                stopwatch.Stop();
                stopwatch.Reset();
                playing = false;
            }
        }

        public void UpdateRecordedInputSource()
        {
            recordedInputSource.Update(stopwatch.ElapsedMilliseconds);
        }

        public float GetAxisRaw(string axisName)
        {
            return recordedInputSource.GetAxisRaw(axisName);
        }

        public float GetAxis(string axisName)
        {
            return recordedInputSource.GetAxis(axisName);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return recordedInputSource.GetKey(keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return recordedInputSource.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return recordedInputSource.GetKeyUp(keyCode);
        }

        public bool GetMouseButton(int buttonIdentifier)
        {
            return recordedInputSource.GetMouseButton(buttonIdentifier);
        }

        public bool GetMouseButtonUp(int buttonIdentifier)
        {
            return recordedInputSource.GetMouseButtonUp(buttonIdentifier);
        }

        public bool GetMouseButtonDown(int buttonIdentifier)
        {
            return recordedInputSource.GetMouseButtonDown(buttonIdentifier);
        }

        public bool GetButton(string buttonName)
        {
            return recordedInputSource.GetButton(buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return recordedInputSource.GetButtonDown(buttonName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return recordedInputSource.GetButtonUp(buttonName);
        }
    }
}