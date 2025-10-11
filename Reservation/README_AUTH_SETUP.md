# Authentication Setup Guide

## Regisztráció és Belépés Funkciók

### API Endpoints:

#### Regisztráció
```
POST /api/auth/register
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "password123",
  "role": 1  // 0=Admin, 1=Host, 2=Guest
}
```

#### Belépés
```
POST /api/auth/login
{
  "username": "testuser",  // vagy email
  "password": "password123"
}
```

#### Jelszó változtatás
```
POST /api/auth/change-password/{userId}
{
  "currentPassword": "oldpassword",
  "newPassword": "newpassword"
}
```

#### Token validáció
```
POST /api/auth/validate-token
"your-jwt-token-here"
```

#### Token frissítés
```
POST /api/auth/refresh-token
"your-jwt-token-here"
```

### Szerepkörök:
- **Admin (0)**: Teljes hozzáférés
- **Host (1)**: Ingatlanok kezelése
- **Guest (2)**: Foglalások, kommentek, like-ok

### Jelszó biztonság:
- BCrypt hash-elés
- Minimum 8 karakter
- Jelszó validáció

### JWT Token:
- 7 napos érvényesség
- User ID, Username, Email, Role információ
- Refresh token funkció


### Biztonsági megjegyzések:
- Jelszavak BCrypt-tel hash-eltek
- JWT tokenek biztonságos kulccsal aláírva
- Role-based authorization
- Token refresh funkció

### Következő lépések:
1. JWT middleware aktiválása
2. Authorization attribútumok hozzáadása
3. Role-based endpoint védelem
4. Frontend integráció
