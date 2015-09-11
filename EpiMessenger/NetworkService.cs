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
    [Service]
    class NetworkService : Service
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
                IPAddress ip = IPAddress.Parse("ns-server.epita.fr");
                IPEndPoint ipEnd = new IPEndPoint(ip, 4242);
                sok = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sok.Connect(ipEnd);
                net_read = new StreamReader(new NetworkStream(sok));
                net_write = new StreamWriter(new NetworkStream(sok));

                String data = net_read.ReadLine();
                net_write.WriteLine("auth_ag ext_user none none\n");
                net_read.ReadLine();
                net_write.WriteLine(GetAuthString(data.Split(' '), login, pass));
                if (net_read.ReadLine().Split(' ')[1] == "002")
                {
                    net_write.WriteLine("state actif:1174984764\n");
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
            while (true)
            {
                while (receive_msg && is_connect)
                {
                    String msg = net_read.ReadLine();
                    ParseMsg(msg);
                }
                Thread.Sleep(500);
            }
        }

        private void ParseMsg(String msg)
        {
            String[] msgs = msg.Split(' ');
            if (msgs[0] == "ping")
                net_write.WriteLine(msg);
            if (msgs[10] == "msg")
            {
                Console.WriteLine(msg);
            }
        }
    }
}