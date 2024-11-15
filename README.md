## Informações Projeto

### SQL / Docker
- Instalar Docker Descktop;
- Abrir PorwerShell em modo ADM;
- Executar o comando "docker pull mcr.microsoft.com/mssql/server" para baixar a image do SQL;
- Após finalizar, executar o comando "docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Str0ngPa$$w0rd' -p 1400:1433 -d mcr.microsoft.com/mssql/server" para criar um container aonde vai rodar o SQL;

### Api / Migrations
- Já deixei a criação inicial da base no arquivo de migrations;
- Apenas abrir o Package Manager Console, selecionar o projeto "Infrastructure" e rodar  o comando "Update-Database"

### Api / Docker
- O mesmo já está configurado no arquivo "launchSettings" para criar a aplicação e subir no Docker;
- OBS.: No final foi encontrado um problema, o projeto sobe no docker, porém da erro de conexão! Se isso acontecer favor rodar em "https" pois o tempo estava finalizando para entrega do desafio.

## Solicitações

### Fase 2: Refinamento
O que você perguntaria para o PO visando o refinamento para futuras implementações ou melhorias.
- Como será feita a autenticação e autorização dos usuários para acessar os recursos da API? 
- Quais são os campos obrigatórios e opcionais para os objetos "Projeto" e "Tarefa"? 
- Como será gerenciado o histórico de versões dos projetos e tarefas? 
- Quais tipos de notificações serão enviadas aos usuários? Por exemplo, notificações sobre novas tarefas, alterações de status...

### Fase 3: Final
O que você melhoraria no projeto, identificando possíveis pontos de melhoria, implementação de padrões, visão do projeto sobre arquitetura/cloud, etc.
- Utilizar serviços em nuvem para escalar a aplicação de forma mais fácil e eficiente.
- Reviar os princípios SOLID para criar um código mais modular, reutilizável e testável.
- Implementar um mecanismo de autenticação. (Talvez um api separada com foco em usuário, aonde apenas gera um token com as informações necessárias pra autenticar na Api proposta.
- Validar todos os dados de entrada para evitar ataques de injeção e outros tipos de vulnerabilidades.
- Implementar um mecanismo adequado para lidar com erros e exceções, evitando que informações sensíveis sejam expostas.


- [Regras do desafio](https://meteor-ocelot-f0d.notion.site/NET-C-5281edbec2e4480d98552e5ca0242c5b)


