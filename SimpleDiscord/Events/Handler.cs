using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using SimpleDiscord.Events.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleDiscord.Events
{
    public class Handler(Client client)
    {
        public Dictionary<string, HashSet<KeyValuePair<object, MethodInfo>>> List { get; } = [];

        public Dictionary<KeyValuePair<string, string>, HashSet<KeyValuePair<object, MethodInfo>>> CommandHandlers { get; } = [];

        public Dictionary<string, HashSet<KeyValuePair<object, MethodInfo>>> ComponentHandlers { get; } = [];

        private readonly Client discordClient = client;

        public void RegisterEvents(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
                RegisterEvents(null, type);
        }

        public void RegisterEvents(object caller = null, Type type = null)
        {
            if (type is null && caller is null)
                throw new ArgumentException("Invalid arguments!\nAt least 'caller' or 'type' is required!");

            type ??= caller.GetType();

            foreach (MethodInfo method in type.GetMethods())
            {
                object[] attribs = method.GetCustomAttributes(typeof(SocketEvent), false);
                if (attribs is not null && attribs.Length > 0)
                    foreach (object rawAttribute in attribs)
                    {
                        SocketEvent attribute = rawAttribute as SocketEvent;
                        discordClient.Logger.Info($"Adding new listener:\n{JsonConvert.SerializeObject(attribute)}");
                        if (List.ContainsKey(attribute.Event))
                            List[attribute.Event].Add(new(caller, method));
                        else
                            List.Add(attribute.Event, [new(caller, method)]);
                    }

                object[] attribs2 = method.GetCustomAttributes(typeof(CommandHandler), false);
                if (attribs2 is not null && attribs2.Length > 0)
                    foreach (object rawAttribute2 in attribs2)
                    {
                        CommandHandler attribute = rawAttribute2 as CommandHandler;
                        if (CommandHandlers.ContainsKey(new(attribute.CommandName, attribute.SubCommandName)))
                            CommandHandlers[new(attribute.CommandName, attribute.SubCommandName)].Add(new(caller, method));
                        else
                            CommandHandlers.Add(new(attribute.CommandName, attribute.SubCommandName), [new(caller, method)]);
                    }

                object[] attribs3 = method.GetCustomAttributes(typeof(ComponentHandler), false);
                if (attribs3 is not null && attribs3.Length > 0)
                    foreach (object rawAttribute3 in attribs3)
                    {
                        ComponentHandler attribute = rawAttribute3 as ComponentHandler;
                        discordClient.Logger.Info($"Adding new listener:\n{JsonConvert.SerializeObject(attribute)}");
                        if (ComponentHandlers.ContainsKey(attribute.ComponentId))
                            ComponentHandlers[attribute.ComponentId].Add(new(caller, method));
                        else
                            ComponentHandlers.Add(attribute.ComponentId, [new(caller, method)]);
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
                            Task.Run(() => method.Value.Invoke(method.Key, [args]));
                        else
                            discordClient.Logger.Error($"Failed to invoke dynamic event handler: wrong args!\nExpected {args.GetType().FullName}, got {method.Value.GetParameters()[0].ParameterType.FullName}");
                    }
                    else
                        Task.Run(() => method.Value.Invoke(method.Key, []));
        }

        internal void InvokeCommand(string name, object args, ApplicationCommandInteractionData data = null)
        {
            InvokeBaseCommand(name, args);
            if (data is not null)
                foreach (ReplyCommandOption option in data.Options.Where(o => o.Type is (int)CommandOptionType.SUB_COMMAND or (int)CommandOptionType.SUB_COMMAND_GROUP))
                    InvokeSubCommand(name, option.Name, args as Interaction, option);
        }

        internal void InvokeComponent(string name, MessageComponentInteractionData args)
        {
            if (ComponentHandlers.TryGetValue(name, out HashSet<KeyValuePair<object, MethodInfo>> list))
                foreach (KeyValuePair<object, MethodInfo> method in list)
                    if (method.Value.GetParameters().Length > 0)
                    {
                        if (method.Value.GetParameters()[0].ParameterType == args.GetType())
                            Task.Run(() => method.Value.Invoke(method.Key, [args]));
                        else
                            discordClient.Logger.Error($"Failed to invoke dynamic event handler: wrong args!\nExpected {args.GetType().FullName}, got {method.Value.GetParameters()[0].ParameterType.FullName}");
                    }
                    else
                        Task.Run(() => method.Value.Invoke(method.Key, []));

        }

        private void InvokeBaseCommand(string name, object args)
        {
            if (CommandHandlers.TryGetValue(new(name, null), out HashSet<KeyValuePair<object, MethodInfo>> list))
                foreach (KeyValuePair<object, MethodInfo> method in list)
                    if (method.Value.GetParameters().Length > 0)
                    {
                        if (method.Value.GetParameters()[0].ParameterType == args.GetType())
                            Task.Run(() => method.Value.Invoke(method.Key, [args]));
                        else
                            discordClient.Logger.Error($"Failed to invoke dynamic event handler: wrong args!\nExpected {args.GetType().FullName}, got {method.Value.GetParameters()[0].ParameterType.FullName}");
                    }
                    else
                        Task.Run(() => method.Value.Invoke(method.Key, []));
        }

        private void InvokeSubCommand(string name, string subName, Interaction args, ReplyCommandOption data)
        {
            (args.Data as ApplicationCommandInteractionData).Options = data.Options;
            if (CommandHandlers.TryGetValue(new(name, subName), out HashSet<KeyValuePair<object, MethodInfo>> list))
                foreach (KeyValuePair<object, MethodInfo> method in list)
                    if (method.Value.GetParameters().Length > 0)
                    {
                        if (method.Value.GetParameters()[0].ParameterType == args.GetType())
                            Task.Run(() => method.Value.Invoke(method.Key, [args]));
                        else
                            discordClient.Logger.Error($"Failed to invoke dynamic event handler: wrong args!\nExpected {args.GetType().FullName}, got {method.Value.GetParameters()[0].ParameterType.FullName}");
                    }
                    else
                        Task.Run(() => method.Value.Invoke(method.Key, []));
        }
    }
}
