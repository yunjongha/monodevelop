using System;
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Components;

namespace MonoDevelop.ConnectedServices
{
	/// <summary>
	/// Abstract implementation of IConfigurationSection.
	/// </summary>
	public abstract class ConfigurationSection : IConfigurationSection
	{
		public static readonly IConfigurationSection [] Empty = new IConfigurationSection [0];

		protected ConfigurationSection (IConnectedService service, string displayName, bool canBeAdded = true)
		{
			this.Service = service;
			this.DisplayName = displayName;
			this.CanBeAdded = canBeAdded;
		}

		/// <summary>
		/// Gets the service for this section
		/// </summary>
		public IConnectedService Service { get; private set; }

		/// <summary>
		/// Gets the name of the section to display to the user.
		/// </summary>
		public string DisplayName { get; private set; }

		/// <summary>
		/// Gets the description of the section to display to the user.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// Gets a value indidating if this section represents something that can be added to the project.
		/// </summary>
		public bool CanBeAdded { get; private set; }

		/// <summary>
		/// Gets a value indicating that whatever changes to the project that can be added by this section have been added.
		/// </summary>
		public abstract bool IsAdded { get; }

		/// <summary>
		/// Occurs when the section is added to the project
		/// </summary>
		public event EventHandler<EventArgs> Added;

		/// <summary>
		/// Occurs before the section is added to the project
		/// </summary>
		public event EventHandler<EventArgs> Adding;

		/// <summary>
		/// Gets the widget to display to the user
		/// </summary>
		public abstract Control GetSectionWidget ();

		/// <summary>
		/// Performs the tasks necessary to add the components that this section represents to the project
		/// </summary>
		public async Task<bool> AddToProject (CancellationToken token)
		{
			this.NotifyAddingToProject ();
			var result = await this.OnAddToProject (token).ConfigureAwait (false);
			if (result)
				this.NotifyAddedToProject ();
			return result;
		}

		protected abstract Task<bool> OnAddToProject (CancellationToken token);

		/// <summary>
		/// Invokes the Adding event on the main thread
		/// </summary>
		protected void NotifyAddingToProject ()
		{
			var handler = this.Adding;
			if (handler != null) {
				Xwt.Application.Invoke (() => {
					handler (this, new EventArgs ());
				});
			}
		}

		/// <summary>
		/// Invokes the Added event on the main thread
		/// </summary>
		protected void NotifyAddedToProject ()
		{
			var handler = this.Added;
			if (handler != null) {
				// make sure this gets called on the main thread in case we have async calls for adding a nuget
				Xwt.Application.Invoke (() => {
					handler (this, new EventArgs ());
				});
			}
		}
	}
}