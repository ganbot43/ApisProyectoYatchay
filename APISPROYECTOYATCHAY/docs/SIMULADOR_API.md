# ?? API Simulador - Referencia Rápida

## ?? Endpoints

### 1. Iniciar Sesión
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
    "idSession": 5,
    "idUsuario": 1,
    "faseActual": 1,
    "estado": "EN_PROGRESO",
    "puntajeTotal": 0
  }
}
```

### 2. Obtener Estado
```
GET /api/simulation/status/{idSession}

Response (200):
{
  "exito": 1,
  "mensaje": "Estado obtenido",
  "datos": {
    "idSession": 5,
    "faseActual": 1,
    "estado": "EN_PROGRESO",
    "puntajeTotal": 0
  }
}
```

### 3. Obtener Contenido Fase
```
GET /api/simulation/content/{idSession}/{fase}

Response (200):
{
  "exito": 1,
  "mensaje": "Contenido obtenido",
  "datos": {
    "idContent": 1,
    "fase": 1,
    "titulo": "Fase 1: Decisión Inicial",
    "opciones": "[{\"id\": 1, \"texto\": \"Opción A\", \"puntaje\": 10}, ...]",
    "feedback": "[{\"id\": 1, \"titulo\": \"Correcto\", \"texto\": \"Excelente\", \"resultado\": \"éxito\"}, ...]"
  }
}
```

### 4. Guardar Decisión (INMUTABLE)
```
POST /api/simulation/decide
Content-Type: application/json

{
  "idSession": 5,
  "idContent": 1,
  "opcionElegida": 1
}

Response (200):
{
  "exito": 1,
  "mensaje": "Decisión guardada",
  "datos": {
    "idDecision": 12,
    "puntajeObtenido": 10,
    "titulo": "Correcto",
    "texto": "Excelente decisión",
    "resultado": "éxito",
    "puntajeTotalActualizado": 10,
    "puedeSiguiente": true
  }
}

Response (403 Forbidden) - Si ya existe:
{
  "exito": 0,
  "mensaje": "Ya has respondido esta pregunta. Las decisiones son inmutables."
}
```

### 5. Obtener Historial
```
GET /api/simulation/history/{idSession}

Response (200):
{
  "exito": 1,
  "mensaje": "Historial obtenido",
  "datos": {
    "idSession": 5,
    "faseActual": 1,
    "puntajeTotal": 10,
    "decisionesPrevias": [
      {
        "idDecision": 12,
        "fase": 1,
        "opcionElegida": 1,
        "puntajeObtenido": 10,
        "decididoAt": "2024-01-15T10:32:00"
      }
    ],
    "totalDecisiones": 1
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

### Patrones
- Repository Pattern
- Service Pattern
- DTO Pattern
- Dependency Injection

### Características
- ? **Inmutabilidad**: Una decisión por pregunta (UNIQUE en BD)
- ? **Puntaje acumulativo**: Suma automática
- ? **Feedback inmediato**: Respuesta basada en opción
- ? **Logging**: Trazabilidad completa
- ? **Manejo de excepciones**: GlobalExceptionMiddleware

---

## ??? Base de Datos

### Tablas
- `SimulationContent` - Preguntas/fases
- `SimulationSession` - Sesiones de usuario
- `Decision` - Historial (UNIQUE por sesión+contenido)

### Constraints
- `UNIQUE(id_session, id_content)` - Previene duplicados
- `FOREIGN KEY` - Integridad referencial

---

## ?? Testing

Ver: **TESTING_GUIDE.md**

---

## ?? Cliente JavaScript

Ver: **SimuladorClient.js**
