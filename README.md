# Desafio Taskrow - Por jpemendonca

Fiz o desafio utilizando o .net 9. Utilizei Entity Framework, incluindo na parte 3 onde foi solicitado as queries, fiz usando queries com Entity e Linq.
O banco de dados para a parte 2 e 3 foi um SqlServer criado em um conteiner Docker na minha máquina.

## Como testar
### Parte 1
A parte 1 basta abrir no diretório correto e usar. Para esse parte ficou faltando a função de fazer JOIN nas tabelas enviadas

   ```bash
   dotnet run
   ```

### Parte 2
Abra a solution que contem as 3 partes. As apis estão documentadas com uso da lib **scalar**, não do swagger.
Todas as apis foram feitas como solicitado, e as tabelas/entidades modelei usando o Entity Framework com migrations

![image](https://github.com/user-attachments/assets/8d21f5bd-2d6a-4ac3-b2c7-22d376880bbc)

### Parte 3
As funcoes que fazem query estão nas pastas correspondentes. Para testar essas funcoes, se quiser, dentro da Parte 2 tem duas APIs GET
/TesteParte3

Ficou faltando nessa parte a terceira query, fiz as duas primeiras solicitadas.
