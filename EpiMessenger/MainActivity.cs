using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace EpiMessenger
{
    [Activity(Label = "EpiMessenger", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        Button button;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //isServiceConnected = false;
            // Get our button from the layout resource,
            // and attach an event to it
            button = FindViewById<Button>(Resource.Id.connectButton);
            
            button.Click += delegate { button.Text = "ok ca marche"; };
        }
    }
}

