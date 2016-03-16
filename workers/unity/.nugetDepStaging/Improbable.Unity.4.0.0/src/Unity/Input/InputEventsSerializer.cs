using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Improbable.Unity.Input.Storage;

namespace Improbable.Unity.Input
{
    public static class InputEventsSerializer
    {
        public static void WriteRecordingToFile(string fileName, List<InputEvent> capturedInput)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                using (var streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    SerializeInputEventsToStream(streamWriter, new List<InputEvent>(capturedInput));
                }
            }
        }

        public static void SerializeInputEventsToStream(TextWriter textWriter, List<InputEvent> inputEvents)
        {
            var serializableQueue = inputEvents;
            var xmlSerializer = new XmlSerializer(typeof(List<InputEvent>));

            xmlSerializer.Serialize(textWriter, serializableQueue);
        }

        public static List<InputEvent> LoadInputEventsFromStream(Stream fileStream)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<InputEvent>));
            return (List<InputEvent>) xmlSerializer.Deserialize(fileStream);
        }
    }
}