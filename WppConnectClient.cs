using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DalSoft.RestClient;
using Microsoft.AspNetCore.Http;

namespace PokemonBot_sample
{
    public class WppConnectClient
    {
        #region End Poinsts
        const string getStatusSession = "status-session";
        const string postStartSession = "start-session";
        const string postLogoutSession = "logout-session";
        const string postSendMessage = "send-message";
        const string postSendFile = "send-file";
        const string postSendImage = "send-image";
        #endregion

        #region Propriedades
        protected string Token;

        protected string HostUrl;

        protected string Url;

        protected string Session;

        private readonly IHttpContextAccessor httpContextAccessor;
        #endregion

        #region Client Config

        dynamic client;

        dynamic Client
        {
            get
            {
                if (client == null)
                    client = new RestClient($"{Url}/{Session}",
                        new Dictionary<string, string> { { "authorization", $"Bearer {Token}" } });
                return client;
            }
        }
        #endregion

        public WppConnectClient(string _hostUrl, string _url, string _session, string _token)
        {
            HostUrl = _hostUrl;
            Token = _token;
            Url = _url;
            Session = _session;
        }

        public dynamic Status()
        {
            return Client.Resource(getStatusSession).Get().Result;
        }

        public dynamic StartSession()
        {
            return Client.Resource(postStartSession).Post(new { webhook = HostUrl }).Result;
        }

        public dynamic LogoutSession()
        {
            return Client.Resource(postLogoutSession).Post().Result;
        }

        public dynamic SendMessage(string _phone, string _message)
        {
            object post = new { message = _message, phone = _phone, isGroup = _phone.Contains("@g.us") };
            return Client.Resource(postSendMessage).Post(post).Result;
        }

        public dynamic SendPokemon(string _phone, dynamic pokemon)
        {
            int len = 34;
            len = len - ((string)pokemon.name).Length;
            len += Convert.ToInt32(len / 3);
            string caption = $"{string.Empty.PadLeft(len, ' ')}*{pokemon.name.ToUpper()}*" +
                $"\n\n                *Altrua :* {((int)pokemon.height) / 10m}m          *Peso :*    {((int)pokemon.weight) / 10m}kg\n";
            object post = new { phone = _phone, path = $"https://pokeres.bastionbot.org/images/pokemon/{pokemon.id}.png", caption = caption,isGroup = _phone.Contains("@g.us") };
            return Client.Resource(postSendImage).Post(post).Result;
        }
        public dynamic SendFileUrl(string _phone, string _message, string filename, string _url)
        {
            object post = new { message = _message, phone = _phone, filename = filename, url = _url };

            return Client.Resource(postSendFile).Post(post).Result;

        }
    }
}
