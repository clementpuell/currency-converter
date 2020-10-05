## Currency Converter

La solution consiste en une application console qui va successivement :
1. Lire les données du fichier fourni en entrée.
2. Construire à partir des taux de change un graphe non orienté. Les `Node` (noeuds) de ce graph représente des devises participant à un taux de change et les `Edge` (arêtes) des taux de change entre deux devises.
3. Appliquer sur ce graphe l'algorithme `Dijkstra` de recherche du plus court chemin entre les deux devises demandées dans le fichier d'entrée.
4. Vérifier que le chemin trouvé, `Path`, est valide, c'est-à-dire que tout ses noeuds sont connectés de la devise de départ à la devise d'arrivée.
5. Appliquer les taux de change des arêtes qui composent ce chemin sur le montant donné dans le fichier d'entrée. Le résultat est le montant exprimé dans la devise demandée.

Les points d'intérêts sont :
- Les `Edge` peuvent être "retournés" (`Reversed`) afin d'être orienté vers l'une ou l'autre de ses deux devises. Faire cela inverse le taux de change. J'oriente les arêtes au moment de construire le chemin, à la fin de l'algorithme Dijkstra.

- Le `Graph` stocke les noeuds et les arêtes dans des `HashSet`. Cela permet de dédoublonner automatiquement les différentes apparitions d'une devise dans le fichier d'entrée, ainsi que les taux de change qui serait exprimés dans les deux sens :  
    `EUR;USD;10`  
    `USD;EUR;0.1`  
    *(Ces données seront représentées par 2 nodes et 1 edge.)*

- L'implémentation de l'algorithme `Dijkstra` utilise trois dictionaires pour stocker les informations de distances, de noeuds visités et de noeuds précédents. Cela permet des accès rapide en O(1). Les noeuds stockés dans ces dictionaires sont des références vers les noeuds du graphe.  
A noter que le résultat de la recherche de chemin est exprimé sous forme d'une suite d'arêtes et non de noeuds.

- Il est possible que le chemin trouvé soit incomplet (par exemple, si la devise cible n'est pas connectée à la devise source). Dans ce cas, l'objet `Path` le détecte et une exception est levée.

Pour exécuter le programme : `dotnet run --project src src\file1.csv`  
Pour exécuter les tests : `dotnet test`
