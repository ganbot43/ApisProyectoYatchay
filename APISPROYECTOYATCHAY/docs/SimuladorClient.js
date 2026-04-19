import axios from 'axios';

const API_URL = 'https://localhost:5001/api';

/**
 * Cliente para interactuar con la API del Simulador
 */
class SimuladorClient {
  constructor() {
    this.client = axios.create({
      baseURL: API_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }

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
      console.error('Error al iniciar sesión:', error);
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
      console.error('Error al obtener estado:', error);
      throw error;
    }
  }

  /**
   * Obtiene el contenido de una fase específica
   * @param {number} idSession - ID de la sesión
   * @param {number} fase - Número de fase
   * @returns {Promise} Contenido de la fase
   */
  async obtenerContenido(idSession, fase) {
    try {
      const response = await this.client.get(`/simulation/content/${idSession}/${fase}`);
      return response.data;
    } catch (error) {
      console.error('Error al obtener contenido:', error);
      throw error;
    }
  }

  /**
   * Guarda la decisión del usuario (INMUTABLE)
   * @param {number} idSession - ID de la sesión
   * @param {number} idContent - ID del contenido
   * @param {number} opcionElegida - ID de la opción seleccionada
   * @returns {Promise} Feedback de la decisión
   */
  async guardarDecision(idSession, idContent, opcionElegida) {
    try {
      const response = await this.client.post('/simulation/decide', {
        idSession,
        idContent,
        opcionElegida,
      });
      return response.data;
    } catch (error) {
      // Si es 403, significa que ya existe la decisión (inmutabilidad)
      if (error.response?.status === 403) {
        console.warn('Decisión ya guardada (inmutable):', error.response.data);
      }
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
      console.error('Error al obtener historial:', error);
      throw error;
    }
  }
}

export default new SimuladorClient();

// ========== EJEMPLO DE USO ==========

/*
import SimuladorClient from './SimuladorClient';

async function flujoCompleto() {
  try {
    // 1. Iniciar sesión
    console.log('Iniciando sesión...');
    const sesionResponse = await SimuladorClient.iniciarSesion(1);
    const idSession = sesionResponse.datos.idSession;
    console.log('Sesión iniciada:', idSession);

    // 2. Obtener contenido de fase 1
    console.log('Obteniendo contenido de fase 1...');
    const contenidoResponse = await SimuladorClient.obtenerContenido(idSession, 1);
    const contenido = contenidoResponse.datos;
    console.log('Contenido:', contenido.titulo);
    
    // Parsear opciones JSON
    const opciones = JSON.parse(contenido.opciones);
    console.log('Opciones disponibles:', opciones);

    // 3. Responder pregunta
    console.log('Guardando decisión...');
    const decisionResponse = await SimuladorClient.guardarDecision(
      idSession,
      contenido.idContent,
      opciones[0].id // Seleccionar primera opción
    );
    const feedback = decisionResponse.datos;
    console.log('Feedback:', feedback);
    console.log('Puntaje obtenido:', feedback.puntajeObtenido);
    console.log('Puntaje total actualizado:', feedback.puntajeTotalActualizado);

    // 4. Obtener historial
    console.log('Obteniendo historial...');
    const historialResponse = await SimuladorClient.obtenerHistorial(idSession);
    console.log('Historial completo:', historialResponse.datos);

  } catch (error) {
    console.error('Error en flujo:', error);
  }
}

// Ejecutar
flujoCompleto();
*/
