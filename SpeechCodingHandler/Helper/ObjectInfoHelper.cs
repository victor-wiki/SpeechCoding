using System.Collections.Generic;

namespace SpeechCodingHandler
{
    public class ObjectInfoHelper
    {
        public static List<ObjectInfo> GetChildlren(ObjectInfo objectInfo, bool includeSelf = true)
        {
            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            if (includeSelf)
            {
                objectInfos.Add(objectInfo);
            }

            if (objectInfo.Children != null)
            {
                foreach (ObjectInfo child in objectInfo.Children)
                {
                    objectInfos.AddRange(GetChildlren(child));
                }
            }

            return objectInfos;
        }
    }
}
