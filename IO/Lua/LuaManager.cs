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
				lua.RegisterFunction ("print", null, t.GetMethod ("LuaPrint"));
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
				lua.RegisterFunction ("require", null, t.GetMethod ("AddFilePath"));

				//Init LuaBinding class that demonstrates communication
				//Also tell Lua about the LuaBinding object to allow Lua to call C# functions
				mainBinding = new MainBinding ();
			}
			public static LuaBinding mainBinding = null;
			private static Dictionary<string, bool> filePaths = new Dictionary<string, bool> ();
			private static List<QueuedFunction> queuedFuncs = new List<QueuedFunction> ();
			private static List<string> registeredBindings = new List<string> ();
			public static string scriptsPath = "";

			public static void RegisterBinding (string name, LuaBinding binding)
			{
				lua [name] = binding;
				registeredBindings.Add (name);
				CheckBindings ();
			}

			public static object[] AddFilePath (string path)
			{
				path = path.Replace ('.', '/'); // Change lua-style directories to regular directories
				if (path.EndsWith ("/lua")) // Fix extension
				{
					path = path.Substring (0, path.Length - 4);
				}
				path += ".lua";
				Debug.Log (path);
				path = System.IO.Path.Combine (scriptsPath, path);
				if (System.IO.File.Exists (path))
				{
					return DoFile (path);
				}
				return null;
			}

			private static void CheckBindings ()
			{
				List<QueuedFunction> removeFunctions = new List<QueuedFunction> ();
				foreach (QueuedFunction queuedFunc in queuedFuncs)
				{
					LuaBinding binding = queuedFunc.binding;
					if (registeredBindings.Contains (binding.name))
					{
						binding.CallLuaFunction (queuedFunc.funcName, queuedFunc.param);
						removeFunctions.Add (queuedFunc);
					}
				}
				foreach (QueuedFunction func in removeFunctions)
				{
					queuedFuncs.Remove (func);
				}
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
				if (registeredBindings.Contains (binding.name))
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

			public static object[] DoFile (string dataPath)
			{
				if (filePaths.ContainsKey (dataPath))
				{
					if (filePaths [dataPath])
					{
						return null;
					}
				}
				string extension = System.IO.Path.GetExtension (dataPath);
				if (extension.ToLower () != ".lua")
				{
					throw new System.IO.IOException (dataPath + " is not an Lua file.");
				}
				object[] output = lua.DoFile (dataPath);
				if (filePaths.ContainsKey (dataPath))
				{
					filePaths [dataPath] = true;
				}
				else
				{
					filePaths.Add (dataPath, true);
				}
				return output;
			}

			public static bool DoString (string luaString)
			{
				try
				{
					lua.DoString (luaString);
					return true;
				}
				catch (LuaInterface.LuaException)
				{
					return false;
				}
			}
		}
	}
}