using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeechCodingHandler
{
    public class TypeHelper
    {
        public static List<ObjectInfo> GetObjetInfos(IEnumerable<Type> types, string assemblyName)
        {
            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            var namespaces = types.Select(item => item.Namespace).Distinct();

            foreach (var ns in namespaces)
            {
                if (ns != null)
                {
                    objectInfos.Add(new ObjectInfo() { Name = ns, Type = ObjectType.Namespace, Assembly = assemblyName });
                }
            }

            foreach (Type type in types)
            {
                if (!type.IsPublic)
                {
                    continue;
                }

                ObjectInfo namespaceObj = objectInfos.FirstOrDefault(item => item.Type == ObjectType.Namespace && item.Name == type.Namespace);

                ObjectType objType = ObjectType.Unknown;
                if (type.IsClass)
                {
                    if (type.BaseType.Name == nameof(MulticastDelegate))
                    {
                        objType = ObjectType.Delegate;
                    }
                    else
                    {
                        objType = ObjectType.Class;
                    }
                }
                else if (type.IsInterface)
                {
                    objType = ObjectType.Interface;
                }
                else if (type.IsEnum)
                {
                    objType = ObjectType.Enum;
                }
                else if (type.BaseType.Name == nameof(ValueType))
                {
                    objType = ObjectType.Struct;
                }

                var objectInfo = new ObjectInfo() { Name = type.Name, Type = objType, Parent = namespaceObj, Assembly = assemblyName };

                if (namespaceObj != null)
                {
                    namespaceObj.Children.Add(objectInfo);
                }

                objectInfos.Add(objectInfo);

                try
                {
                    foreach (var property in type.GetProperties())
                    {
                        var propertyObj = new ObjectInfo() { Name = property.Name, Type = ObjectType.Property, Parent = objectInfo };
                        objectInfos.Add(propertyObj);
                        objectInfo.Children.Add(propertyObj);
                    }

                    foreach (var field in type.GetFields())
                    {
                        if (!field.IsSpecialName)
                        {
                            var fieldObj = new ObjectInfo() { Name = field.Name, Type = ObjectType.Field, Parent = objectInfo };
                            objectInfos.Add(fieldObj);
                            objectInfo.Children.Add(fieldObj);
                        }
                    }

                    foreach (var method in type.GetMethods())
                    {
                        if (!method.IsSpecialName)
                        {
                            var methodObj = new ObjectInfo() { Name = method.Name, Type = ObjectType.Method, Parent = objectInfo };
                            objectInfos.Add(methodObj);
                            objectInfo.Children.Add(methodObj);
                        }
                    }

                    foreach (var evt in type.GetEvents())
                    {
                        var eventObj = new ObjectInfo() { Name = evt.Name, Type = ObjectType.Event, Parent = objectInfo };
                        objectInfos.Add(eventObj);
                        objectInfo.Children.Add(eventObj);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }

            return objectInfos;
        }
    }
}
