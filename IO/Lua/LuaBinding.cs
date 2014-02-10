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

			/// <summary>
			/// Sets the string used by Lua to reference this binding.
			/// i.e. globals:BindMessageFunction(etc)
			/// </summary>
			/// <param name='binding'>
			/// The string used to reference this binding.
			/// </param>
			protected void SetBinding (string binding)
			{
				binding = binding.ToLower ();
				binding = binding.Replace (" ", "");
				LuaManager.lua [binding] = this;
				name = binding;
			}

			protected Dictionary<string, LuaFunction> functionList = new Dictionary<string, LuaFunction> ();
			protected string name = "";

			/// <summary>
			/// Binds an Lua function to a string.
			/// </summary>
			/// <param name='func'>
			/// The Lua function.
			/// </param>
			/// <param name='name'>
			/// The string used to reference it.
			/// </param>
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

			/// <summary>
			/// Removes a bound Lua function.
			/// Any subsequent calls to this function name will no longer go through.
			/// </summary>
			/// <param name='name'>
			/// The name of the function.
			/// </param>
			public void RemoveBoundFunction (string name)
			{
				if (functionList.ContainsKey (name))
				{
					functionList.Remove (name);
				}
			}

			/// <summary>
			/// Calls a function with the given name.
			/// </summary>
			/// <returns>
			/// The output of the function, or null if the function was not found.
			/// </returns>
			/// <param name='funcName'>
			/// The name of the function.
			/// </param>
			public object[] CallLuaFunction (string funcName)
			{
				return CallLuaFunction (funcName, null);
			}

			/// <summary>
			/// Calls a function with a given name and parameters.
			/// </summary>
			/// <returns>
			/// The output of the function, or null if no function was found.
			/// </returns>
			/// <param name='funcName'>
			/// The name of the function.
			/// </param>
			/// <param name='args'>
			/// The arguments to pass.
			/// </param>
			public object[] CallLuaFunction (string funcName, object[] args)
			{
				funcName = funcName.ToLower ();
				if (!LuaManager.QueueFunction (this, funcName, args))
				{
					return null;
				}
				if (Globals.isDebug)
				{
					Language.Output.Print (name + ":" + funcName);
				}
				foreach (KeyValuePair<string, LuaFunction> kvp in functionList)
				{
					if (kvp.Key.ToLower () == funcName)
					{
						return MessageToLua (kvp.Value, args);
					}
				}
				return null;
			}

			/// <summary>
			/// Calls the given LuaFunction with the specified arguments.
			/// </summary>
			/// <returns>
			/// The output of the LuaFunction.
			/// </returns>
			/// <param name='luaFunction'>
			/// The LuaFunction to call.
			/// </param>
			/// <param name='args'>
			/// The arguments to pass.
			/// </param>
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

			/// <summary>
			/// Raises the notify event.
			/// Will call any function bound to the specified EventList.
			/// </summary>
			/// <param name='entity'>
			/// The entity to pass as a parameter.
			/// </param>
			/// <param name='eventType'>
			/// The type of event.
			/// </param>
			public override void OnNotify (object entity, EventList eventType)
			{
				CallLuaFunction (eventType.ToString (), new object[] {
					entity
				}
				);
			}
		}
	}
}