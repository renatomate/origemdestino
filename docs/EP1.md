Renato Seiji Matsudo - 8516542

# Tecnologias utilizadas

Para o desenvolvimento deste projeto, foram utilizadas as seguintes tecnologias:

- Linguagem C# para mapeamento dos dados da pesquisa
- SQLite para persistência dos dados
- JavaScript/HTML5 para renderização do histograma

Em conjunto com essas tecnologias, foram utilizadas algumas bibliotecas disponíveis na Internet que serão especificadas à frente.

# Mapeamento do arquivo de dados

A primeira etapa do desenvolvimento do projeto foi estudar como processar dados DBF, formato utilizado pelo Metrô na divulgação da pesquisa. A idéia inicial era construir um algoritmo totalmente novo para processar esse formato, porém foi encontrada uma biblioteca *open source* para C# que já implementava essa função. A biblioteca chama-se **DbfDataReader** e está disponível no [GitHub](https://github.com/yellowfeather/DbfDataReader).

Para o volume de dados contido na pesquisa, a tarefa de leitura dos dados diretamente do arquivo DBF mostrou-se demorada. Para otimizar o processo de leitura, os dados desse arquivo foram lidos e salvos em um arquivo de banco de dados SQLite. O SQLite é uma biblioteca escrita em C de uma *engine* de banco de dados. O banco de dados é salvo em um arquivo com a extensão *.db*.

## Leitura dos dados

Para a leitura dos dados, primeiramente foram mapeadas as colunas que continham informações sobre os lugares frequentados. Foi criada também uma **lista** de objetos **Location**. Esse tipo de objeto contém as propriedades X, Y (representando as coordenadas) e uma lista linear de objetos **Frequenter**, que por sua vez contém apenas o ID do frequentador (um inteiro). Para a lista, foi utilizada a classe **List** da biblioteca padrão do C#.

Com esse mapeamento, iniciou-se uma leitura sequencial no arquivo DBF. Para cada registro do arquivo, foram obtidas as colunas previamente mapeadas. Caso a coordenada já tivesse sido adicionada na lista, um frequentador era adicionado à lista de frequentadores. Do contrário, era criado uma nova **Location** com o frequentador daquele registro.

## Apresentação dos dados

Para a exibição dos dados, foi utilizada a biblioteca *open source* **Chart.js**. A biblioteca está disponível no [GitHub](https://github.com/chartjs/Chart.js).

Foi construído um histograma, onde o eixo **x** é a quantidade de lugares e o **y** é o número de frequentadores.

O histograma é exibido em uma página web.

# Tempo de processamento

O processamento foi extremamente demorado, o processo todo descrito acima levou cerca de 2 horas para completar. Assintoticamente falando, o processamento dos dados leva um tempo O(n²), visto que o processamento foi totalmente sequencial e a cada novo registro iniciava-se uma nova busca sequencial para definir se o local já havia sido adicionado.

Uma possível solução para diminuir o tempo da busca seria utilizar uma tabela *hash*, assim a busca por um determinado elemento seria mais rápida, um tempo O(1).
