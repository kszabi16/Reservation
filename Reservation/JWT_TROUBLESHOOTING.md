# JWT Token Signature Verification Failed - Megoldás

## Probléma
A JWT token decoder "signature verification failed" hibát ad.

## Megoldások

### 1. NuGet csomagok telepítése
```bash
dotnet restore
```

### 2. JWT konfiguráció ellenőrzése
Az `appsettings.json` fájlban:
```json
{
  "Jwt": {
    "Key": "MySuperSecretKeyForJWTTokenGenerationThatIsAtLeast32CharactersLong123456789",
    "Issuer": "ReservationApp",
    "Audience": "ReservationAppUsers"
  }
}
```

### 3. Teszt token generálása
Használd a teszt endpoint-ot:
```
GET /api/auth/test-token
```

Ez egy érvényes JWT tokent generál, amit tesztelhetsz.

### 4. JWT token dekódolás ellenőrzése

#### JWT.io használata:
1. Menj a https://jwt.io oldalra
2. Illeszd be a tokent a "Encoded" mezőbe
3. A "Verify Signature" részben:
   - **Algorithm**: HS256
   - **Secret**: `MySuperSecretKeyForJWTTokenGenerationThatIsAtLeast32CharactersLong123456789`

#### Token struktúra:
```json
{
  "sub": "999",
  "name": "testuser",
  "email": "test@example.com",
  "role": "Guest",
  "iss": "ReservationApp",
  "aud": "ReservationAppUsers",
  "exp": 1234567890,
  "iat": 1234567890
}
```

### 5. Gyakori hibák és megoldások

#### Hiba: "Invalid signature"
- **Ok**: A secret kulcs nem egyezik
- **Megoldás**: Ellenőrizd, hogy ugyanazt a kulcsot használod mind a generálásnál, mind a validációnál

#### Hiba: "Token expired"
- **Ok**: A token lejárt
- **Megoldás**: Generálj új tokent vagy használd a refresh token funkciót

#### Hiba: "Invalid issuer"
- **Ok**: Az issuer nem egyezik
- **Megoldás**: Ellenőrizd az `appsettings.json`-ban az "Issuer" értéket

#### Hiba: "Invalid audience"
- **Ok**: Az audience nem egyezik
- **Megoldás**: Ellenőrizd az `appsettings.json`-ban az "Audience" értéket

### 6. API tesztelés

#### Regisztráció:
```bash
POST /api/auth/register
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "password123",
  "role": 2
}
```

#### Belépés:
```bash
POST /api/auth/login
{
  "username": "testuser",
  "password": "password123"
}
```

#### Token validáció:
```bash
POST /api/auth/validate-token
"your-jwt-token-here"
```

### 7. Debug lépések

1. **Ellenőrizd a konzol kimenetet** - vannak-e hibák?
2. **Teszteld a teszt endpoint-ot** - `/api/auth/test-token`
3. **Ellenőrizd a JWT.io-n** - a token dekódolása
4. **Nézd meg a token tartalmát** - exp, iss, aud mezők

### 8. Biztonsági megjegyzések

- **Ne használd éles környezetben** a teszt kulcsot
- **Generálj erős, véletlenszerű kulcsot** éles környezethez
- **Használj HTTPS-t** minden API híváshoz
- **Token lejárati idő** beállítása (jelenleg 7 nap)

### 9. JWT token használata

A token használata API hívásoknál:
```
Authorization: Bearer your-jwt-token-here
```

### 10. Ha még mindig nem működik

1. **Restart** az alkalmazást
2. **Clear** a böngésző cache-t
3. **Ellenőrizd** a konzol hibákat
4. **Teszteld** a teszt endpoint-ot



