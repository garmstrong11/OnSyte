namespace OnSyte.Ui.Infra
{
	using System;
	using System.Diagnostics;
	using Caliburn.Micro;

	public class DebugLogger : ILog
	{
		private readonly Type _type;

		public DebugLogger(Type type)
		{
			_type = type;
		}

		public void Info(string format, params object[] args)
		{
			Debug.WriteLine(CreateLogMessage(format, args), "INFO");
		}

		public void Warn(string format, params object[] args)
		{
			Debug.WriteLine(CreateLogMessage(format, args), "WARN");
		}

		public void Error(Exception exception)
		{
			Debug.WriteLine(exception.Message, "ERROR");
		}

		private string CreateLogMessage(string format, params object[] args)
		{
			return string.Format("[{0}] {1}", _type.Name, string.Format(format, args));
		}
	}
}