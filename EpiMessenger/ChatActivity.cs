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
                // Ajouter un pop up dans lequel on peut écrire un login
                // Ajouter 2 bouton : Ajouter / annuler 
            });
        }

        private string[] ParseLogin()
        {
            string l_logins;

            l_logins = m_DataManager.RetreiveData<string>("login");
            if (l_logins != null)
                return (l_logins.Split(';'));
            return null;
        }

        private void AddNewFriend(string p_login)
        {
            string l_logins;

            l_logins = m_DataManager.RetreiveData<string>("login");
            l_logins += p_login;
            m_DataManager.StoreData<string>("login", l_logins);
        }
    }
}