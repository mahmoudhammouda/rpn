# Rpn API 
Reverse Polish Notation Calculator with net core 6 and angular


# Objectif :
* Réalisation d’une calculatrice RPN (notation polonaise inversée) en mode client/serveur

# Langages :
 Backend : API REST, C#, NET
 Frontend : Swagger

# Fonctionnalités demandées :
* Ajout d’un élément dans une pile
*  Récupération de la pile
* Nettoyage de la pile
* Opération +
* Opération -
* Opération *
* Opération /

# Livrables: (En cours)
* Code : Livraison du code sur un repo github à nous communiquer
* ToDo : Fichier « todo.md » listant les améliorations et lesraccourcis pris à cause du temps imparti.
* Roadmap : Fichier « roadmap.md » listant quelques fonctionnalités qui pourraient être
apportées au projet pour constituer une première backlog.

#  Le visual angular RPN (en cours)

![image](https://github.com/mahmoudhammouda/rpn/assets/50197626/a58ddfa3-c85d-42a9-a4a4-4087a5db8fad)

# Le swagger de l'api RPN

![image](https://github.com/mahmoudhammouda/rpn/assets/50197626/6a230fbf-28d5-4870-a729-f30d7f936acd)

# les tests unitaires

![image](https://github.com/mahmoudhammouda/rpn/assets/50197626/89cbe1db-22dd-4dd0-80ac-26d61c99c598)

#  Les tests d'acceptances

![image](https://github.com/mahmoudhammouda/rpn/assets/50197626/80b9a2d7-fff5-4e82-8659-c74fd25912e1)

# Progression :
Voici une description détaillée des étapes  suivies pour créer vcalculatrice :

* Développement d'une Librairie de Calculatrice :
Conception et tests d'une bibliothèque dédiée à la calculatrice dans un projet Visual Studio séparé.

* Introduction de la Facade IRpnCalculator :
Mise en place d'une façade IRpnCalculator pour minimiser le couplage avec les composants internes de la calculatrice.

* Implémentation du Pattern Stratégie avec IOperator :
Utilisation du pattern Stratégie via IOperator pour regrouper les algorithmes de calcul et faciliter l'ajout de nouvelles opérations.

* Adoption du Pattern Command :
Intégration du pattern Command pour gérer les exécutions immédiates et différées ainsi que l'annulation des opérations.

* Application du Pattern Factory pour IRpnStack :
Mise en œuvre du pattern Factory pour découpler la manipulation des IRpnStack de leur implémentation, permettant une structure basée sur des arbres.

* Tests Unitaires des Composants de la Calculatrice :
Réalisation de tests unitaires pour chaque composant de la calculatrice.

* Création d'une API :
Construction du squelette de l'API, en adoptant une architecture centrée sur un cœur fonctionnel adaptée à un projet de petite envergure.

* Intégration de la Logique de Calculatrice dans l'API :
Intégration directe du code de la calculatrice dans l'API pour simplifier le développement et réduire la complexité.

* Exposition des Endpoints de l'API et Création de Services :
Définition des endpoints de l'API et création de services sans logique métier directement dans les contrôleurs.

* Ajout de Swagger et Gestion Multi-Version :
Intégration de Swagger pour la documentation de l'API et mise en place de la gestion multi-version.

* Préparation à l'Externalisation du Cache et de l'Audit :
Configuration initiale pour externaliser le cache et l'audit en base de données, avec Dapper comme ORM.

* Tests d'Intégration via Projet d'Acceptance :
Réalisation de tests d'intégration depuis un projet d'acceptance externe, avec la possibilité d'utiliser Postman pour l'automatisation dans le processus CI/CD.

* Refactorisation et Nettoyage du Code :
Lancement de la phase de refactorisation et de nettoyage du code pour améliorer la maintenabilité et la clarté.

* Detection de problèmes et Hotfix
lancer des operations avec des valeurs limites et voir ce que cela donne. (division par zero au niveau calculator ok, et au niveau api ko => hotfix fait)


