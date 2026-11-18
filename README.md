# 🃏 Juego de Memoria de Cartas en Unity

Juego de parejas desarrollado con **Unity** y **C#**. El jugador voltea las cartas de dos en dos y debe encontrar todas las parejas con el menor número de movimientos y de tiempo. 🧠

📌 **Contexto:** práctica individual de la asignatura *Tecnologías de Desarrollo de Videojuegos y Entornos Virtuales I* (UDIT, curso 2025/2026). La memoria del proyecto la redacté yo y está en la carpeta `docs/`.

## 🚀 Funcionalidades

* 🎚️ **Tres dificultades:** fácil (3 parejas), media (5) y difícil (9), que cambian el tamaño de la cuadrícula.
* 🔀 **Cartas barajadas** en cada partida, con una cuadrícula que se adapta sola.
* 🔄 **Giro animado:** las cartas se voltean con una animación hecha por código.
* ⏱️ **Seguimiento de la partida:** cuenta movimientos, puntos y tiempo.
* 🏆 **Pantalla de resultados** con el resumen final.
* 🔊 **Sonido** al voltear, acertar, fallar y ganar.

## 🧠 Cómo funciona por dentro

| 🧩 Apartado | 🎯 Cómo está hecho |
|---|---|
| 🎚️ Dificultad | Se guarda con `PlayerPrefs` y reconfigura el `GridLayoutGroup`. |
| 🃏 Generación del nivel | Los prefabs se instancian por código con sprites de un array barajado (`barajarSprites()`). |
| 🔄 Animación del giro | **DOTween** (`DOScale` y `DORotate`); la cara cambia justo a los 90°. |
| 🔒 Control del input | El booleano `puedeJugar` bloquea el ratón mientras se comparan dos cartas. |
| ⚖️ Comparación | Una corrutina compara los sprites; si coinciden suma puntos, si no vuelve a esconderlas. |
| 📊 Resultados | `PlayerPrefs` guarda tiempo, movimientos y puntos para la escena final (TextMesh Pro). |

## 🛠️ Tecnologías y conceptos

* 💻 **C#** y **Unity UI (uGUI)**.
* 🎞️ **DOTween** para las animaciones.
* 💾 **PlayerPrefs** para pasar datos entre escenas.
* ⏱️ **Corrutinas** para la evaluación de las parejas.
* 🔤 **TextMesh Pro** para mostrar el resumen.
* 🎬 **SceneManager** para cambiar de escena.

## 🗂️ Piezas del proyecto

| 📄 Archivo | 🎯 Función |
|---|---|
| `Carta.cs` | Comportamiento de cada carta: mostrar y esconder. |
| `CartasControlador.cs` | Genera el nivel, baraja los sprites y gestiona la partida. |
| `MenuInicial.cs` | Menú de selección de dificultad. |
| `ResumenControlador.cs` | Pantalla final con tiempo, movimientos y puntos. |

## ▶️ Cómo jugarlo

1. Descarga el build para **Windows** desde la página del juego en itch.io (enlace pendiente).
2. Descomprime el archivo `.zip`.
3. Ejecuta el `.exe`, elige la dificultad y empieza a buscar parejas.

## 🧑‍💻 Cómo abrir el proyecto

1. Instala **Unity** con la versión indicada en `ProjectSettings/ProjectVersion.txt`.
2. Abre la carpeta del repositorio desde **Unity Hub**.
3. DOTween va incluido en `Assets/`; si Unity lo pide, ejecuta su configuración inicial.

## 📎 Documentación

La carpeta [`docs/`](docs/) incluye la **memoria** del proyecto: descripción general, descripción técnica, diario de desarrollo, anexos y bibliografía.

## 🙌 Créditos

* 🍔 Iconos: *Free Pixel Foods*, de ghostpixxells (itch.io).
* 🃏 Cartas: *Pixel Card Assets* (OpenGameArt).
* 🖼️ Fondos: *Diner Backgrounds*, de Spiral Atlas (itch.io).
* 🎞️ Animaciones: DOTween (Demigiant).
* 🔊 Efectos de sonido: Pixabay.
* 📺 Tutoriales de apoyo: *Cómo hacer un juego de memoria en Unity* y *Make a Memory Game in Unity Like a PRO* (YouTube).

---
🎓 *Práctica académica para trabajar interfaces, animaciones, lógica de estados y persistencia de datos en Unity.*
