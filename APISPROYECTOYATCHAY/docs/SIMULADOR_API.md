# ?? API Simulador Yatchay - Referencia Rápida

## ?? Endpoints

### AUTENTICACIÓN

#### 1. Registro
```
POST /api/auth/register
Content-Type: application/json

{
  "nombre": "Juan",
  "apellido": "Pérez",
  "correo": "i123456789@cibertec.edu.pe",
  "contrasena": "Contraseña123@",
  "dni": "12345678",
  "idRol": 1
}

Response (200):
{
  "exito": 1,
  "mensaje": "Usuario registrado exitosamente"
}
```

#### 2. Login
```
POST /api/auth/login
Content-Type: application/json

{
  "correo": "i123456789@cibertec.edu.pe",
  "contrasena": "Contraseña123@"
}

Response (200):
{
  "exito": 1,
  "mensaje": "Login exitoso",
  "datos": {
    "idUsuario": 1,
    "nombre": "Juan",
    "idRol": 1,
    "nombreRol": "estudiante"
  }
}
```

---

### SIMULACIÓN

#### 3. Iniciar Sesión
```
POST /api/simulation/start
Content-Type: application/json

{
  "idUsuario": 1
}

Response (200):
{
  "exito": 1,
  "mensaje": "Sesión iniciada",
  "datos": {
    "idSession": 1,
    "idUsuario": 1,
    "faseActual": 1,
    "estado": "EN_PROGRESO",
    "puntajeTotal": 0,
    "inicialoAt": "2024-01-15T10:30:00"
  }
}
```

#### 4. Obtener Estado
```
GET /api/simulation/status/{idSession}

Response (200):
{
  "exito": 1,
  "mensaje": "Estado obtenido",
  "datos": {
    "idSession": 1,
    "idUsuario": 1,
    "faseActual": 1,
    "estado": "EN_PROGRESO",
    "puntajeTotal": 0
  }
}
```

#### 5. Obtener Contenido de Fase
```
GET /api/simulation/content/{idSession}/{fase}

Parámetros:
- idSession: int (ID de la sesión)
- fase: int (número de fase: 1, 2, 3...)

Response (200):
{
  "exito": 1,
  "mensaje": "Contenido obtenido",
  "datos": {
    "idContent": 1,
    "fase": 1,
    "tipo": "RIESGO",
    "titulo": "¿Qué haces ante un problema?",
    "opciones": "[{\"id\": 1, \"texto\": \"Ignorar\", \"puntaje\": 0}, {\"id\": 2, \"texto\": \"Analizar\", \"puntaje\": 10}, {\"id\": 3, \"texto\": \"Escalar\", \"puntaje\": 5}]",
    "feedback": "[{\"id\": 1, \"titulo\": \"Mala decisión\", \"texto\": \"Ignorar problemas no los resuelve\", \"resultado\": \"error\"}, ...]",
    "orden": 2
  }
}
```

#### 6. Guardar Decisión (INMUTABLE)
```
POST /api/simulation/decide
Content-Type: application/json

{
  "idSession": 1,
  "idContent": 1,
  "opcionElegida": 2
}

Response (200 OK):
{
  "exito": 1,
  "mensaje": "Decisión guardada",
  "datos": {
    "idDecision": 1,
    "puntajeObtenido": 10,
    "titulo": "Buena decisión",
    "texto": "Analizar es el primer paso correcto",
    "resultado": "éxito",
    "puntajeTotalActualizado": 10,
    "puedeSiguiente": true
  }
}

Response (403 Forbidden - Si ya existe):
{
  "exito": 0,
  "mensaje": "Ya has respondido esta pregunta. Las decisiones son inmutables."
}
```

#### 7. Obtener Historial
```
GET /api/simulation/history/{idSession}

Response (200):
{
  "exito": 1,
  "mensaje": "Historial obtenido",
  "datos": {
    "idSession": 1,
    "faseActual": 3,
    "estado": "EN_PROGRESO",
    "puntajeTotal": 30,
    "decisionesPrevias": [
      {
        "idDecision": 1,
        "fase": 1,
        "opcionElegida": 2,
        "puntajeObtenido": 10,
        "decididoAt": "2024-01-15T10:32:00"
      },
      {
        "idDecision": 2,
        "fase": 2,
        "opcionElegida": 1,
        "puntajeObtenido": 10,
        "decididoAt": "2024-01-15T10:35:00"
      },
      {
        "idDecision": 3,
        "fase": 3,
        "opcionElegida": 2,
        "puntajeObtenido": 10,
        "decididoAt": "2024-01-15T10:38:00"
      }
    ],
    "totalDecisiones": 3
  }
}
```

---

## ??? Arquitectura

### Capas
- **Controller** ? Recibe requests HTTP
- **Service** ? Lógica de negocio (validaciones, puntaje, inmutabilidad)
- **Repository** ? Acceso a datos (Dapper + SQL Server)
- **Middleware** ? Manejo global de excepciones

### Estructura de Features
```
Features/
??? Authentication/
?   ??? Controllers/ (AuthController)
?   ??? Services/ (en AuthController)
?   ??? Repositories/ (IUsuarioRepository)
?   ??? Models/ (Usuario)
?   ??? Dtos/ (LoginRequestDto, RegisterRequestDto, LoginResponseDto)
?
??? Simulation/
    ??? Controllers/ (SimulationController)
    ??? Services/ (ISimulationService, SimulationService)
    ??? Repositories/ (ISimulationRepositories, SimulationRepositories)
    ??? Models/ (SimulationContent, SimulationSession, Decision)
    ??? Dtos/ (SimulationDtos - 6 DTOs)

Common/
??? Exceptions/ (DecisionAlreadyMadeException)
??? Middleware/ (GlobalExceptionMiddleware)
```

### Patrones Aplicados
- ? Repository Pattern
- ? Service Pattern
- ? DTO Pattern
- ? Dependency Injection
- ? Feature-based Architecture

### Características Clave
- ? **Inmutabilidad**: Una decisión por pregunta (UNIQUE en BD)
- ? **Puntaje acumulativo**: Suma automática de puntos
- ? **Feedback inmediato**: Retorna feedback basado en opción elegida
- ? **Logging**: Trazabilidad completa de operaciones
- ? **Manejo de excepciones**: GlobalExceptionMiddleware captura todo
- ? **Validaciones multinivel**: Controller + Service + BD

---

## ??? Base de Datos

### Tablas Principales
- `Usuario` - Usuarios registrados
- `Rol` - Roles (estudiante, analista)
- `SimulationContent` - Preguntas/contenido (3 tipos: RIESGO, SEGURIDAD, GESTION)
- `SimulationSession` - Sesiones activas de usuarios
- `Decision` - Historial immutable de decisiones

### Constraints
- `UNIQUE(id_session, id_content)` - Previene decisiones duplicadas
- `FOREIGN KEY` - Integridad referencial completa

### Datos de Prueba Incluidos
- 3 preguntas (Fase 1-RIESGO, Fase 2-SEGURIDAD, Fase 3-GESTION)
- 3 opciones por pregunta con puntajes diferentes
- Feedback específico para cada opción

---

## ?? Códigos HTTP

| Código | Significado |
|--------|------------|
| `200` | OK - Operación exitosa |
| `400` | Bad Request - Datos inválidos |
| `403` | Forbidden - Decisión duplicada (inmutabilidad) |
| `404` | Not Found - Sesión/Contenido no encontrado |
| `500` | Internal Server Error - Error del servidor |

---

## ?? Testing

Ver: **TESTING_GUIDE.md**

Opciones para probar:
- Swagger (http://localhost:5001/swagger)
- Postman (POSTMAN_COLLECTION.json)
- cURL (TEST_ENDPOINTS.sh)
- JavaScript (SimuladorClient.js)

---

## ?? Cliente JavaScript

Ver: **SimuladorClient.js** para integración en frontend
