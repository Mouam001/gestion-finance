# **Projet : Gestion de Budget Personnel**

## **1. Description du projet**  
Cette application permet aux utilisateurs de gérer leurs finances personnelles en suivant leurs revenus et dépenses. Chaque utilisateur dispose d’un montant initial de **80€** à l’ouverture de son compte.

### **Fonctionnalités principales :**  
✅ Authentification des utilisateurs avec **JWT**  
✅ Gestion des **transactions** (achats et revenus)  
✅ Suivi du **budget** (alerte si dépassement)  
✅ Page d’**achats** qui met à jour automatiquement le budget  
✅ **Tableau de bord** avec aperçu des finances  

---

## **2. Modèle de base de données (SQL Server ou SQLite)**  

### **Table Utilisateurs (`Users`)**  
| Id  | Nom   | Prénom  | Email         | Téléphone  | Adresse    | Montant_Init | Mot de passe (hashé) |
|-----|------|--------|--------------|------------|------------|--------------|------------------|
| 1   | Dupont | Jean  | jean@gmail.com | 0600000000 | Paris, France | 80€ | Hashé |

📌 **Règles :**  
- Lors de l'inscription, l’utilisateur commence avec **80€** de budget initial.  

---

### **Table Transactions (`Transactions`)**  
| Id  | UserId | Montant | Type (Revenu/Dépense) | Catégorie  | Date       | Description        |
|-----|--------|--------|-----------------|-----------|------------|------------------|
| 1   | 1      | -50€   | Dépense         | Alimentation | 2025-02-25 | Courses Carrefour |
| 2   | 1      | 2000€  | Revenu          | Salaire   | 2025-02-28 | Salaire mensuel  |

📌 **Règles :**  
- Une transaction peut être **un revenu ou une dépense**.  

---

### **Table Budgets (`Budgets`)**  
| Id  | UserId | Montant_max | Mois         | Statut |
|-----|--------|------------|--------------|--------|
| 1   | 1      | 500€       | Février 2025 | Actif |

📌 **Règles :**  
- Si le total des **dépenses > Montant_max**, une **alerte** est déclenchée.  

---

### **Table Achats (`Achats`)**  
| Id  | UserId | Nom_Produit | Montant | Date       |
|-----|--------|------------|---------|------------|
| 1   | 1      | PS5        | -450€   | 2025-03-01 |

📌 **Règles :**  
- Chaque achat est enregistré dans la table `Achats` et met à jour le budget de l'utilisateur.  

---

## **3. API REST - Endpoints (12 points d’entrée)**  

### **1. Authentification**  
- `POST /api/auth/register` → Inscription d'un utilisateur  
- `POST /api/auth/login` → Connexion et génération d’un token JWT  

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

### **4. Achats**  
- `POST /api/achats` → Ajouter un achat et mettre à jour le budget  

### **5. Tableau de bord**  
- `GET /api/dashboard` → Obtenir un récapitulatif des finances  

---

## **4. Étapes de développement**  

### **Phase 1 : Backend (ASP.NET Core)**  
- [ ] Créer le projet **ASP.NET Core Web API**  
- [ ] Configurer **Entity Framework Core + SQL Server**  
- [ ] Implémenter **l’authentification JWT**  
- [ ] Développer les **endpoints API**  

### **Phase 2 : Frontend (React)**  
- [ ] Créer le projet **React**  
- [ ] Développer les pages **Login, Dashboard, Transactions, Budget, Achats**  
- [ ] Connecter le frontend avec l’API backend  

---

## **5. Conclusion**  
Ce projet couvre toutes les exigences académiques :  
✅ **4 verbes HTTP** (GET, POST, PUT, DELETE)  
✅ **12 endpoints API**  
✅ **Gestion du budget + transactions + achats**  
✅ **Séparation propre des responsabilités (DAO, DTO, Business, API)**  

Le projet est **réaliste et réalisable dans le temps imparti**, avec une option d'ajout de fonctionnalités avancées si le temps le permet.  

💡 **Prochaine étape : Commencer par l’authentification JWT et les DAO pour les transactions.**

- Tokent github Mouammar :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

