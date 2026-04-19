# ?? Guía de Testing - Simulador

## ? Setup Rápido

### 1. Ejecutar SQL
```bash
# SQL Server Management Studio
# Abre: docs/database/SQL.txt
# Ejecuta todo (Ctrl+A, Ctrl+E)
```

### 2. Compilar
```bash
cd APISPROYECTOYATCHAY
dotnet build
```

### 3. Ejecutar API
```bash
dotnet run
# API en: https://localhost:5001
```

---

## ?? Opción 1: Postman

1. Abre Postman
2. Importa: `docs/POSTMAN_COLLECTION.json`
3. Ejecuta requests en orden:
   - POST /start
   - GET /status/1
   - GET /content/1/1
   - POST /decide
   - GET /history/1

---

## ??? Opción 2: cURL (Bash)

```bash
# 1. Iniciar sesión
curl -X POST https://localhost:5001/api/simulation/start \
  -H "Content-Type: application/json" \
  -d '{"idUsuario": 1}' \
  -k

# 2. Obtener contenido
curl -X GET https://localhost:5001/api/simulation/content/1/1 \
  -H "Content-Type: application/json" \
  -k

# 3. Guardar decisión
curl -X POST https://localhost:5001/api/simulation/decide \
  -H "Content-Type: application/json" \
  -d '{"idSession": 1, "idContent": 1, "opcionElegida": 1}' \
  -k

# 4. Intentar duplicado (debe fallar con 403)
curl -X POST https://localhost:5001/api/simulation/decide \
  -H "Content-Type: application/json" \
  -d '{"idSession": 1, "idContent": 1, "opcionElegida": 2}' \
  -k

# 5. Ver historial
curl -X GET https://localhost:5001/api/simulation/history/1 \
  -H "Content-Type: application/json" \
  -k
```

---

## ????? Opción 3: JavaScript/TypeScript

```javascript
import SimuladorClient from './docs/SimuladorClient.js';

async function test() {
  // 1. Iniciar
  const sesion = await SimuladorClient.iniciarSesion(1);
  console.log('Sesión:', sesion.datos.idSession);

  // 2. Obtener contenido
  const contenido = await SimuladorClient.obtenerContenido(
    sesion.datos.idSession,
    1
  );
  console.log('Pregunta:', contenido.datos.titulo);
  
  // Parsear opciones
  const opciones = JSON.parse(contenido.datos.opciones);
  console.log('Opciones:', opciones);

  // 3. Responder
  const feedback = await SimuladorClient.guardarDecision(
    sesion.datos.idSession,
    contenido.datos.idContent,
    opciones[0].id
  );
  console.log('Feedback:', feedback.datos);

  // 4. Ver historial
  const historial = await SimuladorClient.obtenerHistorial(
    sesion.datos.idSession
  );
  console.log('Decisiones:', historial.datos.decisionesPrevias);
}

test();
```

---

## ? Test Cases

### Test 1: Flujo Normal
```
1. POST /start
   ? Retorna idSession
   
2. GET /content/{id}/1
   ? Retorna pregunta con opciones JSON
   
3. POST /decide
   ? Retorna feedback con puntaje
   
4. GET /status/{id}
   ? Puntaje actualizado
```

### Test 2: Inmutabilidad
```
1. POST /decide (primera vez)
   ? 200 OK
   
2. POST /decide (misma pregunta)
   ? 403 Forbidden ? CORRECTO
   Mensaje: "Ya has respondido esta pregunta"
```

### Test 3: Múltiples Respuestas
```
1. Responder pregunta 1 (+10 pts)
2. Responder pregunta 2 (+15 pts)
3. GET /history/{id}
   ? 2 decisiones registradas
   ? Puntaje total = 25
```

### Test 4: Errores
```
? Datos inválidos ? 400 Bad Request
? Sesión no existe ? 404 Not Found
? Opción inválida ? 403 Forbidden
? Duplicado ? 403 Forbidden
```

---

## ?? Troubleshooting

| Problema | Solución |
|----------|----------|
| "Connection refused" | SQL Server no está corriendo |
| "Database already exists" | `DROP DATABASE SistemaQuizYatchay;` en SQL |
| 404 en endpoint | Verifica ruta: `/api/simulation/...` |
| 403 Forbidden | ? Correcto si es duplicado |
| Compilación falla | `dotnet clean && dotnet restore` |

---

## ?? Expected Responses

### Datos de Prueba en BD
```sql
-- Fase 1: Decisión Inicial
Opciones: [1: 10pts, 2: 5pts, 3: 0pts]

-- Fase 2: Decisión Estratégica  
Opciones: [1: 15pts, 2: 8pts]
```

---

## ? Validaciones

- ? DTOs validados (Required, RegularExpression)
- ? Sesión debe existir
- ? Contenido debe existir
- ? Opción debe ser válida
- ? No duplicar decisiones (UNIQUE en BD)

---

## ?? Verificación Final

- ? API responde en 5001
- ? Base de datos conectada
- ? Tablas creadas (3 nuevas)
- ? Índices optimizados
- ? Datos de prueba insertados
- ? Endpoints funcionan
- ? Inmutabilidad garantizada

**¡Listo para usar! ??**
