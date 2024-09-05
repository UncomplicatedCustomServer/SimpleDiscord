﻿using SimpleDiscord.Events.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleDiscord.Events
{
    public class Handler
    {
        public Dictionary<string, HashSet<KeyValuePair<object, MethodInfo>>> List { get; } = [];

        public void RegisterEvents(Assembly assembly, object caller = null)
        {
            foreach (Type type in assembly.GetTypes())
                RegisterEvents(caller: caller, type: type);
        }

        public void RegisterEvents(object caller = null, Type type = null)
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

        internal void Invoke(string name, object args)
        {
            if (List.TryGetValue(name, out HashSet<KeyValuePair<object, MethodInfo>> list))
                foreach (KeyValuePair<object, MethodInfo> method in list)
                    if (method.Value.GetParameters().Length > 0)
                    {
                        if (method.Value.GetParameters()[0].ParameterType == args.GetType())
                            method.Value.Invoke(method.Key, [args]);
                    }
                    else
                        method.Value.Invoke(method.Key, []);
        }
    }
}
