# ?? Guía de Testing - Simulador Yachay-Tech

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
# Swagger en: https://localhost:5001/swagger
```

---

## ?? Opción 1: Postman

1. Abre Postman
2. Importa: `docs/POSTMAN_COLLECTION.json`
3. Ejecuta requests en orden

---

## ??? Opción 2: cURL (Bash/PowerShell)

### Registrarse
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Juan",
    "apellido": "Perez",
    "correo": "i123456789@cibertec.edu.pe",
    "contrasena": "Contraseña123@",
    "dni": "12345678",
    "idRol": 1
  }' -k
```

### Login
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "correo": "i123456789@cibertec.edu.pe",
    "contrasena": "Contraseña123@"
  }' -k
```

### Iniciar Simulación
```bash
curl -X POST https://localhost:5001/api/simulation/start \
  -H "Content-Type: application/json" \
  -d '{"idUsuario": 1}' -k
```

### Obtener Contenido (Pregunta)
```bash
curl -X GET "https://localhost:5001/api/simulation/content/1/1" \
  -H "Content-Type: application/json" -k
```

### Guardar Decisión
```bash
curl -X POST https://localhost:5001/api/simulation/decide \
  -H "Content-Type: application/json" \
  -d '{
    "idSession": 1,
    "idContent": 1,
    "idOption": 1
  }' -k
```

### Obtener Historial
```bash
curl -X GET "https://localhost:5001/api/simulation/history/1" \
  -H "Content-Type: application/json" -k
```

---

## ?? Opción 3: JavaScript (Frontend)

### Instalar Axios
```bash
npm install axios
```

### Usar SimuladorClient
```javascript
import SimuladorClient from './docs/SimuladorClient.js';

// 1. Registrar
const registroResponse = await SimuladorClient.registrar(
  'Juan', 'Pérez', 'i123456789@cibertec.edu.pe',
  'Contraseña123@', '12345678', 1
);

// 2. Login
const loginResponse = await SimuladorClient.login(
  'i123456789@cibertec.edu.pe',
  'Contraseña123@'
);
const idUsuario = loginResponse.datos.idUsuario;

// 3. Iniciar simulación
const sesionResponse = await SimuladorClient.iniciarSesion(idUsuario);
const idSession = sesionResponse.datos.idSession;

// 4. Obtener pregunta
const contenidoResponse = await SimuladorClient.obtenerContenido(idSession, 1);
const contenido = contenidoResponse.datos;

// 5. Responder (usar idOption del objeto opción)
const decisionResponse = await SimuladorClient.guardarDecision(
  idSession,
  contenido.idContent,
  contenido.opciones[0].idOption  // <-- Usar idOption, no índice
);

// 6. Ver historial
const historialResponse = await SimuladorClient.obtenerHistorial(idSession);
```

---

## ?? Estructura de Respuesta: Pregunta

```json
{
  "exito": 1,
  "mensaje": "Contenido obtenido",
  "datos": {
    "idContent": 1,
    "idContentExterno": "fase1_estimulos",
    "fase": "fase_1",
    "categoria": "analisis_estimulos",
    "tipo": "single_choice",
    "titulo": "Análisis de Estímulos",
    "intro": "Has decidido iniciar con un enfoque sensorial...",
    "pregunta": "¿Qué instrumento aplicarías para identificar los stoppers...",
    "orden": 1,
    "opciones": [
      {
        "idOption": 1,
        "idOptionExterno": "auditoria_contenido_multimedia_ux",
        "texto": "Auditoría de Contenido Multimedia y UX",
        "puntaje": 0.5,
        "nivel": "basic",
        "feedback": "¡Excelente elección! Estás aplicando...",
        "resultado": "Existe falla sensorial...",
        "insight": "El entorno digital no está estimulando...",
        "siguientePreguntaId": null
      },
      {
        "idOption": 2,
        "idOptionExterno": "test_producto_pop_up_stores",
        "texto": "Test de Producto en Puntos de Venta",
        "puntaje": 1,
        "nivel": "medium",
        "feedback": "¡Muy bien pensado!...",
        "resultado": "El 85% de los padres...",
        "insight": "El contacto físico rompe...",
        "siguientePreguntaId": null
      },
      {
        "idOption": 3,
        "idOptionExterno": "prueba_rendimiento_yachay_hub",
        "texto": "Prueba de Rendimiento Crítico de Yachay-Hub",
        "puntaje": 2,
        "nivel": "best",
        "feedback": "¡Decisión estratégica superior!...",
        "resultado": "Al ver la tablet caer...",
        "insight": "La motivación de seguridad...",
        "siguientePreguntaId": null
      }
    ]
  }
}
```

---

## ?? Estructura de Respuesta: Feedback (Decisión Guardada)

```json
{
  "exito": 1,
  "mensaje": "Decisión guardada",
  "datos": {
    "idDecision": 1,
    "puntajeObtenido": 2,
    "titulo": "¡Decisión estratégica superior!",
    "texto": "Has seleccionado una herramienta que analiza profundamente...",
    "resultado": "best",
    "puntajeTotalActualizado": 2,
    "puedeSiguiente": true
  }
}
```

---

## ? Test Cases

### Test 1: Flujo Completo
```
1. POST /auth/register ?
2. POST /auth/login ?
3. POST /simulation/start ?
4. GET /simulation/content/1/1 ?
5. POST /simulation/decide ?
6. GET /simulation/history/1 ?
```

### Test 2: Inmutabilidad (403)
```
1. POST /simulation/decide (primera vez) ? 200 OK ?
2. POST /simulation/decide (misma pregunta) ? 403 Forbidden ?
```

### Test 3: Validaciones
```
- Correo inválido ? 400 Bad Request
- Contraseña débil ? 400 Bad Request
- DNI inválido ? 400 Bad Request
- Sesión no existe ? 404 Not Found
- Opción inválida ? 400 Bad Request
```

---

## ?? Para Frontend (React/Vue/Angular)

### Instalación
```bash
npm install axios
```

### Uso en componente
```javascript
import SimuladorClient from '@/services/SimuladorClient';

export default {
  data() {
    return {
      idSession: null,
      contenido: null,
      opciones: [],
      historial: [],
      puntajeTotal: 0
    };
  },
  methods: {
    async cargarPregunta(idContent) {
      try {
        const response = await SimuladorClient.obtenerContenido(
          this.idSession,
          idContent
        );
        this.contenido = response.datos;
        this.opciones = response.datos.opciones;
      } catch (error) {
        console.error('Error:', error);
      }
    },
    async responder(idOption) {
      try {
        const response = await SimuladorClient.guardarDecision(
          this.idSession,
          this.contenido.idContent,
          idOption
        );
        this.puntajeTotal = response.datos.puntajeTotalActualizado;
        // Mostrar feedback
        alert(`${response.datos.titulo}: ${response.datos.texto}`);
        // Cargar siguiente pregunta
        await this.cargarSiguiente();
      } catch (error) {
        if (error.response?.status === 403) {
          alert('Ya respondiste esta pregunta');
        }
      }
    }
  }
};
```

---

## ?? Datos de Prueba en BD

**Fase 1 - Análisis de Estímulos** (id_content=1)
- 3 opciones (0.5, 1, 2 puntos)
- Niveles: basic, medium, best

**Fase 1 - Toma de Decisiones** (id_content=2)
- 3 opciones (0.5, 1, 2 puntos)

**Fase 1 - Actitudes y Creencias** (id_content=3)
- 3 opciones (0.5, 1, 2 puntos)

**Fase 2 - Buyer Persona Hijo** (id_content=4)
- 4 opciones (0.5, 1, 2, 2.5 puntos)

**Fase 2 - Buyer Persona Padre** (id_content=5)
- 4 opciones (0.5, 1, 2, 2.5 puntos)

**Fase 3 - Precio vs. Calidad** (id_content=6)
- 4 opciones (0.5, 1, 2, 3 puntos)

**Fase 3 - Innovación vs. Diseño** (id_content=7)
- 4 opciones (0.5, 1, 2, 3 puntos)

**Fase 3 - Prestigio vs. Experiencia** (id_content=8)
- 4 opciones (0.5, 1, 2, 3 puntos)

**Total máximo: 20 puntos**

---

## ?? Flujo Visual Sugerido

```
1. Login Screen
   ?
2. Pantalla de Inicio de Simulación
   (Mostrar: usuario, puntaje, fase actual)
   ?
3. Pregunta + Opciones
   (Mostrar: intro, pregunta, 3-4 opciones)
   ?
4. Feedback + Puntaje
   (Mostrar: feedback, puntaje obtenido, puntaje total)
   ?
5. Siguiente Pregunta
   (Loop hasta completar)
   ?
6. Pantalla Final
   (Mostrar: puntaje final, nivel desempeño, historial)
```

---

## ? API está 100% lista para consumir ??
