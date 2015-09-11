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
    public class MainActivity : Activity, IServiceConnection
    {

        Button m_connectionButton;
        NetworkService m_netwokService;

        EditText m_login;
        EditText m_password;

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            m_netwokService = (service as NetworkBinder).GetService();
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Intent networkIntent;
            networkIntent = new Intent(this, typeof(NetworkService));
            BindService(networkIntent, this, Bind.AutoCreate);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //isServiceConnected = false;
            // Get our button from the layout resource,
            // and attach an event to it
            m_connectionButton = FindViewById<Button>(Resource.Id.connectButton);
            m_login = FindViewById<EditText>(Resource.Id.loginField);
            m_password = FindViewById<EditText>(Resource.Id.passwordField);

            m_connectionButton.Click += delegate {
                m_netwokService.SetLoginInfo(m_login.Text, m_password.Text);
                m_netwokService.Connect();
            };
        }

        private void OnLogin(bool p_isLogedIn)
        {
            if (p_isLogedIn == false)
            {
                AlertDialog.Builder l_alert = new AlertDialog.Builder(this);
                l_alert.SetMessage("Connection failed...");
                l_alert.SetNegativeButton("Cancel", delegate { });
            }
        }
    }
}

