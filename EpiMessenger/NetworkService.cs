using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using Android.Util;

namespace EpiMessenger
{
    [Service] class NetworkService : Service
    {
        private bool is_connect;
        private Socket sok;
        public event OnLogin LoginEvent;
        public delegate void OnLogin(bool result);
        public event OnMsg MsgEvent;
        public delegate void OnMsg(String send, String msg);
        private String login;
        private String pass;
        private StreamReader net_read;
        private StreamWriter net_write;
        private bool receive_msg;

        public override void OnCreate()
        {
            base.OnCreate();
            receive_msg = false;
            Update();
        }

        public override IBinder OnBind(Intent intent)
        {
            return (new NetworkBinder(this));
        }

        public bool IsConnect()
        {
            return (is_connect);
        }

        public void SetLoginInfo(String login, String pass)
        {
            this.login = login;
            this.pass = pass;
        }

        private String GetAuthString(String[] datas, String login, String pass)
        {
            String auth = "ext_user_log ";
            auth += login + " ";
            auth += Utils.CalculateMD5Hash(datas[2] + '-' + datas[3] + '/' + datas[4] + pass).ToLower() + ' ';
            auth += "#EpiMessenger #EpiMessenger\n";
            return auth;
        }

        public void Login()
        {
            var t = new Thread(() =>
            {
                IPAddress ip = Dns.GetHostEntry("ns-server.epita.fr").AddressList[0];
                IPEndPoint ipEnd = new IPEndPoint(ip, 4242);
                sok = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sok.Connect(ipEnd);
                net_read = new StreamReader(new NetworkStream(sok));
                net_write = new StreamWriter(new NetworkStream(sok));

                Console.WriteLine("Connecter.............");

                String data = net_read.ReadLine();
                net_write.Write("auth_ag ext_user none none\n");
                net_write.Flush();
                net_read.ReadLine();
                net_write.Write(GetAuthString(data.Split(' '), login, pass));
                net_write.Flush();
                if (net_read.ReadLine().Split(' ')[1] == "002")
                {
                    net_write.Write("state actif:1174984764\n");
                    net_write.Flush();
                    is_connect = true;
                    if (LoginEvent != null)
                        LoginEvent(true);
                }
                else if (LoginEvent != null)
                    LoginEvent(false);
            });
            t.Start();
        }

        public void StartUpdate()
        {
            receive_msg = true;
        }

        public void StopUpdate()
        {
            receive_msg = false;
        }

        private void Update()
        {
            var t = new Thread(() =>
            {
                while (true)
                {
                    while (receive_msg && is_connect)
                    {
                        String msg = net_read.ReadLine();
                        ParseMsg(msg);
                    }
                    Thread.Sleep(500);
                }
            });
            t.Start();
        }

        private void ParseMsg(String msg)
        {
            String[] msgs = msg.Split(' ');
            if (msgs[0] == "ping")
            {
                net_write.Write(msg);
                net_write.Flush();
            }
            else//if (msgs[10] == "msg")
            {
                Console.WriteLine(msg);
                var nMgr = (NotificationManager)GetSystemService(NotificationService);
                var notification = new Notification(Resource.Drawable.Icon, "Message from netsoul");
                var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);
                notification.SetLatestEventInfo(this, "network Service Notification", msg, pendingIntent);
                nMgr.Notify(0, notification);
            }
        }
    }
}