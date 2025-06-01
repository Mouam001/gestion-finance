# 📊 Projet de Gestion Financiere - API Interne & OBP

Ce projet est une plateforme de gestion de finances personnelles qui combine :

* ✅ **Une API interne avec gestion d'utilisateurs standards** (via JWT)
* 🔄 **Une API externe OBP (Open Bank Project)** avec des données bancaires simulées

## 🔧 Technologies utilisées

* .NET 8 / ASP.NET Core Web API
* Entity Framework Core + SQLite
* Blazor Server
* JWT (authentification)
* OBP Sandbox API
* Swagger UI

## 📆 Structure du projet

```
GestionAPI/
├── Common           # DTOs, enums, classes partagées
├── DataAccess       # DAO, Migrations, DbContext
├── Business         # Services métier (UserService, AuthService, etc.)
├── GestionWebAPP    # Application Blazor (UI)
├── GestionAPI       # API REST principale
```
## 🔐 API Interne - Utilisateur Standard

### Fonctionnalités

* Création de compte / Authentification
* Ajout / Modification / Suppression de transactions
* Visualisation des soldes, revenus, dépenses et graphiques
* Export PDF de relevé bancaire : User Standard

### Principales routes disponibles (via Swagger)

| Méthode | Endpoint                           | Description                           |
| ------- | ---------------------------------- | ------------------------------------- |
| POST    | `/api/auth/register`               | Inscription utilisateur               |
| POST    | `/api/auth/login`                  | Connexion utilisateur (JWT)           |
| GET     | `/api/transaction`                 | Récupère les transactions utilisateur |
| POST    | `/api/transaction`                 | Ajout d'une transaction               |
| PUT     | `/api/transaction/{transactionId}` | Mise à jour transaction               |
| DELETE  | `/api/transaction/{transactionId}` | Suppression d'une transaction         |
| GET     | `/api/User/me`                     | Infos utilisateur courant             |

## 🌐 API OBP - Open Bank Project

L'API OBP est une API bancaire ouverte de test (sandbox).

### Fonctionnalités disponibles

* Authentification DirectLogin OBP
* Récupération des comptes utilisateurs
* Visualisation de toutes les transactions bancaires
* Export PDF des relevés OBP

### Endpoints disponibles

| Méthode | Endpoint                                          | Description                |
| ------- | ------------------------------------------------- | -------------------------- |
| POST    | `/api/obp/loginOBP`                               | Connexion OBP              |
| GET     | `/api/obp/accounts`                               | Liste des comptes OBP      |
| GET     | `/api/obp/transactions/{bankId}/{accountId}`      | Transactions du compte OBP |
| GET     | `/api/obp/accounts/balances/{bankId}/{accountId}` | Solde du compte OBP        |

## 👥 Utilisateurs OBP de test

| Utilisateur   | Email                                                     | Mot de passe |
| ------------- | --------------------------------------------------------- | ------------ |
| Utilisateur 1 | [katja.fi.29@example.com](mailto:katja.fi.29@example.com) | ca0317       |
| Utilisateur 2 | [timo.fi.29@example.com](mailto:timo.fi.29@example.com)   | 6addcd       |
| Utilisateur 3 | [ellie.de.29@example.com](mailto:ellie.de.29@example.com) | 2efb1f       |

Ces comptes sont déjà présents dans le sandbox OBP.

## 🧠 Scénarios à tester

### Utilisateur Standard

1. Inscription via `/api/auth/register`
2. Connexion (JWT)
3. Ajout de transactions via `/api/transaction`
4. Accès à Blazor UI (solde, graphiques)
5. Export PDF du relevé

### Utilisateur OBP

1. Connexion à OBP via `/api/obp/loginOBP`
2. Choix du compte bancaire (ex: `gh.29.fi`)
3. Visualisation des transactions OBP
4. Export PDF des opérations bancaires

## ⚠️ Problèmes connus

| Problème                          | Détail                                                                                           |
| --------------------------------- | ------------------------------------------------------------------------------------------------ |
| 🔄 Rafraîchissement manuel requis | Après connexion OBP, l'affichage peut nécessiter un refresh manuel                               |
| 🔒 Déconnexion lente              | Le bouton logout peut ne pas déclencher la redirection immédiate                                 |
| 🔐 Accès interdit pour OBP        | Les routes `/api/transaction` et `/api/user` ne fonctionnent que pour les utilisateurs standards |



Contact : `mouammar8080ni@gmail.com` and `Celestin Goumou`


