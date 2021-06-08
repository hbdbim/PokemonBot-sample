using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DalSoft.RestClient;
using Microsoft.AspNetCore.Http;

namespace PokemonBot_sample
{
    public class PokemonApiClient
    {
        #region End Poinsts
        const string getPokemon = "pokemon";
        const string postStartSession = "start-session";
        const string postLogoutSession = "logout-session";
        const string postSendMessage = "send-message";
        const string postSendFile = "send-file";
        #endregion

        #region Propriedades
        protected string Url = "https://pokeapi.co/api/v2";
        #endregion

        #region Client Config

        dynamic client;

        dynamic Client
        {
            get
            {
                if (client == null)
                    client = new RestClient($"{Url}");
                return client;
            }
        }
        #endregion

        public PokemonApiClient()
        {

        }

        public dynamic PokemonInfo(string pokemon)
        {

            return Client.Resource($"{getPokemon}/{pokemon}").Get().Result;
        }
    }
}
