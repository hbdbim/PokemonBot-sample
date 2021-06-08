using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonBot_sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokeDexController : ControllerBase
    {
        #region Pokemin Hunter
        private static List<string> _pokeHunters;
        private List<string> PokeHunters
        {
            get
            {
                if (_pokeHunters == null)
                    _pokeHunters = new List<string>();
                return _pokeHunters;
            }
        }
        #endregion

        #region Wpp Connect Client
        private static WppConnectClient client;
        protected WppConnectClient Client
        {
            get
            {
                if (client == null)
                    client = new WppConnectClient($"{Request.Scheme}://{Request.Host}/pokedex", "http://localhost:21465/api", "pokedex", "$2b$10$llkjYZaMIPVc8MfuAPLA4OGBEe2lQQBTsCsAZOHLrEljG7NF5j5Jq");
                return client;
            }


        }
        #endregion

        #region Wpp Connect Client
        private static PokemonApiClient pokeclient;
        protected PokemonApiClient PokeClient
        {
            get
            {
                if (pokeclient == null)
                    pokeclient = new PokemonApiClient();
                return pokeclient;
            }


        }
        #endregion

        private readonly Random _random = new Random();

        const string MensagemInicial = "Ola! Eu sou o PokeBot 🤖. Sua agenda pokemon";
        const string MensagemComandos = "Aqui a lista de funções que possuo !" +
            "\n\nPara ver os dados de um pokemon digite o nome ou o numero do pokemon" +
            "\npode tambem digitar *aleatorio* que eu escolho um" +
            "\n\ncaso eu durma me chama (pokemon,bot) que eu acordo rapidinho !";

        string[] c_aleatorio = { "aleatorio", "aleatório" };


        const string MesagemFallBack = "Sinto muito, não encontrei seu pokemon, se esta em duvida digite *bot*";

        private readonly ILogger<WeatherForecastController> _logger;

        public PokeDexController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string Post([FromBody] dynamic jsonbody)
        {
            dynamic data = JObject.Parse(jsonbody.ToString());

            //Verifica o evento do webhook
            if (data.@event == "onmessage")
            {
                string pokeHunter = data.from;

                string comando = ((string)data.body).Split(" ")[0];
                dynamic result = null;

                if (!PokeHunters.Contains(pokeHunter) || comando.Contains("pokemon", StringComparison.InvariantCultureIgnoreCase)
                    || comando.Contains("bot", StringComparison.InvariantCultureIgnoreCase)
                    || comando.Contains("pokebot", StringComparison.InvariantCultureIgnoreCase))
                {
                    PokeHunters.Add(pokeHunter);
                    Client.SendMessage(pokeHunter, MensagemInicial);
                    Client.SendMessage(pokeHunter, MensagemComandos);

                }
                else
                {
                    if (c_aleatorio.Contains(comando.ToLower()))
                        comando = new Random().Next(800).ToString();

                    result = PokeClient.PokemonInfo(comando);
                }


                if (result != null && result.HttpResponseMessage.IsSuccessStatusCode)
                {
                    Client.SendPokemon(pokeHunter, result);
                }
                else
                    Client.SendMessage(pokeHunter, MesagemFallBack);
            }
            return MensagemInicial;
        }
    }
}
