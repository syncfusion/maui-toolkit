using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Collections.Concurrent;

namespace Syncfusion.Maui.ControlsGallery.PdfViewer.SfPdfViewer
{
	[Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, Exported = true)]
	class IntermediateActivity : Activity
	{
		const string LaunchedExtra = "launched";
		const string ActualIntentExtra = "actual_intent";
		const string GuidExtra = "guid";
		const string RequestCodeExtra = "request_code";

		static readonly ConcurrentDictionary<string, IntermediateTask> pendingTasks = new();

		bool _launched;
		Intent? _actualIntent;
		string? _guid;
		int _requestCode;

		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var extras = savedInstanceState ?? Intent?.Extras;

			// read the values
			_launched = extras?.GetBoolean(LaunchedExtra, false) ?? false;
#pragma warning disable 618 // TODO: one day use the API 33+ version: https://developer.android.com/reference/android/os/Bundle#getParcelable(java.lang.String,%20java.lang.Class%3CT%3E)
#pragma warning disable CA1422 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
			_actualIntent = extras?.GetParcelable(ActualIntentExtra) as Intent;
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore 618
			_guid = extras?.GetString(GuidExtra);
			_requestCode = extras?.GetInt(RequestCodeExtra, -1) ?? -1;
			if (GetIntermediateTask(_guid) is IntermediateTask task)
			{
				task.OnCreate?.Invoke(_actualIntent!);
			}

			// if this is the first time, lauch the real activity
			if (!_launched)
			{
				StartActivityForResult(_actualIntent, _requestCode);
			}
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			// make sure we mark this activity as launched
			outState.PutBoolean(LaunchedExtra, true);

			// save the values
			outState.PutParcelable(ActualIntentExtra, _actualIntent);
			outState.PutString(GuidExtra, _guid);
			outState.PutInt(RequestCodeExtra, _requestCode);

			base.OnSaveInstanceState(outState);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// we have a valid GUID, so handle the task
			if (GetIntermediateTask(_guid, true) is IntermediateTask task)
			{
				if (resultCode == Result.Canceled)
				{
					task.TaskCompletionSource.TrySetCanceled();
				}
				else
				{
					try
					{
						data ??= new Intent();

						task.OnResult?.Invoke(data);

						task.TaskCompletionSource.TrySetResult(data);
					}
					catch (Exception ex)
					{
						task.TaskCompletionSource.TrySetException(ex);
					}
				}
			}

			// close the intermediate activity
			Finish();
		}

		public static Task<Intent> StartAsync(Intent intent, int requestCode, Action<Intent>? onCreate = null, Action<Intent>? onResult = null)
		{
			// make sure we have the activity
			var activity = GetCurrentActivity(ActivityStateManager.Default, true)!;

			// create a new task
			var data = new IntermediateTask(onCreate, onResult);
			pendingTasks[data.Id] = data;

			// create the intermediate intent, and add the real intent to it
			var intermediateIntent = new Intent(activity, typeof(IntermediateActivity));
			intermediateIntent.PutExtra(ActualIntentExtra, intent);
			intermediateIntent.PutExtra(GuidExtra, data.Id);
			intermediateIntent.PutExtra(RequestCodeExtra, requestCode);

			// start the intermediate activity
			activity.StartActivityForResult(intermediateIntent, requestCode);

			return data.TaskCompletionSource.Task;
		}

		static IntermediateTask? GetIntermediateTask(string? guid, bool remove = false)
		{
			if (string.IsNullOrEmpty(guid))
			{
				return null;
			}

			if (remove)
			{
				pendingTasks.TryRemove(guid, out var removedTask);
				return removedTask;
			}

			pendingTasks.TryGetValue(guid, out var task);
			return task;
		}

		class IntermediateTask(Action<Intent>? onCreate, Action<Intent>? onResult)
		{
			public string Id { get; } = Guid.NewGuid().ToString();

			public TaskCompletionSource<Intent> TaskCompletionSource { get; } = new TaskCompletionSource<Intent>();

			public Action<Intent>? OnCreate { get; } = onCreate;

			public Action<Intent>? OnResult { get; } = onResult;
		}

		static Android.App.Activity? GetCurrentActivity(IActivityStateManager manager, bool throwOnNull)
		{
			var activity = manager.GetCurrentActivity();
			if (throwOnNull && activity == null)
			{
				throw new NullReferenceException("The current Activity can not be detected. Ensure that you have called Init in your Activity or Application class.");
			}

			return activity;
		}
	}
}
