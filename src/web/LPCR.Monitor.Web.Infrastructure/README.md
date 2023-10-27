# LPCR.Monitor.Web.Infrastructure

## Gestion de la base de données

### Création d'une migration

Lorsque le modèle de la base de données change, une migration doit être généré afin d'éffectuer le différentiel entre l'ancienne version de la base de données et la nouvelle.
Pour cela, appliquer la commande suivante dans la `Console du Gestionnaire de package` (`Package Manager Console`) intégrée à Visual Studio.

> Note : Remplacez `"NAME"` par le nom de votre migration afin de pouvoir l'identifiée.

```
PM> Add-Migration "NAME" -Context MonitoringDbContext -Project LPCR.Monitor.Web.Infrastructure -Startup LPCR.Monitor.Web
```

### Mettre à jour la base de données automatiquement

Une fois la migration créée, il faut l'appliquée à la base de donnéees de développement (présente sur votre poste).
Executez la commande suivante afin d'appliquer automatiquement les modifications apportées au modèle de données.

```
PM> Update-Database -Context MonitoringDbContext -Project LPCR.Monitor.Web.Infrastructure -Startup LPCR.Monitor.Web
```

### Générer le script de migration au format SQL

Lors du passage en recette ou production, il est nécessaire de générer les scripts SQL pour **chaque migration**.
Executez la commande suivante :

> Note : Remplacez `"PrécédenteMigration"` par le nom de la migration précédente et `"MigrationActuelle"` par le nom de la dernière migration effectuée.

```
PM> Script-Migration -From "PrécédenteMigration" -To "MigrationActuelle" -Context MonitoringDbContext -Project LPCR.Monitor.Web.Infrastructure -Startup LPCR.Monitor.Web
```

Une fois le script SQL généré, sauvegardez le dans le dossier : `Persistance/Migrations/SQL` du projet `LPCR.Monitor.Web.Infrastructure`.
