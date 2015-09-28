using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace EpiMessenger
{
    public class AddFriendPopup : DialogFragment
    {
        TextView loginValue;
        View m_view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup root, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            m_view = inflater.Inflate(Resource.Layout.AddPopup, root, false);
            Dialog.SetTitle("Enter a login :");
            m_view.FindViewById<Button>(Resource.Id.AddLoginButton).Click += AddNewFriend;
            return m_view;
        }

        private void AddNewFriend(object sender, EventArgs e)
        {
            string l_logins;
            string l_new;
            DataManager l_DataManager;

            l_new = m_view.FindViewById<EditText>(Resource.Id.LoginToAdd).Text;
            Console.WriteLine(l_new);
            if (l_new.Length > 7)
            {
                Toast.MakeText(m_view.Context, "Login too long...", ToastLength.Long);
                Dismiss();
                return;
            }
            l_DataManager = DataManager.GetDataManager();
            l_logins = l_DataManager.RetreiveData<string>("loginList");
            if (l_logins == null)
                l_logins = l_new;
            else
                l_logins += l_new;
            l_logins += ";";
            l_DataManager.StoreData<string>("loginList", l_logins);
            Console.WriteLine(l_logins);
            Dismiss();
            return;
        }
    }
}