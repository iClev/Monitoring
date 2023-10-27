# Project Monitor

[![Build](https://github.com/devlpcr/Monitor/actions/workflows/build.yml/badge.svg)](https://github.com/devlpcr/Monitor/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=devlpcr_Monitor&metric=alert_status&token=30de0e767462ad3d303897663d1c4c570b277c66)](https://sonarcloud.io/summary/new_code?id=devlpcr_Monitor)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=devlpcr_Monitor&metric=vulnerabilities&token=30de0e767462ad3d303897663d1c4c570b277c66)](https://sonarcloud.io/summary/new_code?id=devlpcr_Monitor)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=devlpcr_Monitor&metric=security_rating&token=30de0e767462ad3d303897663d1c4c570b277c66)](https://sonarcloud.io/summary/new_code?id=devlpcr_Monitor)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=devlpcr_Monitor&metric=sqale_rating&token=30de0e767462ad3d303897663d1c4c570b277c66)](https://sonarcloud.io/summary/new_code?id=devlpcr_Monitor)

Monitor est une application permettant d'orchestrer, planifier et monitorer les flux métier du SI LPCR.

> Un flux métier est par exemple, un flux de synchronisation entre l'Intranet et Sales Force (montée ou descente d'informations).

## Prérequis

* [Visual Studio 2022](https://visualstudio.microsoft.com/fr/)
* [SQL Server (Developer Edition)](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads)
* [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

## Mise en place de l'environnement de développement

Dans un terminal Windows ou PowerShell, entrez les commandes suivantes:

1. Clonez le dépôt

```
$ git clone https://github.com/devlpcr/Monitor.git
```

2. Restaurez les dépendances des projets

```
$ cd Monitor
$ dotnet restore
$ dotnet build
```

3. Installez l'outils EF Core tools

> Cette étape est nécessaire pour créer la base de données.

```
$ dotnet tool install --global dotnet-ef
```

Si l'outil est déjà installé, mettez le à jour avec la commande suivante:
```
$ dotnet tool update --global dotnet-ef
```

4. Créez la base de données locale

> La commande suivante, va créer une nouvelle base de donnée nommée `MonitorDev` sur l'instance `(localhost)` de votre SQL Server local.

```
$ dotnet ef database update -p .\src\web\LPCR.Monitor.Web.Infrastructure\LPCR.Monitor.Web.Infrastructure.csproj -s .\src\web\LPCR.Monitor.Web\LPCR.Monitor.Web.csproj
```

5. Lancez la solution

Double cliquez sur le fichier `LPCR.Monitor.sln` pour ouvrir la solution avec Visual Studio.
Une fois Visual Studio ouvert, faites un clique doit sur le projet `LPCR.Monitor.Web` > `Définir comme projet de démarrage`. Puis démarrez le projet en debug.

L'interface web devrait se présenter à vous.

## Fonctionnement

L'application est composées des briques suivantes :
* Une interface de monitoring
* Un "runner" permettant d'exécuter les jobs
* Une API appelée par le runner pour la mise à jour des jobs et envoie de traces

![image](https://user-images.githubusercontent.com/98390902/207108707-d060fca0-c74b-4f54-8cf9-e68144d840de.png)

### Runner

Le runner est le programme responsable de l'orchestration et de l'exécution de chaque jobs. Lors de son démarrage, ce dernier va récupérer depuis l'API, l'ensemble des jobs et leur définition de planification et effectuer leur planification au sein du système (via la librairie [Quartz.NET](https://github.com/quartznet/quartznet)).

Egalement pendant le démarrage, le runner va charger dynamiquement les "Job Processors" depuis le dossier `jobs` situé à la racine du répertoire ou se situe l'exécutable.

### Job Processor

Chaque Job Processor est en fait représenté par une `.dll` contenant le code métier à exécuter par le runner. 

La propriété `ProcessorName` de la table `Jobs` est le nom **complet** du Job au sens technique. Par exemple, si un projet s'appelle : `LPCR.EspaceFamille.SynchronisationContrats` et que le job s'appelle : `SynchronizeContractsJob`, le nom du processor sera : `LPCR.EspaceFamille.SynchronisationContrats.SynchronizeContractsJob`.

## Environnements

### Recette

*Environnement non provisioné.*

### Production

*Environnement non provisioné.*
