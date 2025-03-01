# Motor Centro de Distribuição

O motor para consulta de centro de distribuição (CD) é uma aplicação utilizada para consultar os CDs vinculados a 1 ou n itens de um pedido no carrinho de compra

## Cenário

- Old: API que fornece os CDs por itens, que permite apenas a consulta de um item por vez;
- New: Nova API que fornece os CDs por itens, e que permite a consulta de todos os itens de um pedido de uma vez;

Requisito: A nova API deve consumir os dados da antiga.

## Old API

Para simular a API de consulta de CDs por item foi criado um projeto mock, com as características abaixo:

#### Variáveis de ambiente:
```
- [ERROR] Pode ser configurada uma possibilidade percentual de lançar erros 500 em cada requisição. Isso permite testar a resiliência da nova app;
- [DELAY] Também pode ser configurado um delay adicional para a resposta da requisição, de modo a simular a latência de uma aplicação real;
- [TODOS_CDS] E a quantidade máxima de CDs também pode ser ajustada, permitindo retornar no máximo 6 ou 20 centros de distribuição;
```

#### Requisições
```http
  GET /distribuitioncenters?itemId={itemId}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `itemId` | `string` | **Required**. Id do item a ser consultado |


```http
Response:

{
  distribuitionCenters: ["CD1", "CD2", "CD3"]
}

```
## New API

O projeto principal possui as seguintes características:

#### Respostas [POST]:
```
- Caso todos os itens do pedido sejam validos, e processados com sucesso, a resposta da requisição terá status 200 (ok), com o ID do pedido, itens com seus CDs, e data de validade do pedido;
- Caso todos os itens não sejam validos por não existirem, a resposta da requisição terá status 404 (not found);
- Caso pelo menos um dos itens tenha falha no processamento, ou não esteja disponível em nenhum centro de distribuição, a requisição retornará o status 404 (bad request) com todos os itens com falha (e os respectivos motivos), todos os itens processados com os seus CDs (caso existam), e todos os itens não encontrado (caso existam). Porém, nesse caso o pedido não terá um ID, e não será possível consulta-lo posteriormente.
```

#### Variáveis de ambiente:
```
- [MAX_REQ_PARALELAS] Pode ser configurado o paralelismo para realizar as requisições REST;
- [ASPNETCORE_ENVIRONMENT] Essa variável é importante, e deve ter o valor "docker" quando for executada em container, para permitir a comunicação entre as apps;
```

#### Requisições
```http
  GET /pedidos/{pedidoId}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `pedidoId` | `string` | **Required**. Id do pedido a ser consultado |

```http
Response: 

{
	"pedidoId": "647be05a-545e-4111-b0b1-df6bab86b898",
	"itens": [
		{
			"item": 2,
			"distribuitionCenters": [
				"CD2",
				"CD3",
				"CD4",
				"CD5",
				"CD6"
			]
		},
		{
			"item": 115,
			"distribuitionCenters": [
				"CD1",
				"CD2"
			]
		}
	],
	"validade": "2025-02-07T00:57:09.5488945+00:00"
}
```

```http
  POST /pedidos
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `pedido` | `Pedido` | **Required**. Pedido a ser processado |

```http
Request (Body):

{
	"itens": [1,11,12,120]
}

```

```http
Response: Igual ao do GET

```

## Execução

### Visual Studio
#### Requisitos
Para editar e fazer a depuração do código, são necessários:
- [Recomendado] Visual Studio 2022 (ou superior). Todavia, pode ser utilizada outra IDE com suporte a .net 8;
- .NET 8;

#### Execução
Clicar com o botão direito sobre a solução, e configurar ambos os projetos como de inicialização. 
O swagger de ambas abrirá assim que as apps forem executadas.

### Container
#### Requisitos
- Docker

#### Execução
Abrir o CMD na pasta raiz do arquivo docker-compose.yml, e executar o comando:

```bash
  docker-compose up --build
```
Depois de executadas, as apps podem ser acessadas em:
- Consulta CDs: `localhost:9191/swagger/index.html`;
- Motor de consulta: `localhost:9292/swagger/index.html`


## Tecnologias e Frameworks

| Tech             | Comentário                                                                |
| ----------------- | ------------------------------------------------------------------ |
| Banco de dados| [LiteDB] Para fins de teste, a aplicação esta utilizando uma base de dados chave-valor em memória |
| Retentativa | [Polly] Para casos de retentativas em requisições falhas |
| Circuit-Breaker | [Polly] Para evitar estressar endpoints não saudaveis e diminuir erros em tentativas de realizar uma requisição |
| Auto-Mapeamento | [AutoMapper] Para mapeamento entre classes |
| Log | [Serilog] |
| Cache | [Caching.Memory] Cacheamento do resultado de requisições HTTP com sucesso |