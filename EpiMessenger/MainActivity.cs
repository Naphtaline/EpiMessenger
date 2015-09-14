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

        NetworkService m_netwokService;
        DataManager m_dataManager;

        Button m_connectionButton;
        EditText m_login;
        EditText m_password;
        CheckBox m_checkBox;

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

            m_dataManager = DataManager.GetDataManager();

            m_connectionButton = FindViewById<Button>(Resource.Id.connectButton);
            m_login = FindViewById<EditText>(Resource.Id.loginField);
            m_password = FindViewById<EditText>(Resource.Id.passwordField);
            m_checkBox = FindViewById<CheckBox>(Resource.Id.rememberPass);

            // Store user data
            if (m_dataManager.RetreiveData<bool>("loginCheckBox") == true)
            {
                m_login.Text = m_dataManager.RetreiveData<string>("login");
                m_password.Text = m_dataManager.RetreiveData<string>("password");
                m_checkBox.Checked = m_dataManager.RetreiveData<bool>("loginCheckBox");
            }

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
                if (m_checkBox.Checked == true)
                {
                    m_dataManager.StoreData<string>("login", m_login.Text);
                    m_dataManager.StoreData<string>("password", m_password.Text);
                }
                else
                {
                    m_dataManager.RemoveData("login");
                    m_dataManager.RemoveData("password");
                }
                m_dataManager.StoreData<bool>("loginCheckBox", m_checkBox.Checked);
                Console.WriteLine("success to connect");
                StartActivity(typeof(ChatActivity));
            }
        }
    }
}

