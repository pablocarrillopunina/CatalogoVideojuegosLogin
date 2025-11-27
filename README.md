# 🕹️ Catálogo de Videojuegos con Sistema de Login  
**Práctica 3.1 – Desarrollo de Interfaces (C# / Windows Forms)**  

Este proyecto consiste en una aplicación de escritorio desarrollada en **C# y Windows Forms**, que incluye:

- 🔐 **Sistema de Login y Registro de Usuarios**
- 📂 **Gestión de un Catálogo de Videojuegos usando XML**
- ✔️ **Validaciones completas (campos vacíos, usuario incorrecto, contraseña errónea, usuario inexistente, etc.)**
- 🚫 **Bloqueo temporal tras 3 intentos fallidos**
- 🔄 **Cerrar sesión / Cerrar aplicación**

---

## 📌 Funcionalidades principales

### 🔐 **Login**
- Valida usuario y contraseña.
- No permite campos vacíos.
- Muestra mensajes claros de error o éxito.
- Bloqueo de usuario durante unos segundos tras 3 intentos fallidos.
- Permite registrarse desde el mismo formulario.

### 📝 **Registro de usuario**
- Alta de nuevos usuarios.
- Validación de repeticiones de contraseña.
- Mensajes de confirmación claros.

### 🎮 **Catálogo de Videojuegos (XML)**
- Añadir juegos
- Modificar juegos
- Eliminar juegos
- Cargar juegos desde un archivo XML
- Mostrar la lista en un `DataGridView`

Cada videojuego contiene:
- Título  
- Desarrollador  
- Plataforma  
- Precio 💶  

---

## 🛠️ **Tecnologías utilizadas**

- **C# .NET 8**
- **Windows Forms**
- **XML para almacenamiento**
- **GitHub para control de versiones**

---

## 📁 Estructura del proyecto

/PracticaXMLDinamica
│── FormLogin.cs
│── FormRegistro.cs
│── Form1.cs (catálogo de videojuegos)
│── catalogo.xml
│── Interfaz.xml
│── Program.cs


---

## 🧪 Testing (TC01 – TC07)

Se han probado todos los casos requeridos:

- ✔️ Login exitoso  
- ✔️ Login fallido  
- ✔️ Usuario no existente  
- ✔️ Campos vacíos  
- ✔️ Cerrar sesión  
- ✔️ Cerrar aplicación  
- ✔️ Bloqueo tras 3 intentos fallidos  

Todas las pruebas han sido marcadas como **OK**.

---

## 🚀 Ejecución

1. Clona el repositorio:
   ```bash
   git clone https://github.com/pablocarrillopunina/CatalogoVideojuegosLogin.git
Abre el proyecto en Visual Studio.

Ejecuta con F5.

👤 Autor

Pablo Carrillo Punina
IES Rey Fernando VI – DAM 2º
