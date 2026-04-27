import axios from 'axios';

const API_URL = 'https://localhost:5001/api';

/**
 * Cliente para interactuar con la API del Simulador Yachay-Tech
 */
class SimuladorClient {
  constructor() {
    this.client = axios.create({
      baseURL: API_URL,
      headers: {
        'Content-Type': 'application/json',
      },
      httpsAgent: {
        rejectUnauthorized: false, // Para desarrollo local
      },
    });
  }

  // ============ AUTENTICACIÓN ============

  /**
   * Registra un nuevo usuario
   * @param {string} nombre - Nombre del usuario
   * @param {string} apellido - Apellido del usuario
   * @param {string} correo - Correo institucional (@cibertec.edu.pe)
   * @param {string} contrasena - Contraseña
   * @param {string} dni - DNI (8 dígitos)
   * @param {number} idRol - ID del rol (1=estudiante, 2=docente, 3=admin)
   * @returns {Promise} Respuesta del registro
   */
  async registrar(nombre, apellido, correo, contrasena, dni, idRol = 1) {
    try {
      const response = await this.client.post('/auth/register', {
        nombre,
        apellido,
        correo,
        contrasena,
        dni,
        idRol,
      });
      return response.data;
    } catch (error) {
      console.error('Error al registrar:', error.response?.data || error.message);
      throw error;
    }
  }

  /**
   * Inicia sesión con correo y contraseña
   * @param {string} correo - Correo institucional
   * @param {string} contrasena - Contraseña
   * @returns {Promise} Datos del usuario autenticado
   */
  async login(correo, contrasena) {
    try {
      const response = await this.client.post('/auth/login', {
        correo,
        contrasena,
      });
      return response.data;
    } catch (error) {
      console.error('Error al login:', error.response?.data || error.message);
      throw error;
    }
  }

  // ============ SIMULACIÓN ============

  /**
   * Inicia una nueva sesión de simulación
   * @param {number} idUsuario - ID del usuario
   * @returns {Promise} Respuesta con datos de la sesión
   */
  async iniciarSesion(idUsuario) {
    try {
      const response = await this.client.post('/simulation/start', {
        idUsuario,
      });
      return response.data;
    } catch (error) {
      console.error('Error al iniciar sesión:', error.response?.data || error.message);
      throw error;
    }
  }

  /**
   * Obtiene el estado actual de una sesión
   * @param {number} idSession - ID de la sesión
   * @returns {Promise} Estado de la sesión
   */
  async obtenerEstado(idSession) {
    try {
      const response = await this.client.get(`/simulation/status/${idSession}`);
      return response.data;
    } catch (error) {
      console.error('Error al obtener estado:', error.response?.data || error.message);
      throw error;
    }
  }

  /**
   * Obtiene el contenido de una pregunta específica
   * @param {number} idSession - ID de la sesión
   * @param {number} idContent - ID del contenido/pregunta
   * @returns {Promise} Contenido con opciones
   */
  async obtenerContenido(idSession, idContent) {
    try {
      const response = await this.client.get(
        `/simulation/content/${idSession}/${idContent}`
      );
      return response.data;
    } catch (error) {
      console.error('Error al obtener contenido:', error.response?.data || error.message);
      throw error;
    }
  }

  /**
   * Guarda la decisión del usuario (INMUTABLE - solo una vez por pregunta)
   * @param {number} idSession - ID de la sesión
   * @param {number} idContent - ID del contenido/pregunta
   * @param {number} idOption - ID de la opción seleccionada
   * @returns {Promise} Feedback de la decisión
   */
  async guardarDecision(idSession, idContent, idOption) {
    try {
      const response = await this.client.post('/simulation/decide', {
        idSession,
        idContent,
        idOption,
      });
      return response.data;
    } catch (error) {
      // Si es 403, significa que ya existe la decisión (inmutabilidad)
      if (error.response?.status === 403) {
        console.warn('Decisión ya guardada (inmutable):', error.response.data);
      }
      console.error('Error al guardar decisión:', error.response?.data || error.message);
      throw error;
    }
  }

  /**
   * Obtiene el historial de decisiones de una sesión
   * @param {number} idSession - ID de la sesión
   * @returns {Promise} Historial de decisiones
   */
  async obtenerHistorial(idSession) {
    try {
      const response = await this.client.get(`/simulation/history/${idSession}`);
      return response.data;
    } catch (error) {
      console.error('Error al obtener historial:', error.response?.data || error.message);
      throw error;
    }
  }
}

export default new SimuladorClient();

// ========== EJEMPLO DE USO COMPLETO ==========

/*
import SimuladorClient from './SimuladorClient';

async function flujoCompleto() {
  try {
    console.log('=== SIMULADOR YACHAY-TECH ===\n');

    // 1. REGISTRO (Opcional - si es primer acceso)
    console.log('1. Registrando nuevo usuario...');
    try {
      const registroResponse = await SimuladorClient.registrar(
        'Juan',
        'Pérez',
        'i123456789@cibertec.edu.pe',
        'Contraseña123@',
        '12345678',
        1 // Estudiante
      );
      console.log('? Usuario registrado:', registroResponse.mensaje);
    } catch (e) {
      console.log('Usuario ya existe, continuando...');
    }

    // 2. LOGIN
    console.log('\n2. Iniciando sesión...');
    const loginResponse = await SimuladorClient.login(
      'i123456789@cibertec.edu.pe',
      'Contraseña123@'
    );
    const idUsuario = loginResponse.datos.idUsuario;
    console.log('? Login exitoso. Usuario:', idUsuario);

    // 3. INICIAR SIMULACIÓN
    console.log('\n3. Iniciando simulación...');
    const sesionResponse = await SimuladorClient.iniciarSesion(idUsuario);
    const idSession = sesionResponse.datos.idSession;
    console.log('? Sesión iniciada:', idSession);
    console.log('  Fase actual:', sesionResponse.datos.faseActual);
    console.log('  Puntaje:', sesionResponse.datos.puntajeTotal);

    // 4. OBTENER PRIMERA PREGUNTA
    console.log('\n4. Obteniendo primera pregunta...');
    const contenidoResponse = await SimuladorClient.obtenerContenido(idSession, 1);
    const contenido = contenidoResponse.datos;
    console.log('? Pregunta cargada:');
    console.log('  Título:', contenido.titulo);
    console.log('  Tipo:', contenido.tipo);
    console.log('  Categoría:', contenido.categoria);
    console.log('  Intro:', contenido.intro);
    console.log('  Pregunta:', contenido.pregunta);
    console.log('  Opciones disponibles:');
    
    contenido.opciones.forEach((opt, idx) => {
      console.log(`    ${idx + 1}. [ID: ${opt.idOption}] ${opt.texto} (${opt.puntaje} pts, ${opt.nivel})`);
    });

    // 5. RESPONDER PREGUNTA
    console.log('\n5. Guardando decisión (seleccionando opción con más puntos)...');
    const mejorOpcion = contenido.opciones.reduce((max, opt) => 
      opt.puntaje > max.puntaje ? opt : max
    );
    
    const decisionResponse = await SimuladorClient.guardarDecision(
      idSession,
      contenido.idContent,
      mejorOpcion.idOption
    );
    const feedback = decisionResponse.datos;
    console.log('? Decisión guardada:');
    console.log('  Opción elegida:', mejorOpcion.texto);
    console.log('  Puntaje obtenido:', feedback.puntajeObtenido);
    console.log('  Feedback título:', feedback.titulo);
    console.log('  Feedback texto:', feedback.texto);
    console.log('  Resultado:', feedback.resultado);
    console.log('  Puntaje total actualizado:', feedback.puntajeTotalActualizado);
    console.log('  ¿Puede siguiente?', feedback.puedeSiguiente);

    // 6. OBTENER ESTADO ACTUAL
    console.log('\n6. Obteniendo estado actual...');
    const estadoResponse = await SimuladorClient.obtenerEstado(idSession);
    console.log('? Estado actual:');
    console.log('  Fase:', estadoResponse.datos.faseActual);
    console.log('  Estado:', estadoResponse.datos.estado);
    console.log('  Puntaje total:', estadoResponse.datos.puntajeTotal);

    // 7. OBTENER HISTORIAL
    console.log('\n7. Obteniendo historial de decisiones...');
    const historialResponse = await SimuladorClient.obtenerHistorial(idSession);
    const historial = historialResponse.datos;
    console.log('? Historial:');
    console.log('  Total decisiones:', historial.totalDecisiones);
    console.log('  Puntaje total:', historial.puntajeTotal);
    historial.decisionesPrevias.forEach((dec, idx) => {
      console.log(`  Decisión ${idx + 1}: ID Option=${dec.idOption}, Puntaje=${dec.puntajeObtenido}`);
    });

    console.log('\n=== FLUJO COMPLETADO EXITOSAMENTE ===');

  } catch (error) {
    console.error('? Error en flujo:', error.message);
    console.error('Detalles:', error.response?.data || error);
  }
}

// Ejecutar
flujoCompleto();
*/
