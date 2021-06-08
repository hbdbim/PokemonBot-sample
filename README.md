# PokemonBot-sample


Não foi implementado no bot a parte de conexão, entao inicie a sessao no server (pokedex). 
Se voce utilizar outro nome de sessão ou outra secret key no server o token gerado sera diferente e voce vai precisar 
alterar na controller o nome da sessao e o token

```C#
client = new WppConnectClient($"{Request.Scheme}://{Request.Host}/pokedex", "http://localhost:21465/api",
                        "pokedex",
                        "$2b$10$llkjYZaMIPVc8MfuAPLA4OGBEe2lQQBTsCsAZOHLrEljG7NF5j5Jq");
```

Lembre-se de verificar na hora de iniciar a sessao no server se o endereco do webhook esta correto.
```
http://localhost:2076/pokedex
```
