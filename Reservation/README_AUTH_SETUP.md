# Authentication Setup Guide

## Regisztráció és Belépés Funkciók

### Létrehozott komponensek:

1. **AuthDto.cs** - Authentication DTO-k
2. **IAuthService.cs** - Auth service interfész
3. **AuthService.cs** - Auth service implementáció
4. **AuthController.cs** - Auth controller

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

### Beállítás lépések:

1. **NuGet csomagok telepítése:**
   ```bash
   dotnet restore
   ```

2. **JWT konfiguráció engedélyezése:**
   - Program.cs-ben uncommenteld a JWT beállításokat
   - appsettings.json-ben már be van állítva a JWT konfiguráció

3. **Adatbázis migráció:**
   ```bash
   dotnet ef migrations add AddAuth
   dotnet ef database update
   ```

4. **Tesztelés:**
   - Swagger UI: https://localhost:7102/swagger
   - Auth endpoints: /api/auth/*

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
