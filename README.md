# Burla de Centinelas · Prototipo

**Trabajo Práctico de Programación de Videojuegos I**  
*Tecnicatura Universitaria en Diseño Integral de Videojuegos — Facultad de Ingeniería, UNJu*

**Desarrolladora:** Gianella Gutierrez · **LU:** TUDIVJ00080

**Versión de Unity:** Unity 6.5

## Vista general del nivel



## Descripción del juego

*Burla de Centinelas* es un prototipo de videojuego 3D de exploración, supervivencia y evasión, desarrollado en Unity.

El jugador debe recorrer el escenario y recolectar **100 datos** mientras intenta escapar de los centinelas que patrullan el área y persiguen al detectar su presencia. El jugador debe administrar sus habilidades y aprovechar los elementos del entorno para evitar ser alcanzado.

**Objetivo:** recolectar 100 datos para alcanzar la victoria y evitar que los centinelas derroten al jugador.

**Peligros:** los centinelas pueden patrullar y perseguir al jugador. Además, el escenario contiene una zona de barro que reduce considerablemente su velocidad, facilitando que los enemigos lo alcancen.

**Ayudas:** el jugador dispone de un dash para desplazarse rápidamente y escapar de situaciones peligrosas. También puede recoger un power-up de invisibilidad que dificulta que los centinelas lo detecten durante un tiempo limitado de 5 segundos.

La estrategia es fundamental: utilizar el dash en el momento adecuado y aprovechar la invisibilidad puede marcar la diferencia entre escapar y ser atrapado.

## Guía de controles

| Tecla | Acción |
|---|---|
| `W` / `A` / `S` / `D` | Mover al jugador |
| `Espacio` | Saltar |
| `Shift` | Realizar un dash |
| `F` | Aturdir a los centinelas |
| `Enter` | Iniciar la partida |
| `Esc` | Pausar o reanudar el juego |

## Requisitos obligatorios

## Scripts del proyecto

El proyecto utiliza scripts desarrollados en C# para implementar el movimiento del jugador, la inteligencia artificial de los centinelas, la recolección de datos y las distintas mecánicas del escenario.

| Script | Funcionalidad |
|---|---|
| `CentinelaIA.cs` | Controla la inteligencia artificial del centinela mediante `Patrullar()`, `DetectarJugador()` y `PerderJugador()`, utilizando `NavMeshAgent` para desplazarse entre puntos o perseguir al jugador. `OnCollisionEnter()` aplica daño al jugador, mientras que `Aturdir()` y la corrutina `Aturdimiento()` detienen temporalmente al centinela y modifican su color. |
| `Dato.cs` | Gestiona la recolección de datos mediante `OnTriggerEnter()`. Comprueba si el jugador entra en contacto, llama a `SumarPuntos()` de `PlayerState` para otorgar los puntos configurados y destruye el objeto recolectado. |
| `GameManagerListener.cs` | Se suscribe a los eventos `OnVictoria` y `OnDerrota` del `GameManager` mediante `OnEnable()` y `OnDisable()`. `MostrarVictoria()` y `MostrarDerrota()` muestran el resultado correspondiente, mientras que `MostrarResultado()` actualiza y activa el texto en pantalla. |
| `PlayerMovements.cs` | Gestiona el movimiento y salto del jugador mediante `MoverJugador()`, `ComprobarSuelo()` y `GestionarSalto()`. `GestionarInteraccion()` detecta objetos mediante Raycast, mientras que `GestionarAturdimiento()` permite aturdir centinelas. `ActualizarLaser()` muestra la dirección del apuntado. `RealizarDash()` implementa el desplazamiento rápido con tiempo de recarga, y `CambiarVelocidad()` y `RestaurarVelocidad()` controlan la velocidad en zonas como el barro. `OnDrawGizmos()` visualiza los raycasts en el editor. |
| `PlayerState.cs` | Gestiona la salud y la puntuación del jugador. `RecibirDano()` reduce la salud y declara la derrota al llegar a cero; `SumarPuntos()` aumenta la puntuación y declara la victoria al alcanzar 100 puntos. `ActualizarHUD()` y `ActualizarBarraSalud()` actualizan la interfaz. `Start()` inicializa los valores y la interfaz al comenzar la partida. |
| `PowerUpInvisibilidad.cs` | Gestiona la invisibilidad temporal mediante `ActivarInvisibilidad()` e `InvisibilidadTemporal()`, una corrutina que controla la duración del efecto. `CambiarVisibilidad()` oculta o muestra los renderizadores del jugador, mientras que `OnTriggerEnter()` activa el efecto y destruye el objeto recolectado. |
| `ZonaBarro.cs` | Reduce la velocidad del jugador al entrar en la zona mediante `OnTriggerEnter()` y `CambiarVelocidad()`. Al salir, `OnTriggerExit()` llama a `RestaurarVelocidad()` para recuperar la velocidad habitual. |
| `ZonaDeteccion.cs` | Gestiona el área de detección del centinela mediante un `SphereCollider` configurado como Trigger. Detecta cuándo el jugador entra o sale de la zona y comprueba su estado de invisibilidad. Si el jugador es invisible, el centinela deja de detectarlo; cuando vuelve a ser visible dentro del área, puede detectarlo nuevamente. También muestra el radio de detección mediante un Gizmo en el editor de Unity. |
| `GameManager.cs` | Administra los estados del juego mediante `CambiarEstado()`, que controla el tiempo con `Time.timeScale` y notifica los cambios mediante `OnEstadoCambiado`. `Update()` gestiona el inicio con Enter y la pausa con Escape. `DeclararVictoria()` y `DeclararDerrota()` finalizan la partida y emiten sus respectivos eventos. |
## Mecánicas principales

- **Recolección de datos:** el jugador debe reunir 100 datos para alcanzar la victoria.
- **Inteligencia artificial:** los centinelas patrullan y persiguen al jugador cuando lo detectan.
- **Dash:** permite desplazarse rápidamente para escapar de los enemigos, teniendo en cuenta su tiempo de reutilización.
- **Aturdimiento:** permite incapacitar temporalmente a los centinelas mediante una habilidad del jugador.
- **Zona de barro:** reduce considerablemente la velocidad y aumenta el riesgo de ser alcanzado.
- **Power-up de invisibilidad:** evita temporalmente la detección del jugador durante 5 segundos.
- **Gestión de estados:** controla el inicio, la pausa, la victoria y la derrota de la partida.


## Detalles técnicos

- **Detección del suelo:** se utiliza un Raycast para comprobar si el jugador está sobre una superficie válida antes de permitir el salto.
- **Interacción:** se utilizan raycasts y máscaras de capas para detectar elementos interactivos y enemigos dentro del alcance establecido.
- **Inteligencia artificial:** los centinelas patrullan entre puntos definidos y pueden perseguir al jugador cuando lo detectan.
- **Dash:** permite aumentar temporalmente la velocidad de desplazamiento. Cuenta con una duración y un tiempo de reutilización.
- **Aturdimiento:** el jugador puede utilizar una habilidad para aturdir temporalmente a los centinelas.
- **Zona de barro:** reduce la velocidad del jugador mientras permanece dentro de ella.
- **Power-up de invisibilidad:** vuelve al jugador indetectable para los centinelas durante 5 segundos.
- **Sistema de puntuación:** cada dato recolectado suma puntos. El objetivo es alcanzar los 100 puntos necesarios para ganar.
- **Gestión de estados:** el `GameManager` controla el inicio, la pausa, la victoria y la derrota de la partida.

## Detalle de opcionales elegidos

### Opción 1 · Dash

Habilidad que permite al jugador desplazarse rápidamente para escapar de los centinelas y atravesar zonas peligrosas. Su uso debe planificarse debido a su tiempo de reutilización.



### Opción 2 · Power-up de invisibilidad

Objeto recolectable que permite al jugador evitar la detección de los centinelas durante 5 segundos, facilitando el escape en situaciones de riesgo.



### Opción 3 · Zona de desaceleración (barro)

Zona del escenario que reduce la velocidad del jugador mientras permanece dentro de sus límites. Obliga a buscar una ruta alternativa o utilizar el dash estratégicamente para escapar de los enemigos.



## Capturas del juego

### Vista general del nivel

![Vista general del nivel](Capturas/nivel.png)

### Jugador y centinelas

![Jugador y centinelas](Capturas/centinelas.png)

### Power-up de invisibilidad

![Power-up de invisibilidad](Capturas/invisibilidad.png)

### Partida en ejecución

![Partida en ejecución](Capturas/partida.png)

## Cómo abrir y ejecutar el proyecto

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/AlezzzGutierrez/TP2_PVJ1
   ```

2. Abrir Unity Hub y seleccionar **Add project from disk**.
3. Elegir la carpeta del proyecto.
4. Abrirlo con la versión de Unity utilizada para desarrollarlo.
5. Abrir la escena principal desde `Assets/Scenes/`.
6. Presionar **Enter** para ejecutar el prototipo.
7. En caso de no generarse el NavMesh, desactivar desde la jerarquia los componentes Player, Centinelas, ZonasDeBarro, PowersUp y Centinelas momentaneamente. Despues ir hasta el componente NavEscenario y seleccionar su hijo IANav para generar el NavMesh.


## Tecnologías utilizadas

- Unity 6
- C#
- Git y GitHub
