TODO: Rest a faire sur le cours terme :

# Paquetiser la Calculatrice en un NuGet Partageable :
La calculatrice, initialement développée comme une bibliothèque pour une application console, devrait être transformée en un package NuGet pour faciliter sa réutilisation dans différents types d'hôtes (API, WCF, console, etc.). Créer un package NuGet testable et injectable, incluant le code et les tests, est une pratique recommandée.

# Intégration d'ILogger :
Incorporer ILogger dans le projet et créer un adaptateur pour s'adapter à une implémentation existante telle que Log4Net.

# Gestion de la Configuration :
* Adapter la configuration en fonction de l'environnement en procédant comme suit :
* Ajouter un répertoire config contenant quatre sous-répertoires (Dev, Rec, Med, Prd), chacun avec son propre appsettings_Env.json correspondant à son environnement.
Rendre les paramètres de configuration modifiables, avec des valeurs ajoutées par les agents de déploiement.

# Configuration des Mappings AutoMapper :
Pour chaque DTO, configurer un mapping correspondant vers le modèle associé. Assurer que les règles de validation des modèles sont respectées, car AutoMapper peut contourner la validation standard.

# Ajout de la Fonctionnalité Undo pour les Opérandes
Actuellement, la fonctionnalité d'annulation (Undo) est implémentée uniquement pour les opérateurs. Il est nécessaire d'étendre cette fonctionnalité pour permettre également l'annulation des opérandes.

# Initialisation d'un Calculateur en Mode Différé
Introduire la possibilité d'initialiser le calculateur en mode différé. Cette option sera particulièrement utile pour l'interprétation d'expressions complexes ou pour des calculs nécessitant une évaluation ultérieure.

# Rendre RpnCalculator Indépendant de la Mise en Cache :
Implémenter IStackCache et IStackHistoryCache pour encapsuler les interactions avec le cache et ainsi rendre RpnCalculator indépendant de toute logique de mise en cache spécifique.

![image](https://github.com/mahmoudhammouda/rpn/assets/50197626/dbf4678e-8f29-447b-bec6-c07d06ecaad4)

# Rendre la Calculatrice Thread-Safe :
Le RpnCalculator utilise une instance singleton pour gérer l'état des différentes piles. Pour éviter la corruption des données dans un environnement multithread, il est nécessaire de :
* Implémenter une logique de synchronisation pour isoler les accès concurrents à une même pile.
* Remplacer Dictionary par ConcurrentDictionary, qui est thread-safe.
* Utiliser les méthodes TryGetValue, TryAdd, et TryRemove.
* Tester la concurrence avec des outils tels que JMeter pour s'assurer que l'état reste cohérent

# Réduire la Visibilité de l'État Partagé :
Isoler les données entre les sessions utilisateurs pour que les données de l'utilisateur A ne soient pas visibles par l'utilisateur B. Cela nécessite :
* Associer chaque état de pile à un utilisateur spécifique via un identifiant de session ou un JWT.
* Intégrer l'identification de l'utilisateur dans la structure de persistance de données dans le cache.
* Example creation d'une stack :

![image](https://github.com/mahmoudhammouda/rpn/assets/50197626/7b538333-7e5a-4665-9473-c6dec2f1bb8b)

# Optimisation de la Mémoire et Surveillance du Cache :
Implémenter une politique de nettoyage automatisé pour les utilisateurs n'utilisant plus la calculatrice :
* Définir un TTL après lequel les données de la pile sont effacées.
* Proposer un recyclage en fin de journée pour redémarrer le service ou mettre en place un service en arrière-plan pour nettoyer le cache.

# Transition vers un Cache Externe :
Utiliser un cache externe pour améliorer la tolérance aux pannes et rester sans état (stateless) :
* Séparer la persistance des données du service de calcul en injectant RpnCalculator en tant que service avec une portée limitée (Scoped).
* Utiliser un cache externe comme Redis.

# Finaliser la Partie Angular :
* Créer operator.service.ts et stack.service.ts.
* Ajouter les interfaces des modèles.
* Implémenter les méthodes de récupération de données en utilisant Observable plutôt que Promise.
* Sauvegarder les données dans un cache qui persiste au-delà de la durée de vie du composant en utilisant un service de gestion du cache, localStorage, sessionStorage, ou NgRx.

