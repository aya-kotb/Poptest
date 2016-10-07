using UnityEngine;
using System.Collections;

namespace Poptropica2
{
	/// <summary>
	/// Interface for the example service. It's important to create an interface for
	/// each new service so that they can be identified in the SAMApplication. This also
	/// allows for implementations to be swapped out as needed. Service interfaces must
	/// inherit from IService.
	/// </summary>
	public interface IExampleService : IService
	{
		bool CheckIfCool();
		void SillyFunction();
	}

	/// <summary>
	/// Example service. This is the implementation of the service.
	/// </summary>
	public class ExampleService : IExampleService {
		private ExampleServiceConfig config;

		/// <summary>
		/// Starts the service. Called when the service is added to the application.
		/// </summary>
		/// <param name="application">The SAM Application instance.</param>
		public void StartService(SAMApplication application)
		{
			SillyFunction();
		}

		/// <summary>
		/// Stops the service. Called when the service is removed frmo the application.
		/// </summary>
		/// <param name="application">The SAM Application instance.</param>
		public void StopService(SAMApplication application)
		{
		}

		/// <summary>
		/// Configure the service with the specified config. This is called during game startup.
		/// </summary>
		/// <param name="config">The configuration for the service. Will be the specific type for the class.</param>
		/// <param name="serviceConfig">Service config.</param>
		public void Configure(ServiceConfiguration serviceConfig)
		{
			config = serviceConfig as ExampleServiceConfig;
		}

		public bool CheckIfCool()
		{
			return config.IsCool;
		}


		public void SillyFunction()
		{
			IExampleService svc = SAMApplication.mainInstance.GetService<IExampleService>();
			// now it's a bit silly to get our own service here, but imagine we are getting 
			// some other service like the quest service.
			Debug.Log("Is the service cool? " + svc.CheckIfCool());
		}
	}
}