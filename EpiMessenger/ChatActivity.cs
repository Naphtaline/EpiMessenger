using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EpiMessenger
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : Activity
    {
        DataManager m_DataManager;

        Button m_AddFriendButton;
        string[] m_parsedLogin;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FriendList);

            m_DataManager = DataManager.GetDataManager();

            m_AddFriendButton = FindViewById<Button>(Resource.Id.AddFriendButton);

            m_parsedLogin = ParseLogin();

            //TODO : les afficher en liste

            m_AddFriendButton.Click += AddFriendButton;
        }

        private void AddFriendButton(object sender, EventArgs e)
        {
            RunOnUiThread(() => {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                AddFriendPopup dialFrag = new AddFriendPopup();
                dialFrag.Show(transaction, "lol");
            });
        }

        private string[] ParseLogin()
        {
            string l_logins;

            l_logins = m_DataManager.RetreiveData<string>("loginList");
            if (l_logins != null)
                return (l_logins.Split(';'));
            return null;
        }
    }
}