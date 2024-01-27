# EXPLICA��ES

Foi criado o microservi�o Lancamento.API, este � respons�vel por expor endpoints como:

	1. GET /lancamento: mostra todos os lan�amentos inseridos no banco de dados
	2. POST /lancamento: insere um lan�amento no banco de dados, para a correta inser��o deve-se informar: Data, Descri��o, Tipo (D ou C), valor e UsuarioId (Id do usuario que est� realizando 
	a inser��o). Ap�s a inser��o ser� inserido dado na fila para que o servi�o Consolidado.API consuma esses dados e tenha os dados dos lan�amentos consolidados.
	3. PUT /lancamento: edita um lan�amento. Deve-se informar os mesmos dados da inser��o mais o campo Id do registro que ser quer editar. Ap�s a edi��o ser� inserido dado na fila para que o 
	servi�o Consolidado.API consuma esses dados e tenha os dados dos lan�amentos consolidados.
	4. GET /lancamento/consolidate/{data}: esse endpoint retorna os dados consolidados da data informada
	5. POST /lancamento/reprocess/{data}: esse endpoint vai buscar os dados lan�ados para a data informada e vai enviar uma msg para a fila para que o servi�o Consolidado.API reprocesse os dados a partir dessa data. Isso � �til para caso em algum dia o saldo n�o esteja correto devido a alguma falha que possa ter acontecido, ent�o tem a op��o de reprocessar os lan�amentos e corrigir o saldo.
	6. GET /lancamento/{id}: mostra os dados do registro com o id informado
	7. DELETE /lancamento/{id}: apaga o registro. Ap�s a dele��o ser� inserido dado na fila para que o servi�o Consolidado.API consuma esses dados e tenha os dados dos lan�amentos consolidados.

Al�m desses endpoints foram criados endpoints para cria��o do usu�rio, isso foi feito para ter uma informa��o no lan�amento de qual usu�rio realizou o lan�amentos. O seguintes enpoints foram criados:

	1. GET /usuario: mostra todos os lan�amentos inseridos no banco de dados
	2. POST /usuario: insere um usu�rio no banco de dados
	3. PUT /usuario: edita um usu�rio no banco de dados baseado num id informado
	4. GET /usuario{id}: busca dados de um determinado usu�rio
	5. DELETE /usuario/{id}: apaga um usu�rio.

# Como a aplica��o funciona:
	- O usu�rio atrav�s da Lancamento.API vai realizar lan�amentos de debito ou cr�dito por dia, usando os endpoints explicado acima. Os dados ser�o armazenados no banco de dados dbLancamentos, 
	uma inst�ncia SqlServer contida no container Docker, al�m disso, uma mensagem ser� publicada no RabbitMQ que est� em um container Docker. Essa mensagem serve para a comunica��o entre o microservi�o 
	Lancamento.API com o microservi�o Consolidado.API. O servi�o Consolidado.API consome essa mensagem e insere os dados no banco de dados dbConsolidados que tamb�m � uma inst�ncia SqlServer e tamb�m 
	est� no container Docker.

	- O servi�os s�o completamente independentes, ou seja, se o Consolidados n�o estiver rodando, o usu�rio pode seguir com seus lan�amentos normalmente, quando o servi�o Consolidados voltar a ficar 
	dispon�vel, ele consumir� a fila e atualizar� os dados.

	- Se por alguma raz�o ocorrer algum erro e o saldo n�o ficar correto, o usu�rio poder� reprocessar os lan�amentos para aquele dia, a api vai ajustar os dados do dia e recalcular o saldo daquele 
	dia para a frente.

# Desenho da solu��o:

	- Na pasta Lancamento consta a imagem "Solucao.png" mostrando como foi pensada essa solu��o.

# Instala��o:

	- Clonar o reposit�rio com o comando "git clone https://github.com/jeansantos77/Lancamento.git"
	- Entrar dentro do diret�rio lancamento via cmd e digitar "docker-compose up" para iniciar o container
	- Ap�s o container estiver startado deve-se entrar no banco via Sql Management Studio e conectar no banco com usu�rio sa e senha "Senha@2023"
	- Clicar no menu na op��o "Query", isso vai abrir o SqlQuery para que seja poss�vel executar comandos
	- Copiar o script "InitialCreate.sql" contido em "Lancamento\Lancamento.API.Infra.Data\Migrations\Script"
	- Colar e executar na janela do SqlQuery, isso vai criar o banco de dados e as tabelas do banco dbLancamentos
	- Ap�s essa execu��o pode-se entrar na pasta "Lancamento\Lancamento.API" via CMD e digitar "dotnet run"
	- Abrir o browse e digitar "https://localhost:5001/swagger" que a aplica��o Lancamento.API estar� rodando.



