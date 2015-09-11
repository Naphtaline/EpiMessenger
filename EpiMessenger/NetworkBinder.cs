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
    class NetworkBinder : Binder
    {
        NetworkService service;

        public NetworkBinder(NetworkService service)
        {
            this.service = service;
        }

        public NetworkService GetService()
        {
            return service;
        }
    }
}