using SimpleDiscord.Events.Attribute;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleDiscord.Events
{
    public class Handler
    {
        public static Dictionary<string, HashSet<KeyValuePair<object, MethodInfo>>> List { get; } = [];

        public static void RegisterEvents(Assembly assembly, object caller = null)
        {
            foreach (Type type in assembly.GetTypes())
                RegisterEvents(caller: caller, type: type);
        }

        public static void RegisterEvents(object caller = null, Type type = null)
        {
            if (type is null && caller is null)
                throw new ArgumentException("Invalid arguments!\nAt least 'caller' or 'type' is required!");

            type ??= caller.GetType();

            foreach (MethodInfo method in type.GetMethods())
            {
                object[] attribs = method.GetCustomAttributes(typeof(SocketEvent), false);
                if (attribs != null && attribs.Length > 0)
                    foreach (object rawAttribute in attribs)
                    {
                        SocketEvent attribute = rawAttribute as SocketEvent;
                        if (List.ContainsKey(attribute.Event))
                            List[attribute.Event].Add(new(caller, method));
                        else
                            List.Add(attribute.Event, [new(caller, method)]);
                    }
            }
        }

        internal static void Invoke(string name, object args)
        {
            if (List.TryGetValue(name, out HashSet<KeyValuePair<object, MethodInfo>> list))
                foreach (KeyValuePair<object, MethodInfo> method in list)
                    if (method.Value.GetGenericArguments().Length > 0)
                        method.Value.Invoke(method.Key, [args]);
                    else
                        method.Value.Invoke(method.Key, []);
        }
    }
}
