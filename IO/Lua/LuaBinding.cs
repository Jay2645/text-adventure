using TextAdventure.Observers;
using LuaInterface;
using TextAdventure.GameObjects;
using System;
using System.Collections.Generic;
namespace TextAdventure.IO
{
	namespace LuaSystem
	{
		public class LuaBinding : Observer
		{
			protected LuaBinding ()
			{
				/* EMPTY */
			}
			public LuaBinding (string binding)
			{
				SetBinding (binding);
			}

			protected void SetBinding (string binding)
			{
				binding = binding.ToLower ();
				binding = binding.Replace (" ", "");
				LuaManager.lua [binding] = this;
				name = binding;
			}

			protected Dictionary<string, LuaFunction> functionList = new Dictionary<string, LuaFunction> ();
			protected string name = "";
			public void BindMessageFunction (LuaFunction func, string name)
			{
				//Binding
				if (functionList.ContainsKey (name))
				{
					functionList [name] = func;
				}
				else
				{
					functionList.Add (name, func);
				}
			}

			public object[] MessageToLua (LuaFunction luaFunction, object[] args)
			{
				if (args == null)
				{
					return luaFunction.Call ();
				}
				else
				{
					return luaFunction.Call (args);
				}
			}

			public override void OnNotify (object entity, EventList eventType)
			{
				CallLuaFunction (eventType.ToString (), new object[] {
					entity
				}
				);
			}

			public object[] CallLuaFunction (string funcName)
			{
				return CallLuaFunction (funcName, null);
			}

			public object[] CallLuaFunction (string funcName, object[] args)
			{
				foreach (KeyValuePair<string, LuaFunction> kvp in functionList)
				{
					if (kvp.Key.ToLower () == funcName.ToLower ())
					{
						return MessageToLua (kvp.Value, args);
					}
				}
				return null;
			}
		}
	}
}