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

            SetContentView(Resource.Layout.Main);

            m_connectionButton = FindViewById<Button>(Resource.Id.connectButton);
            m_login = FindViewById<EditText>(Resource.Id.loginField);
            m_password = FindViewById<EditText>(Resource.Id.passwordField);
            StartService(new Intent(this, typeof(NetworkService)));
            BindService(new Intent(this, typeof(NetworkService)), this, Bind.AutoCreate);

            m_connectionButton.Click += delegate {
                m_netwokService.SetLoginInfo(m_login.Text, m_password.Text);
                m_netwokService.LoginEvent += OnLogin;
                m_netwokService.Login();
            };
        }

        private void OnLogin(bool p_isLogedIn)
        {
            if (p_isLogedIn == false)
            {
                RunOnUiThread(() => {
                    AlertDialog.Builder l_alert = new AlertDialog.Builder(this);
                    l_alert.SetMessage("Connection failed...");
                    l_alert.SetNegativeButton("Cancel", delegate { });
                    Console.WriteLine("Failed to connect");
                    l_alert.Show();
                });
            }
            else
            {
                Console.WriteLine("success to connect");
            }
        }
    }
}

