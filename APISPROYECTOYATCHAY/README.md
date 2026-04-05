# 🎓 ApisProyectoYatchay - Sistema Quiz Inteligente

API REST desarrollada con **.NET 8** para un **Sistema de Quiz Adaptativo** con autenticación de usuarios, gestión de roles y lógica de ramificación inteligente.

## 📋 Descripción General

ApisProyectoYatchay es una plataforma educativa que permite:
- **Autenticación segura** de usuarios con hash SHA256
- **Gestión de roles**: Estudiante (resuelve quizzes) y Analista (crea/administra quizzes)
- **Sistema de Quiz adaptativo** con lógica de árbol de decisiones
- **Seguimiento de respuestas** según el camino elegido por el estudiante
- **API RESTful** completa con documentación Swagger

## ✨ Características

### Autenticación y Usuarios
- ✅ Registro de nuevos usuarios
- ✅ Login seguro con contraseñas hasheadas (SHA256)
- ✅ Gestión de dos roles: Estudiante y Analista
- ✅ Validación de correo y DNI únicos

### Quiz Adaptativo
- 🌳 Lógica de árbol de decisiones
- 📊 Preguntas con múltiples respuestas
- 🔀 Ramificación dinámica según respuestas
- 📈 Seguimiento del progreso del estudiante
- 🎯 Resultados y análisis

### Arquitectura
- 🏗️ Patrón Repository
- 💾 Stored Procedures en SQL Server
- 📚 Inyección de dependencias (DI)
- 🔒 Separación de capas
- 📖 Swagger/OpenAPI integrado

## 🚀 Tecnologías

| Tecnología | Versión |
|-----------|---------|
| .NET | 8 |
| SQL Server | 2019+ |
| ASP.NET Core | 8 |
| C# | 12.0 |

## 📦 Requisitos

- ✔️ .NET 8 SDK
- ✔️ SQL Server 2019 o superior
- ✔️ Visual Studio 2022+ o VS Code
- ✔️ Git

## 🔧 Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/TU_USUARIO/ApisProyectoYatchay.git
cd ApisProyectoYatchay/APISPROYECTOYATCHAY
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar la base de datos

- Abre `appsettings.Development.json`
- Actualiza la cadena de conexión con tu servidor SQL Server

```json
{
  "ConnectionStrings": {
    "Default": "Server=TU_SERVIDOR;Database=SistemaQuizYatchay;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Crear la base de datos

Ejecuta el script SQL en `docs/database/SistemaQuizYatchay.sql` en SQL Server Management Studio

### 5. Ejecutar la aplicación

```bash
dotnet run
```

La API estará disponible en: `https://localhost:7000`

## 📚 Documentación de API

### Acceder a Swagger

Una vez ejecutada la aplicación, accede a:
```
https://localhost:7000/swagger
```

### Endpoints Principales

#### **Registro de Usuario**
```http
POST /api/usuarios/registro
Content-Type: application/json

{
  "nombre": "Juan",
  "apellido": "Pérez",
  "correo": "juan@example.com",
  "contrasena": "SecurePassword123!",
  "dni": "12345678",
  "idRol": 1
}
```

**Respuesta:**
```json
{
  "exito": 1,
  "mensaje": "Usuario registrado exitosamente"
}
```

#### **Login**
```http
POST /api/usuarios/login
Content-Type: application/json

{
  "correo": "juan@example.com",
  "contrasena": "SecurePassword123!"
}
```

**Respuesta:**
```json
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

## 🏗️ Estructura del Proyecto

```
APISPROYECTOYATCHAY/
├── Controllers/
│   └── UsuariosController.cs          # Endpoints de autenticación
├── Repositories/
│   ├── Interfaces/
│   │   └── IUsuarioRepository.cs      # Contrato del repositorio
│   └── UsuarioRepository.cs           # Implementación con SQL
├── Contracts/
│   └── Dtos/
│       ├── LoginDto.cs                # DTO de login
│       ├── LoginResponseDto.cs        # DTO de respuesta login
│       └── RegistroDto.cs             # DTO de registro
├── Program.cs                         # Configuración de inicio
├── appsettings.json                  # Configuración general
└── appsettings.Development.json      # Configuración local (git ignored)
```

## 🔐 Seguridad

- **Contraseñas**: Hasheadas con SHA256
- **Validación**: Email y DNI únicos
- **Roles**: Control de acceso basado en roles (RBAC)
- **API Keys**: (Próxima implementación)
- **JWT**: (Próxima implementación)

## 📅 Roadmap

### Versión 1.0 (Actual)
- [x] Autenticación de usuarios
- [x] Gestión de roles
- [x] Endpoints de registro y login
- [x] Base de datos con Stored Procedures

### Versión 1.1 (Próxima)
- [ ] Sistema de Quiz básico
- [ ] Crear/Editar preguntas (Analista)
- [ ] Resolver quiz (Estudiante)
- [ ] Almacenar respuestas

### Versión 1.2
- [ ] Lógica de árbol de decisiones
- [ ] Quiz adaptativos
- [ ] Resultados y reportes

### Versión 2.0
- [ ] Autenticación JWT
- [ ] Roles y permisos avanzados
- [ ] Estadísticas y analytics
- [ ] Web UI



