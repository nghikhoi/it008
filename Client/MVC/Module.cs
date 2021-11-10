using System.Collections.Generic;

namespace UI.MVC {
	
	public class Module {
		
		public IModel model { get; private set; }

		public IView view { get; private set; }

		public IController controller { get; private set; }

		public void InitializeMVC(IModel model, IView view, IController controller)
		{
			this.model = model;
			this.view = view;
			this.controller = controller;
		}
		
	}

	public class Module<M, V, C> : Module
		where M : IModel
		where V : IView
		where C : IController {
		new public M model { get; private set; }
		new public V view { get; private set; }
		new public C controller { get; private set; }

		public void InitializeMVC(M model, V view, C controller)
		{
			this.model = model;
			this.view = view;
			this.controller = controller;
			ModuleContainer.Register(this);
		}

		~Module() {
			ModuleContainer.Unregister(this);
		}
	}
	
	public class ModuleContainer {

		public static List<Module> modules { get; private set; } = new List<Module>();

		/// <summary>
		/// Find application 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetModule<T>() where T : Module
		{
			foreach (Module module in modules)
			{
				if (module is T)
					return module as T;
			}

			return null;
		}

		public static void Register(Module module)
		{
			modules.Add(module);
		}

		public static void Unregister(Module module)
		{
			modules.Remove(module);
		}

		public static void Clear()
		{
			modules.Clear();
		}


		// This will be called when connection to server is lost
		public static void OnDisconnection(bool lostConnection = false)
		{
            
		}

		public static void OnReconnected()
		{
            
		}
	}
	
}