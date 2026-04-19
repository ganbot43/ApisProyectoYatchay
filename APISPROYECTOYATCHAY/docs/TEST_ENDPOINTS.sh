#!/bin/bash

# ?? EJEMPLOS DE PRUEBA - API Simulador
# Ejecuta estos comandos para probar los endpoints

API_URL="https://localhost:5001/api"
USER_ID=1

echo "=========================================="
echo "     EJEMPLOS DE PRUEBA - API SIMULADOR"
echo "=========================================="

# 1?? INICIAR SESIÓN DE SIMULACIÓN
echo ""
echo "1?? INICIAR SESIÓN"
echo "Endpoint: POST /api/simulation/start"
curl -X POST "$API_URL/simulation/start" \
  -H "Content-Type: application/json" \
  -d "{
    \"idUsuario\": $USER_ID
  }" \
  -w "\nStatus: %{http_code}\n\n"

# Guarda el idSession de la respuesta anterior para los siguientes comandos
SESSION_ID=1  # Reemplazar con el ID real de la respuesta

# 2?? OBTENER ESTADO DE SESIÓN
echo ""
echo "2?? OBTENER ESTADO DE SESIÓN"
echo "Endpoint: GET /api/simulation/status/{idSession}"
curl -X GET "$API_URL/simulation/status/$SESSION_ID" \
  -H "Content-Type: application/json" \
  -w "\nStatus: %{http_code}\n\n"

# 3?? OBTENER CONTENIDO DE FASE 1
echo ""
echo "3?? OBTENER CONTENIDO DE FASE 1"
echo "Endpoint: GET /api/simulation/content/{idSession}/{fase}"
curl -X GET "$API_URL/simulation/content/$SESSION_ID/1" \
  -H "Content-Type: application/json" \
  -w "\nStatus: %{http_code}\n\n"

# 4?? GUARDAR DECISIÓN (Primera respuesta - Debe exitosa)
echo ""
echo "4?? GUARDAR DECISIÓN - OPCIÓN 1"
echo "Endpoint: POST /api/simulation/decide"
curl -X POST "$API_URL/simulation/decide" \
  -H "Content-Type: application/json" \
  -d "{
    \"idSession\": $SESSION_ID,
    \"idContent\": 1,
    \"opcionElegida\": 1
  }" \
  -w "\nStatus: %{http_code}\n\n"

# 5?? GUARDAR DECISIÓN DUPLICADA (Debe fallar con 403)
echo ""
echo "5?? GUARDAR DECISIÓN DUPLICADA - DEBE FALLAR CON 403"
echo "Endpoint: POST /api/simulation/decide (mismo contenido)"
curl -X POST "$API_URL/simulation/decide" \
  -H "Content-Type: application/json" \
  -d "{
    \"idSession\": $SESSION_ID,
    \"idContent\": 1,
    \"opcionElegida\": 2
  }" \
  -w "\nStatus: %{http_code}\n\n"

# 6?? OBTENER HISTORIAL
echo ""
echo "6?? OBTENER HISTORIAL DE DECISIONES"
echo "Endpoint: GET /api/simulation/history/{idSession}"
curl -X GET "$API_URL/simulation/history/$SESSION_ID" \
  -H "Content-Type: application/json" \
  -w "\nStatus: %{http_code}\n\n"

# 7?? RESPONDER SIGUIENTE CONTENIDO
echo ""
echo "7?? RESPONDER CONTENIDO DIFERENTE (id_content: 2)"
curl -X POST "$API_URL/simulation/decide" \
  -H "Content-Type: application/json" \
  -d "{
    \"idSession\": $SESSION_ID,
    \"idContent\": 2,
    \"opcionElegida\": 1
  }" \
  -w "\nStatus: %{http_code}\n\n"

echo ""
echo "=========================================="
echo "? PRUEBA COMPLETADA"
echo "=========================================="
