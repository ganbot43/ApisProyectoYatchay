# ?? GUÍA RÁPIDA PARA FRONTEND

## ? API ESTÁ 100% LISTA PARA CONSUMIR

---

## ?? Pasos para integrar tu Frontend

### 1. Copiar SimuladorClient.js
```bash
# Copia este archivo a tu proyecto frontend
APISPROYECTOYATCHAY/docs/SimuladorClient.js

# En tu proyecto:
src/services/SimuladorClient.js
```

### 2. Instalar Axios
```bash
npm install axios
```

### 3. Importar en tu componente
```javascript
import SimuladorClient from '@/services/SimuladorClient';
```

---

## ?? Flujo Básico (Copy-Paste Ready)

```javascript
import SimuladorClient from '@/services/SimuladorClient';

async function flujoSimulador() {
  // 1. LOGIN
  const loginRes = await SimuladorClient.login(
    'i123456789@cibertec.edu.pe',
    'Contraseña123@'
  );
  const idUsuario = loginRes.datos.idUsuario;

  // 2. INICIAR SESIÓN
  const sesionRes = await SimuladorClient.iniciarSesion(idUsuario);
  const idSession = sesionRes.datos.idSession;

  // 3. CARGAR PRIMERA PREGUNTA
  const preguntaRes = await SimuladorClient.obtenerContenido(idSession, 1);
  const pregunta = preguntaRes.datos;
  
  console.log(pregunta.titulo);
  console.log(pregunta.pregunta);
  console.log(pregunta.opciones); // Array de opciones

  // 4. RESPONDER (cuando usuario hace clic en opción)
  const decisionRes = await SimuladorClient.guardarDecision(
    idSession,
    pregunta.idContent,
    pregunta.opciones[0].idOption // ? IMPORTANTE: usar idOption
  );
  
  const feedback = decisionRes.datos;
  console.log(feedback.titulo);        // "¡Excelente..."
  console.log(feedback.texto);         // Detalles del feedback
  console.log(feedback.puntajeObtenido); // 2
  console.log(feedback.puntajeTotalActualizado); // 2

  // 5. CARGAR SIGUIENTE PREGUNTA
  const siguienteRes = await SimuladorClient.obtenerContenido(idSession, 2);
  // ... repetir paso 4

  // 6. AL FINAL: OBTENER HISTORIAL
  const historialRes = await SimuladorClient.obtenerHistorial(idSession);
  console.log(historialRes.datos.puntajeTotal); // Puntaje final
}
```

---

## ?? Estructura de UI Sugerida

### Pantalla 1: Login
```
???????????????????????????
?   SIMULADOR YACHAY      ?
?                         ?
? Correo: [____________]  ?
? Pass:   [____________]  ?
?         [INGRESAR]      ?
???????????????????????????
```

### Pantalla 2: Pregunta + Opciones
```
???????????????????????????????????????
? Fase: fase_1                        ?
? Puntaje: 5/20                       ?
???????????????????????????????????????
? ANÁLISIS DE ESTÍMULOS               ?
?                                     ?
? Has decidido iniciar con...         ?
? ¿Qué instrumento...?                ?
???????????????????????????????????????
? ? Auditoría de Contenido (0.5 pts)  ?
? ? Test de Producto (1 pts)          ?
? ? Prueba de Rendimiento (2 pts)     ?
???????????????????????????????????????
```

### Pantalla 3: Feedback Inmediato
```
???????????????????????????????????????
? ¡Decisión estratégica superior!     ?
?                                     ?
? Has seleccionado una herramienta    ?
? que analiza profundamente...        ?
?                                     ?
? Puntaje obtenido: +2 pts            ?
? Total actualizado: 7/20 pts         ?
?                                     ?
?        [SIGUIENTE PREGUNTA]         ?
???????????????????????????????????????
```

### Pantalla 4: Resumen Final
```
???????????????????????????????????????
? ?? ¡SIMULACIÓN COMPLETADA!          ?
?                                     ?
? Puntaje Total: 20/20 PUNTOS         ?
? Nivel: SOBRESALIENTE                ?
?                                     ?
? Decisiones tomadas:                 ?
? 1. Prueba de Rendimiento ? +2 pts   ?
? 2. Shadowing ? +2 pts               ?
? 3. Focus Group ? +2 pts             ?
? ... (8 preguntas)                   ?
?                                     ?
?    [DESCARGAR PDF] [SALIR]          ?
???????????????????????????????????????
```

---

## ?? Datos Importantes

### Cada Opción Tiene:
```javascript
{
  idOption: 1,              // ? USA ESTO en guardarDecision
  idOptionExterno: "...",   // ID único para DB
  texto: "Auditoría...",    // Mostrar al usuario
  puntaje: 0.5,             // Puntos que suma
  nivel: "basic",           // basic, regular, medium, good, best
  feedback: "¡Excelente...", // Texto que ver después
  resultado: "Existe falla...", // Análisis detallado
  insight: "El entorno...", // Insight clave
}
```

### Tipos de Respuesta:
```javascript
// OK - Decisión guardada
{
  exito: 1,
  datos: { puntajeObtenido: 2, ... }
}

// ERROR - Ya respondiste
{
  exito: 0,
  mensaje: "Ya has respondido esta pregunta"
}

// ERROR - Validación fallida
{
  exito: 0,
  mensaje: "Datos inválidos"
}
```

---

## ??? Ejemplo React Completo

```javascript
import { useState } from 'react';
import SimuladorClient from '@/services/SimuladorClient';

export default function Simulador() {
  const [idSession, setIdSession] = useState(null);
  const [preguntaActual, setPreguntaActual] = useState(null);
  const [puntaje, setPuntaje] = useState(0);
  const [feedback, setFeedback] = useState(null);
  const [cargando, setCargando] = useState(false);

  const iniciar = async () => {
    setCargando(true);
    try {
      // Login automático (en producción: formulario de login)
      const login = await SimuladorClient.login(
        'i123456789@cibertec.edu.pe',
        'Contraseña123@'
      );
      
      const sesion = await SimuladorClient.iniciarSesion(
        login.datos.idUsuario
      );
      setIdSession(sesion.datos.idSession);
      
      cargarPregunta(1, sesion.datos.idSession);
    } catch (error) {
      alert('Error: ' + error.message);
    }
    setCargando(false);
  };

  const cargarPregunta = async (idContent, sessionId) => {
    try {
      const res = await SimuladorClient.obtenerContenido(sessionId, idContent);
      setPreguntaActual(res.datos);
      setFeedback(null);
    } catch (error) {
      alert('Error: ' + error.message);
    }
  };

  const responder = async (idOption) => {
    setCargando(true);
    try {
      const res = await SimuladorClient.guardarDecision(
        idSession,
        preguntaActual.idContent,
        idOption
      );
      
      const { feedback: fb, puntajeTotalActualizado } = res.datos;
      setFeedback(res.datos);
      setPuntaje(puntajeTotalActualizado);
      
      // Auto avanzar en 3 segundos
      setTimeout(() => {
        if (preguntaActual.idContent < 8) {
          cargarPregunta(preguntaActual.idContent + 1, idSession);
        }
      }, 3000);
    } catch (error) {
      if (error.response?.status === 403) {
        alert('Ya respondiste esta pregunta');
      } else {
        alert('Error: ' + error.message);
      }
    }
    setCargando(false);
  };

  if (!idSession) {
    return (
      <button onClick={iniciar} disabled={cargando}>
        {cargando ? 'Cargando...' : 'Iniciar Simulador'}
      </button>
    );
  }

  return (
    <div>
      <h1>{preguntaActual?.titulo}</h1>
      <p>Puntaje: {puntaje}/20</p>
      
      {!feedback ? (
        <div>
          <h3>{preguntaActual?.pregunta}</h3>
          {preguntaActual?.opciones?.map((opt) => (
            <button
              key={opt.idOption}
              onClick={() => responder(opt.idOption)}
              disabled={cargando}
            >
              {opt.texto} ({opt.puntaje} pts)
            </button>
          ))}
        </div>
      ) : (
        <div>
          <h3>{feedback.titulo}</h3>
          <p>{feedback.texto}</p>
          <p>Puntos: +{feedback.puntajeObtenido}</p>
          <p>Total: {feedback.puntajeTotalActualizado}</p>
          <p>(Siguiente pregunta en 3 segundos...)</p>
        </div>
      )}
    </div>
  );
}
```

---

## ?? Ejemplo Vue Completo

```vue
<template>
  <div class="simulador">
    <button v-if="!idSession" @click="iniciar">
      Iniciar Simulador
    </button>
    
    <div v-else>
      <h1>{{ preguntaActual?.titulo }}</h1>
      <p>Puntaje: {{ puntaje }}/20</p>
      
      <div v-if="!feedback">
        <h3>{{ preguntaActual?.pregunta }}</h3>
        <button
          v-for="opt in preguntaActual?.opciones"
          :key="opt.idOption"
          @click="responder(opt.idOption)"
          :disabled="cargando"
        >
          {{ opt.texto }} ({{ opt.puntaje }} pts)
        </button>
      </div>
      
      <div v-else>
        <h3>{{ feedback.titulo }}</h3>
        <p>{{ feedback.texto }}</p>
        <p>+{{ feedback.puntajeObtenido }} pts</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import SimuladorClient from '@/services/SimuladorClient';

const idSession = ref(null);
const preguntaActual = ref(null);
const puntaje = ref(0);
const feedback = ref(null);
const cargando = ref(false);

const iniciar = async () => {
  cargando.value = true;
  const login = await SimuladorClient.login(
    'i123456789@cibertec.edu.pe',
    'Contraseña123@'
  );
  const sesion = await SimuladorClient.iniciarSesion(login.datos.idUsuario);
  idSession.value = sesion.datos.idSession;
  cargarPregunta(1);
  cargando.value = false;
};

const cargarPregunta = async (idContent) => {
  const res = await SimuladorClient.obtenerContenido(idSession.value, idContent);
  preguntaActual.value = res.datos;
  feedback.value = null;
};

const responder = async (idOption) => {
  cargando.value = true;
  const res = await SimuladorClient.guardarDecision(
    idSession.value,
    preguntaActual.value.idContent,
    idOption
  );
  feedback.value = res.datos;
  puntaje.value = res.datos.puntajeTotalActualizado;
  cargando.value = false;
};
</script>
```

---

## ?? PUNTOS IMPORTANTES

### 1. SIEMPRE usa `idOption` (no índice)
```javascript
// ? CORRECTO
guardarDecision(idSession, idContent, opcion.idOption)

// ? INCORRECTO
guardarDecision(idSession, idContent, 0) // índice del array
```

### 2. 403 = Ya respondiste (es normal)
```javascript
try {
  await guardarDecision(...)
} catch (error) {
  if (error.response?.status === 403) {
    // Ya respondiste esta pregunta, es esperado
  }
}
```

### 3. Estructura de opciones
```javascript
// Cada pregunta viene con array de opciones
{
  opciones: [
    { idOption: 1, texto: "...", puntaje: 0.5, ... },
    { idOption: 2, texto: "...", puntaje: 1, ... },
    { idOption: 3, texto: "...", puntaje: 2, ... }
  ]
}
```

---

## ?? Soporte Rápido

| Problema | Solución |
|----------|----------|
| 404 Not Found | Verifica puerto (5001) y rutas |
| 400 Bad Request | Revisa datos enviados (idOption vs índice) |
| 403 Forbidden | Ya respondiste esa pregunta (espera OK) |
| CORS Error | Agrega `httpsAgent` al cliente |
| Conexión rechazada | API no está corriendo |

---

## ? TODO LISTO PARA RENDERIZAR ??

**Proximos pasos:**
1. Copia `SimuladorClient.js` a tu proyecto
2. Usa uno de los ejemplos (React/Vue)
3. Personaliza estilos
4. Conecta tu formulario de login
5. ¡Listo para producción!
