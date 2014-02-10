using LuaInterface;
using System.Reflection;
using System.Collections.Generic;
namespace TextAdventure.IO
{
	namespace LuaSystem
	{
		public static class LuaManager
		{
			private struct QueuedFunction
			{
				public LuaBinding binding;
				public string funcName;
				public object[] param;
			}

			//Reference to the Lua virtual machine
			public static Lua lua
			{
				get
				{
					if (_lua == null)
					{
						Init ();
					}
					return _lua;
				}
			}
			private static Lua _lua = null;
			private static bool didAllFiles = false;
			//Filename of the Lua file to load
			public static void Init ()
			{
				if (_lua != null)
				{
					return;
				}
				//Init instance of Lua virtual machine (Note: Can only init ONCE)
				_lua = new LuaInterface.Lua ();

				System.Type t = typeof(LuaManager);
				MethodInfo print = t.GetMethod ("LuaPrint");
				lua.RegisterFunction ("print", null, print);
				lua.RegisterFunction ("error", null, t.GetMethod ("LuaError"));
				lua.RegisterFunction ("assert", null, t.GetMethod ("LuaAssert",
																BindingFlags.Static | BindingFlags.Public,
																System.Type.DefaultBinder,
																new[] {
					typeof(bool),
					typeof(string)
				},
																null)
				);

				//Init LuaBinding class that demonstrates communication
				//Also tell Lua about the LuaBinding object to allow Lua to call C# functions
				mainBinding = new MainBinding ();
			}
			public static LuaBinding mainBinding = null;
			private static List<string> filePaths = new List<string> ();
			private static List<QueuedFunction> queuedFuncs = new List<QueuedFunction> ();

			public static void AddFilePath (string path)
			{
				if (System.IO.File.Exists (path))
				{
					filePaths.Add (path);
				}
			}

			public static void DoAllFiles ()
			{
				if (didAllFiles)
				{
					return;
				}
				didAllFiles = true;
				foreach (string file in filePaths)
				{
					DoFile (file);
				}
				foreach (QueuedFunction queuedFunc in queuedFuncs)
				{
					LuaBinding binding = queuedFunc.binding;
					binding.CallLuaFunction (queuedFunc.funcName, queuedFunc.param);
				}
				queuedFuncs.Clear ();
			}

			public static void LuaPrint (object message)
			{
				//Output message into the debug log
				TextAdventure.Language.Output.Print (message);
			}

			public static void LuaError (object message)
			{
				throw new LuaException (message.ToString ());
			}

			public static bool QueueFunction (LuaBinding binding, string funcName, object[] param)
			{
				if (didAllFiles)
				{
					return true;
				}
				// This is a bit messy, but basically this keeps track of all functions
				QueuedFunction queuedFunc = new QueuedFunction ();
				queuedFunc.binding = binding;
				queuedFunc.funcName = funcName;
				queuedFunc.param = param;
				queuedFuncs.Add (queuedFunc);
				return false;
			}

			public static bool LuaAssert (bool value, object message)
			{
				if (value)
				{
					return true;
				}
				else
				{
					LuaError (message);
					return false;
				}
			}

			public static void DoFile (string dataPath)
			{
				string extension = System.IO.Path.GetExtension (dataPath);
				if (extension.ToLower () != ".lua")
				{
					throw new System.IO.IOException (dataPath + " is not an Lua file.");
				}
				lua.DoFile (dataPath);
			}

			public static bool DoString (string luaString)
			{
				try
				{
					lua.DoString (luaString);
					return true;
				}
				catch (System.Exception)
				{
					return false;
				}
			}
		}
	}
}