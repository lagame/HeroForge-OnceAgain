using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HeroForge_OnceAgain
{
    public class HttpListenerService
    {
        private static HttpListenerService _instance;
        private HttpListener _listener;

        private HttpListenerService()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:8080/");
        }

        public static HttpListenerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HttpListenerService();
                }
                return _instance;
            }
        }

        public void StartListener()
        {
            if (!_listener.IsListening)
            {
                _listener.Start();
            }
        }

        public void StopListener()
        {
            if (_listener.IsListening)
            {
                _listener.Stop();
            }
        }

        // Você pode adicionar outros métodos conforme necessário, 
        // como um método para lidar com as solicitações que chegam.
    }

}
