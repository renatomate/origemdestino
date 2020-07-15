# Como foi obtido

Para tornar o processamento mais rápido, os nós obtidos através do arquivo de texto foram convertidos para uma _Hash Table_. Para a busca em profundidade, esses nós foram alocados numa variável para controlar os nós ainda não visitados.

As componentes nada mais são do que uma lista de sub-grafos, portanto foi criada uma lista de objetos do tipo **Graph** para guardá-las.

Foi criado então um laço para ser executado enquanto existirem nós ainda não visitados. Esse laço chama a função **DeepFirstSearch**, passando o objeto do tipo **Node** e a tabela _hash_ de nós ainda não visitados.

```C#
public List<Graph> GetComponents()
{
    // nós que ainda não foram visitados
    var unvisitedNodes = Nodes.ToHashSet();

    // as componentes são uma lista de grafos
    var components = new List<Graph>();

    // enquanto existirem nós não visitados, executa a função de busca para encontrar novas componentes
    while (unvisitedNodes.Any())
    {
        components.Add(new Graph
        {
            Nodes = DeepFirstSearch(unvisitedNodes.First(), unvisitedNodes);
        });

    // retorna as componentes encontradas
    return components;
}
```

A função **DeepFirstSearch** primeiramente remove o nó que está sendo visitado da tabela de nós que não foram visitados. Ela então cria uma lista de nós encontrados a partir desse nó, adicionando ele mesmo nessa lista. Para cada vizinho desse nó que ainda não foi visitado é executado o método **DeepFirstSearch** sobre ele.

```C#
private List<Node> DeepFirstSearch(Node node, HashSet<Node> unvisitedNodes)
{
    // remove o nó que está sendo visitado da lista de nós não visitados
    unvisitedNodes.Remove(node);

    // cria a lista de nós encontrados, adicionando ele próprio a ela
    var foundNodes = new List<Node> { node };

    // percorre todos os vizinhos que ainda não foram visitados e executa a busca sobre ele
    foreach (var neighboor in node.Neighboors.Where(n => unvisitedNodes.Contains(n)))
    {
        foundNodes.AddRange(DeepFirstSearch(neighboor, unvisitedNodes));
    }

    // retorna os nós encontrados
    return foundNodes;
}
```

O resultado da função **GetComponents()** da classe **Graph** é uma lista de objetos do tipo **Graph**. Para obter o resultado dessa tarefa, bastou agrupar esses grafos com base na contagem de seus nós, que é feita em uma página HTML:

```C#
Model.GroupBy(m => m.Nodes.Count()).OrderBy(m => m.Key)
```

A componente gigante possui 52.436 nós.

# Cenário utilizado

O cenário utilizado foi aquele onde todos os lugares estão abertos. Esse cenário é o que provavelmente possuir a maior componente gigante.

# Tabela de componentes

| Tamanho da Componente | Quantidade |
| --------------------- | ---------- |
| 2                     | 1895       |
| 3                     | 763        |
| 4                     | 325        |
| 5                     | 123        |
| 6                     | 58         |
| 7                     | 33         |
| 8                     | 29         |
| 9                     | 18         |
| 10                    | 7          |
| 11                    | 9          |
| 12                    | 5          |
| 13                    | 5          |
| 14                    | 2          |
| 15                    | 1          |
| 22                    | 2          |
| 27                    | 2          |
| 52436                 | 1          |
