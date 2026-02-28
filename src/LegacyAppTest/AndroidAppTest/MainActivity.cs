using System;
using System.Text;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;

using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;

using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;

using Rxmxnx.PInvoke.ApplicationTest;

using Xamarin.Essentials;

namespace AndroidAppTest
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Platform.Init(this, savedInstanceState);
			this.SetContentView(Resource.Layout.activity_main);

			Toolbar? toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
			this.SetSupportActionBar(toolbar);

			FloatingActionButton? fab = this.FindViewById<FloatingActionButton>(Resource.Id.fab);
			if (fab != null)
				fab.Click += MainActivity.FabOnClick;

			AppCompatTextView? text = this.FindViewById<AppCompatTextView>(Resource.Id.textView1);
			if (text == null) return;
			StringBuilder sb = new();

			RuntimeHelper.PrintRuntimeInfo(sb);
			text.Text = sb.ToString();
		}
		public override Boolean OnCreateOptionsMenu(IMenu? menu)
		{
			this.MenuInflater.Inflate(Resource.Menu.menu_main, menu);
			return true;
		}
		public override Boolean OnOptionsItemSelected(IMenuItem item)
		{
			Int32 id = item.ItemId;
			return id == Resource.Id.action_settings || base.OnOptionsItemSelected(item);
		}
		public override void OnRequestPermissionsResult(Int32 requestCode, String[] permissions,
			[GeneratedEnum] Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			if ((Int32)Build.VERSION.SdkInt >= 23)
				base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		private static void FabOnClick(Object sender, EventArgs eventArgs)
		{
			View view = (View)sender;
			Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
			        .SetAction("Action", (View.IOnClickListener?)null).Show();
		}
	}
}