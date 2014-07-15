namespace OnSyte.Ui.Infra
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using Caliburn.Micro;
	using Crypto;
	using Properties;
	using SimpleInjector;
	using ViewModels;

	public class SiBootstrapper : BootstrapperBase
	{
		private Container _container;

		public SiBootstrapper()
		{
			Initialize();
			LogManager.GetLog = type => new DebugLogger(type);
		}

		protected override void Configure()
		{
			_container = new Container();

			_container.Register<IWindowManager, WindowManager>();
			_container.Register<IShell, ShellViewModel>();
			_container.RegisterSingle<IEventAggregator, EventAggregator>();
			_container.RegisterSingle<ICryptoProvider>(new Blowfish(Settings.Default.BlowfishKey));

			_container.Verify();
		}

		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_container.InjectProperties(instance);
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<IShell>();
		}
	}
}