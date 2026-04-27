# ?? Simulador Yachay-Tech - Sistema Interactivo de Decisiones

## ? Resumen

Sistema completo de simulador interactivo en **ASP.NET Core 8** listo para consumo desde **cualquier frontend** (React, Vue, Angular, etc.) con:
- ? 8 preguntas completamente configuradas
- ? 5 Endpoints REST funcionales
- ? Inmutabilidad de decisiones garantizada
- ? Puntaje acumulativo (máx 20 pts)
- ? Feedback detallado por opción
- ? Autenticación con correo institucional
- ? Logging integrado
- ? Manejo global de excepciones

---

## ?? Quick Start (5 min)

### 1. Ejecutar SQL
```bash
# SQL Server Management Studio
# Abre: docs/database/SQL.txt
# Ejecuta (Ctrl+A, Ctrl+E)
```

### 2. Compilar API
```bash
dotnet build
```

### 3. Ejecutar API
```bash
dotnet run
# API en: https://localhost:5001
# Swagger en: https://localhost:5001/swagger
```

### 4. Probar Frontend
Ver **Opción 3: JavaScript** en `docs/TESTING_GUIDE.md`

---

## ?? Endpoints Disponibles

### AUTENTICACIÓN

#### `POST /api/auth/register`
Registra nuevo usuario (correo institucional requerido)

```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Juan",
    "apellido": "Pérez",
    "correo": "i123456789@cibertec.edu.pe",
    "contrasena": "Contraseña123@",
    "dni": "12345678",
    "idRol": 1
  }' -k
```

#### `POST /api/auth/login`
Login con credenciales

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "correo": "i123456789@cibertec.edu.pe",
    "contrasena": "Contraseña123@"
  }' -k
```

### SIMULACIÓN

#### `POST /api/simulation/start`
Inicia sesión de simulación

#### `GET /api/simulation/status/{idSession}`
Obtiene estado actual

#### `GET /api/simulation/content/{idSession}/{idContent}`
Obtiene pregunta con opciones

#### `POST /api/simulation/decide`
Guarda decisión (INMUTABLE)

#### `GET /api/simulation/history/{idSession}`
Obtiene historial completo

---

## ?? Para Frontend (JavaScript/TypeScript)

### Instalación
```bash
npm install axios
```

### Uso Básico
```javascript
import SimuladorClient from './SimuladorClient.js';

// 1. Login
const login = await SimuladorClient.login(
  'i123456789@cibertec.edu.pe',
  'Contraseña123@'
);
const idUsuario = login.datos.idUsuario;

// 2. Iniciar simulación
const sesion = await SimuladorClient.iniciarSesion(idUsuario);
const idSession = sesion.datos.idSession;

// 3. Obtener pregunta
const contenido = await SimuladorClient.obtenerContenido(idSession, 1);
console.log(contenido.datos.pregunta);
console.log(contenido.datos.opciones); // Array de opciones

// 4. Responder pregunta
const decision = await SimuladorClient.guardarDecision(
  idSession,
  contenido.datos.idContent,
  contenido.datos.opciones[0].idOption // Usar idOption de la opción
);
console.log(decision.datos.puntajeObtenido);
console.log(decision.datos.puntajeTotalActualizado);

// 5. Ver historial
const historial = await SimuladorClient.obtenerHistorial(idSession);
console.log(historial.datos.decisionesPrevias);
```

---

## ?? Estructura de Datos

### Respuesta: Pregunta con Opciones
```json
{
  "exito": 1,
  "datos": {
    "idContent": 1,
    "fase": "fase_1",
    "titulo": "Análisis de Estímulos",
    "pregunta": "¿Qué instrumento aplicarías...",
    "opciones": [
      {
        "idOption": 1,
        "texto": "Opción A",
        "puntaje": 0.5,
        "nivel": "basic",
        "feedback": "Feedback detallado...",
        "resultado": "Resultado del análisis..."
      }
    ]
  }
}
```

### Respuesta: Feedback (Después de decidir)
```json
{
  "exito": 1,
  "datos": {
    "idDecision": 1,
    "puntajeObtenido": 2,
    "titulo": "Decisión estratégica superior",
    "texto": "Has seleccionado...",
    "resultado": "best",
    "puntajeTotalActualizado": 2,
    "puedeSiguiente": true
  }
}
```

---

## ?? Flujo de Simulación

```
Login ? Iniciar Sesión ? Pregunta 1 ? Responder ? Feedback
  ?
Pregunta 2 ? Responder ? Feedback
  ?
... (8 preguntas total)
  ?
Historial Final ? Puntaje Total (0-20)
```

---

## ?? Documentación Completa

| Archivo | Descripción |
|---------|------------|
| **TESTING_GUIDE.md** | Guía de testing (Postman, cURL, JS) |
| **SIMULADOR_API.md** | Referencia detallada de endpoints |
| **SimuladorClient.js** | Cliente JavaScript listo para usar |
| **POSTMAN_COLLECTION.json** | Colección para Postman |
| **database/SQL.txt** | Scripts de BD con 8 preguntas |

---

## ?? Datos de Prueba

**8 preguntas precargadas:**
- Fase 1: 3 preguntas de análisis (6 pts)
- Fase 2: 2 preguntas de Buyer Persona (5 pts)
- Fase 3: 3 preguntas de posicionamiento (9 pts)
- **Total máximo: 20 puntos**

**Roles disponibles:**
- Estudiante (id=1) - Responde simulador
- Docente (id=2) - Revisa desempeño
- Administrador (id=3) - Gestión total

---

## ? Verificación Final

- ? API compilada y corriendo
- ? BD con 8 preguntas + opciones
- ? 5 endpoints funcionales
- ? Inmutabilidad garantizada (UNIQUE en BD)
- ? Autenticación con validaciones
- ? Cliente JS actualizado
- ? Documentación completa
- ? Listo para conectar frontend

---

## ?? Próximos Pasos

1. **Ejecuta la API**: `dotnet run`
2. **Prueba en Swagger**: `https://localhost:5001/swagger`
3. **Conecta tu Frontend**: Importa `SimuladorClient.js`
4. **Renderiza las preguntas** según estructura de `opciones`
5. **Envía decisiones** con `idOption` (no índice de array)

---

## ?? Compatible Con

? React | Vue | Angular | Svelte | Next.js
? Mobile (React Native, Flutter, Ionic)
? Vanilla JavaScript/TypeScript

---

## ?? Características de Seguridad

- ? Contraseña hasheada (SHA256)
- ? Correo @cibertec.edu.pe requerido
- ? DNI único
- ? UNIQUE constraint en decisiones
- ? Validación multinivel
- ? Middleware de excepciones global

---

## ?? Ejemplo React

```javascript
import { useState, useEffect } from 'react';
import SimuladorClient from '@/services/SimuladorClient';

export default function SimuladorApp() {
  const [idSession, setIdSession] = useState(null);
  const [contenido, setContenido] = useState(null);
  const [puntaje, setPuntaje] = useState(0);

  useEffect(() => {
    iniciarSimulador();
  }, []);

  const iniciarSimulador = async () => {
    const sesion = await SimuladorClient.iniciarSesion(1);
    setIdSession(sesion.datos.idSession);
    cargarPregunta(1);
  };

  const cargarPregunta = async (idContent) => {
    const contenido = await SimuladorClient.obtenerContenido(
      idSession,
      idContent
    );
    setContenido(contenido.datos);
  };

  const responder = async (idOption) => {
    const decision = await SimuladorClient.guardarDecision(
      idSession,
      contenido.idContent,
      idOption
    );
    setPuntaje(decision.datos.puntajeTotalActualizado);
    // Mostrar feedback y cargar siguiente pregunta
  };

  return (
    <div>
      <h1>{contenido?.titulo}</h1>
      <p>{contenido?.pregunta}</p>
      <div>
        {contenido?.opciones?.map((opt) => (
          <button
            key={opt.idOption}
            onClick={() => responder(opt.idOption)}
          >
            {opt.texto} ({opt.puntaje} pts)
          </button>
        ))}
      </div>
      <p>Puntaje: {puntaje}</p>
    </div>
  );
}
```

---

## ? 100% LISTO PARA CONECTAR FRONTEND ??

**Archivos clave:**
- `SimuladorClient.js` - Cliente JavaScript
- `TESTING_GUIDE.md` - Ejemplos completos
- `database/SQL.txt` - BD con datos
- `appsettings.json` - Config de conexión

**Próximo paso:** Importa `SimuladorClient.js` en tu frontend y comienza a renderizar las preguntas! ??
