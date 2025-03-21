# **Projet : Gestion de Budget Personnel**

# Simplification du projet

Pour gagner du temps, on peut réduire la portée du projet tout en respectant les 4 verbes HTTP (GET, POST, PUT, DELETE) et les 12 points d’entrée demandés. Voici comment on peut simplifier :

## Ce qu’on garde :
✅ Authentification et gestion des utilisateurs avec JWT  
✅ Gestion des transactions (revenus & dépenses) (sans connexion à une API bancaire externe)  
✅ Gestion d’un budget simple (avec alerte si dépassement)  
✅ Tableau de bord basique (sans génération de PDF ni export Excel)  

## Les jours à mettre optionnel à rajouter si le temps nous le permet:
❌ Connexion à une API bancaire (Plaid, Tink, Yodlee) → Trop complexe et demande une API tierce  
❌ Chiffrement AES avancé → On sécurise juste les mots de passe avec hashing (BCrypt)  
❌ Notifications SMS/e-mails (Twilio, SendGrid) → On peut juste afficher une alerte sur le site  
❌ Export des données en PDF/Excel → On se concentre sur l'affichage des données sur l’interface


## **1. Description du projet**
Ce projet est une application de gestion de budget personnel permettant aux utilisateurs de suivre leurs revenus et dépenses, définir un budget mensuel et consulter un tableau de bord récapitulatif. L'application utilise **ASP.NET Core** pour le backend et **React** pour le frontend.

---

## **2. Fonctionnalités du projet**

### **Authentification et gestion des utilisateurs**
- Inscription et connexion avec **JWT**
- Gestion des rôles (**Utilisateur, Admin**)

### **Gestion des transactions (revenus & dépenses)**
- Affichage des transactions avec filtres
- Ajout, modification et suppression des transactions

### **Gestion des budgets**
- Définition d’un budget mensuel
- Alerte si le budget est dépassé

### **Tableau de bord**
- Visualisation du solde actuel
- Graphiques des revenus/dépenses par mois

---

## **3. Technologies utilisées**

### **Backend**
- **ASP.NET Core Web API**
- **Entity Framework Core + SQL Server (ou SQLite)**
- **Authentification JWT**

### **Frontend**
- **React.js**
- **Axios (pour les appels API)**
- **Recharts (pour les graphiques)**

---

## **4. Base de données (SQL Server ou SQLite)**

### **Table `Users`**  
| Id  | Nom          | Email          | Mot de passe (hashé) |
|----|-------------|---------------|---------------------|
| 1  | Jean Dupont | jean@gmail.com | Hashé               |

### **Table `Transactions`**  
| Id  | UserId | Montant | Type (Revenu/Dépense) | Catégorie   | Date       | Description         |
|----|--------|--------|--------------------|------------|-----------|------------------|
| 1  | 1      | -50€   | Dépense            | Alimentation | 2025-02-25 | Courses Carrefour |
| 2  | 1      | 2000€  | Revenu             | Salaire      | 2025-02-28 | Salaire mensuel   |

### **Table `Budgets`**  
| Id  | UserId | Montant max | Mois           | Statut |
|----|--------|------------|---------------|--------|
| 1  | 1      | 500€       | Février 2025 | Actif  |

---

## **5. API REST - Endpoints (12 points d’entrée)**

### **1. Authentification**
- `POST /api/auth/register` → Inscription
- `POST /api/auth/login` → Connexion

### **2. Transactions**
- `GET /api/transactions` → Liste des transactions
- `POST /api/transactions` → Ajouter une transaction
- `PUT /api/transactions/{id}` → Modifier une transaction
- `DELETE /api/transactions/{id}` → Supprimer une transaction

### **3. Budgets**
- `GET /api/budgets` → Voir le budget actuel
- `POST /api/budgets` → Définir un budget
- `PUT /api/budgets/{id}` → Modifier un budget
- `DELETE /api/budgets/{id}` → Supprimer un budget

### **4. Tableau de bord**
- `GET /api/dashboard` → Obtenir le résumé des finances (solde actuel, total dépenses, total revenus)

---

## **6. Architecture des dossiers**

### **Backend (ASP.NET Core)**
```
/Backend
│── /Controllers
│    │── AuthController.cs
│    │── TransactionsController.cs
│    │── BudgetsController.cs
│── /Models
│    │── User.cs
│    │── Transaction.cs
│    │── Budget.cs
│── /Data
│    │── ApplicationDbContext.cs
│── /Services
│    │── AuthService.cs
│── Program.cs
│── appsettings.json
```

### **Frontend (React)**
```
/Frontend
│── /src
│    │── /components
│    │    │── Login.js
│    │    │── Register.js
│    │    │── Dashboard.js
│    │    │── Transactions.js
│    │    │── Budget.js
│    │── /services
│    │    │── api.js (appel API)
│    │── App.js
│── package.json
│── index.html
```

---

## **7. Étapes de développement**

### **Phase 1 : Backend (ASP.NET Core)**
- [ ] Créer le projet **ASP.NET Core Web API**
- [ ] Configurer **Entity Framework Core et SQL Server/SQLite**
- [ ] Implémenter l’**authentification JWT**
- [ ] Développer les **endpoints API**

### **Phase 2 : Frontend (React)**
- [ ] Créer le projet **React**
- [ ] Développer les pages **Login, Dashboard, Transactions, Budget**
- [ ] Connecter le frontend avec l’API backend

---

## **8. Maquette (UI simplifiée)**

### **1. Page d’Accueil (Connexion)**
📌 **Formulaire de connexion** (email, mot de passe)  
📌 **Bouton "S'inscrire"**  

### **2. Tableau de Bord**
📌 **Solde actuel**  
📌 **Graphique des dépenses & revenus**  
📌 **Liste des dernières transactions**  

### **3. Page Transactions**
📌 **Liste des transactions** (filtrable)  
📌 **Formulaire "Ajouter une transaction"**  

### **4. Page Budget**
📌 **Budget mensuel actuel**  
📌 **Bouton pour modifier/supprimer le budget**  
📌 **Alerte si budget dépassé**  

---

## **9. Conclusion**
Ce projet propose une **gestion simple et efficace** des finances personnelles. Il est **rapide à développer** tout en respectant les **exigences du professeur** :
✅ **4 verbes HTTP** (GET, POST, PUT, DELETE)  
✅ **12 points d’entrée API**  
✅ **Interface simple et fonctionnelle avec React**  

🚀 **Prêt à coder ?** 😊

