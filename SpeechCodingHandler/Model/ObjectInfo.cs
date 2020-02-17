using System.Collections.Generic;

namespace SpeechCodingHandler
{
    public class ObjectInfo
    {
        public string Assembly { get; set; }
        public ObjectType Type { get; set; }
        public string Name { get; set; }
        public List<ObjectInfo> Children { get; set; } = new List<ObjectInfo>();
        public ObjectInfo Parent { get; set; }
    }
}
