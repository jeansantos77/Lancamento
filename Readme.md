# EXPLICAÇÕES

Foi criado o microserviço Lancamento.API, este é responsável por expor endpoints como:

	1. GET /lancamento: mostra todos os lançamentos inseridos no banco de dados
	2. POST /lancamento: insere um lançamento no banco de dados, para a correta inserção deve-se informar: Data, Descrição, Tipo (D ou C), valor e UsuarioId (Id do usuario que está realizando 
	a inserção). Após a inserção será inserido dado na fila para que o serviço Consolidado.API consuma esses dados e tenha os dados dos lançamentos consolidados.
	3. PUT /lancamento: edita um lançamento. Deve-se informar os mesmos dados da inserção mais o campo Id do registro que ser quer editar. Após a edição será inserido dado na fila para que o 
	serviço Consolidado.API consuma esses dados e tenha os dados dos lançamentos consolidados.
	4. GET /lancamento/consolidate/{data}: esse endpoint retorna os dados consolidados da data informada
	5. POST /lancamento/reprocess/{data}: esse endpoint vai buscar os dados lançados para a data informada e vai enviar uma msg para a fila para que o serviço Consolidado.API reprocesse os dados a partir dessa data. Isso é útil para caso em algum dia o saldo não esteja correto devido a alguma falha que possa ter acontecido, então tem a opção de reprocessar os lançamentos e corrigir o saldo.
	6. GET /lancamento/{id}: mostra os dados do registro com o id informado
	7. DELETE /lancamento/{id}: apaga o registro. Após a deleção será inserido dado na fila para que o serviço Consolidado.API consuma esses dados e tenha os dados dos lançamentos consolidados.

Além desses endpoints foram criados endpoints para criação do usuário, isso foi feito para ter uma informação no lançamento de qual usuário realizou o lançamentos. O seguintes enpoints foram criados:

	1. GET /usuario: mostra todos os lançamentos inseridos no banco de dados
	2. POST /usuario: insere um usuário no banco de dados
	3. PUT /usuario: edita um usuário no banco de dados baseado num id informado
	4. GET /usuario{id}: busca dados de um determinado usuário
	5. DELETE /usuario/{id}: apaga um usuário.

# Como a aplicação funciona:
	- O usuário através da Lancamento.API vai realizar lançamentos de debito ou crédito por dia, usando os endpoints explicado acima. Os dados serão armazenados no banco de dados dbLancamentos, 
	uma instância SqlServer contida no container Docker, além disso, uma mensagem será publicada no RabbitMQ que está em um container Docker. Essa mensagem serve para a comunicação entre o microserviço 
	Lancamento.API com o microserviço Consolidado.API. O serviço Consolidado.API consome essa mensagem e insere os dados no banco de dados dbConsolidados que também é uma instância SqlServer e também 
	está no container Docker.

	- O serviços são completamente independentes, ou seja, se o Consolidados não estiver rodando, o usuário pode seguir com seus lançamentos normalmente, quando o serviço Consolidados voltar a ficar 
	disponível, ele consumirá a fila e atualizará os dados.

	- Se por alguma razão ocorrer algum erro e o saldo não ficar correto, o usuário poderá reprocessar os lançamentos para aquele dia, a api vai ajustar os dados do dia e recalcular o saldo daquele 
	dia para a frente.

# Desenho da solução:

	- Na pasta Lancamento consta a imagem "Solucao.png" mostrando como foi pensada essa solução.

# Instalação:

	- Clonar o repositório com o comando "git clone https://github.com/jeansantos77/Lancamento.git"
	- Entrar dentro do diretório lancamento via cmd e digitar "docker-compose up" para iniciar o container
	- Após o container estiver startado deve-se entrar no banco via Sql Management Studio e conectar no banco com usuário sa e senha "Senha@2023"
	- Clicar no menu na opção "Query", isso vai abrir o SqlQuery para que seja possível executar comandos
	- Copiar o script "InitialCreate.sql" contido em "Lancamento\Lancamento.API.Infra.Data\Migrations\Script"
	- Colar e executar na janela do SqlQuery, isso vai criar o banco de dados e as tabelas do banco dbLancamentos
	- Após essa execução pode-se entrar na pasta "Lancamento\Lancamento.API" via CMD e digitar "dotnet run"
	- Abrir o browse e digitar "https://localhost:5001/swagger" que a aplicação Lancamento.API estará rodando.



