<template>
  <div class="container-fluid">
    <div>
      <nav class="navbar navbar-expand-lg ">
        <div class="container-fluid">
          <div class="collapse navbar-collapse">
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
              <li class="nav-item">
                <span v-if="usuario.Nombre != ''" class="nav-link disabled"> <b> Usuario:</b> {{ usuario.Nombre }}</span>
              </li>
              <li class="nav-item">
                <span v-if="usuario.Monto != ''" class="nav-link  disabled"> <b> Monto: $</b> {{ usuario.Monto }}</span>
              </li>
            </ul>
          </div>
          <button v-if="usuario.Nombre" class="navbar-brand btn btn-success" @click="recargarSaldoModal()"
            data-bs-toggle="modal" data-bs-target="#modal">Recargar saldo</button>
          <button v-if="usuario.Nombre" class="navbar-brand btn btn-danger" @click="CerrarSesion()">Cerrar
            Sesión</button>
        </div>
      </nav>

    </div>


    <div v-if="ViewInicio" class="fade-in d-flex justify-content-center align-items-center"
      style="height: 90vh; position: relative;">
      <img src="../assets/IMG/casino.png" alt="Casino"
        class="img-fluid position-absolute top-0 start-50 translate-middle-x" style="max-height: 80%; max-width: 80%;">
      <div class="container">
        <div class="text-center">
          <h1 class="mt-5">Bienvenido al Juego de la Ruleta</h1>
          <div class="mt-5 offset-md-3">
            <div class="row">              
              <div class="col-3">
                <button id="btnModalIniciarSesion" @click="mostrarModalIniciarSesion" data-bs-toggle="modal"
                  data-bs-target="#modal" class="btn btn-primary btn-lg w-100">
                  <div class="card-body">
                    <i class="bi bi-box-arrow-in-right fs-2"></i>
                    <div class="card-text">Iniciar Sesión</div>
                  </div>
                </button>
              </div>
              <div class="col-3">
                <button id="btnModalRegistrarse" @click="mostrarModalRegistrarse" data-bs-toggle="modal"
                  data-bs-target="#modal" class="btn btn-secondary btn-lg w-100">
                  <div class="card-body">
                    <i class="bi bi-person-plus fs-2"></i>
                    <div class="card-text">Registrarse</div>
                  </div>
                </button>
              </div>
              <div class="col-3">
                <button @click="mostrarModalInvitado" data-bs-toggle="modal" data-bs-target="#modal"
                  class="btn btn-light btn-lg w-100">
                  <div class="card-body">
                    <i class="bi bi-person-raised-hand fs-2"></i>
                    <div class="card-text">Jugar como Invitado</div>
                  </div>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>



    <div v-else-if="ViewIniciarSesion" class="fade-in">
      <h1 class="mt-5">Iniciar Sesión</h1>
      <div class="mt-3">
        <label for="nombre">Nombre de Usuario:</label>
        <input type="text" id="nombre" v-model="nombre" class="form-control" />
      </div>
      <button @click="iniciarSesion" class="btn btn-success mt-3">Iniciar Sesión</button>
      <button @click="mostrarRegistro = true" class="btn btn-link mt-2">Registrarse</button>
    </div>
    <div v-else-if="ViewRegistrarse" class="fade-in">
      <h1 class="mt-5">Registro</h1>
      <div class="mt-3">
        <label for="nombreRegistro">Nombre de Usuario:</label>
        <input type="text" id="nombreRegistro" v-model="nombreRegistro" class="form-control" />
      </div>
      <div class="mt-3">
        <label for="montoRegistro">Monto:</label>
        <input type="number" id="montoRegistro" class="form-control" />
      </div>
      <button @click="registrarse" class="btn btn-success mt-3">Registrarse</button>
    </div>

    <!-- Juego -->
    <div v-if="ViewJuego" class="fade-in">
      <!-- Ruleta -->
      <div class="me-auto ms-auto" style="position: relative; width: 400px; height: 400px; overflow: hidden;">
        <div ref="rouletteContainer" style="width: 100%; height: 100%; transition: transform 5s ease;">
          <img src="../assets/whel.png" alt="Ruleta" style="width: 100%; height: 100%;">
        </div>
        <div ref="arrow" style="width: 0px; 
            height: 0px; 
            border-left: 15px solid transparent; 
            border-right: 15px solid transparent; 
            border-top: 30px solid white; 
            position: absolute; 
            top: 0; 
            left: calc(50% - 15px);">
        </div>
      </div>

      <!-- Formulario de apuesta -->
      <div class="container">
        <div class="mb-3">
          <label for="tipoApuesta" class="form-label">Tipo de apuesta:</label>
          <select id="tipoApuesta" v-model="TipoApuesta" v-on:change="onChangeDisabled" class="form-select">
            <option value="1">Color</option>
            <option value="2">Color y pares o impares</option>
            <option value="3">Color y número</option>
          </select>
        </div>
        <hr />
        <div class="row g-3">
          <div class="col-md-6">
            <label for="color" class="form-label">Color:</label>
            <select id="color" v-model="Apuesta.Color" class="form-select">
              <option selected>Rojo</option>
              <option>Negro</option>
            </select>
          </div>
          <div class="col-md-6">
            <label for="tipoNumero" class="form-label">Tipo de número:</label>
            <select id="tipoNumero" v-model="Apuesta.TipoNumero" :disabled="TipoNumeroDisabled" class="form-select">
              <option value="">Ninguno</option>
              <option>Par</option>
              <option>Impar</option>
            </select>
          </div>
          <div class="col-md-6">
            <label for="numero" class="form-label">Número:</label>
            <input id="numero" v-model="Apuesta.Numero" min="0" max="36" :disabled="NumeroDisabled" type="number"
              class="form-control" />
          </div>
          <div class="col-md-6">
            <label for="monto" class="form-label">Monto:</label>
            <input id="monto" v-model="Apuesta.Monto" type="number" min="1" class="form-control" />
          </div>
        </div>
        <div class="d-flex justify-content-center align-items-center mt-3">
          <button class="btn btn-success" v-on:click="iniciarApuesta()" :disabled="isSpinning">Iniciar Apuesta</button>
          <button v-if="this.usuario === 'Invitado'" class="btn btn-primary" v-on:click="guardarPartida">Guardar
            Partida</button>
        </div>
      </div>


    </div>

    <!-- Modal principal -->
    <div class="modal fade" id="modal" tabindex="-1" role="dialog" data-bs-backdrop="static" data-bs-keyboard="false"
      aria-labelledby="staticBackdropLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modal-title-id">Ingresar Monto</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" @click="cerrarModal"
              aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div v-if="btnIniciarSesion || btnRegistrarse" class="form-group">
              <label for="monto" class="lblTextLabel">Nombre:</label>
              <input type="text" required="required" class="form-control" id="nombreModal" />
            </div>
            <div v-if="btnInvitado || btnRegistrarse || btnRecargarSaldo" class="form-group">
              <label for="monto" class="lblTextLabel">Monto:</label>
              <input type="number" required="required" class="form-control" id="montoModal" @input="validarMonto"
                min="0" />
            </div>
            <div v-if="lblMostrarError">
              <label><b id="lblError" class="text-danger"></b> </label>
            </div>
          </div>
          <div class="modal-footer">
            <button id="btnInvitado" v-if="btnInvitado" type="button" class="btn btn-primary"
              @click="guardarModalInvitado" data-bs-dismiss="modal">Aceptar</button>
            <button id="btnIniciarSesion" data-bs-dismiss="modal" v-if="btnIniciarSesion" type="button"
              class="btn btn-primary" @click="IniciarSesionModal">Iniciar sesion</button>
            <button id="btnRegistrarse" data-bs-dismiss="modal" v-if="btnRegistrarse" type="button"
              class="btn btn-primary" @click="registrarseModal">Registrarse</button>

            <button id="btnRecargarSaldo" data-bs-dismiss="modal" v-if="btnRecargarSaldo" type="button"
              class="btn btn-primary" @click="recargarSaldo()">Recargar saldo</button>

            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>

          </div>
        </div>
      </div>
    </div>

    <!-- Modal resultado apuesta -->
    <button data-bs-toggle="modal" data-bs-target="#modalApuesta" style="display:none" id="btnModalResultado"></button>
    <div class="modal fade" id="modalApuesta" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5" id="exampleModalLabel">Resultado de apuesta</h1>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div class="form-control">
              <label class="lblTextLabel">Resultado: <b id="lblResultado"></b></label>
              <input id="inpResultadoApuesta" class="form-control" disabled />
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Confirmar Cerrar sesion -->
    <button data-bs-toggle="modal" data-bs-target="#modalConfirmarCerrerSesion" style="display:none"
      id="btnModalCerrarSesion"></button>
    <div class="modal fade" id="modalConfirmarCerrerSesion" tabindex="-1" aria-labelledby="exampleModalLabel"
      aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5" id="exampleModalLabel">¿Desea salir sin guardar?</h1>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div class="form-control">
              <label class="lblTextLabel text-danger"><b>Si no guarda la partida, se perderan los datos.</b></label>
            </div>
          </div>
          <div class="modal-footer justify-content-center">
            <button id="btnModalRegistrarse2" type="button" class="btn btn-success" @click="mostrarModalRegistrarse" data-bs-toggle="modal"
              data-bs-target="#modal">Guardar partida</button>
            <button type="button" class="btn btn-danger" @click="Inicio" data-bs-dismiss="modal">Salir sin
              guardar</button>
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Confirmacion -->
    <button data-bs-toggle="modal" data-bs-target="#modalConfirmacion" style="display:none"
      id="btnModalConfirmacion"></button>
    <div class="modal fade" id="modalConfirmacion" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5" id="exampleModalLabel">Saldo recargado correctamente</h1>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div class="form-control">
              <input id="inpResultadoConfirmacion" class="form-control" disabled />
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>

import { apiUrl } from '../../config.js';
export default {
  data() {
    return {
      usuario: { Nombre: "", Monto: 0 },
      Apuesta: {
        TipoNumero: "Par",
        Color: "Rojo",
        Numero: "",
        Monto: 0,
        MontoResultante: 0
      },
      TipoNumeroDisabled: true,
      NumeroDisabled: true,
      TipoApuesta: 1,

      ViewInicio: true,
      ViewIniciarSesion: false,
      ViewRegistrarse: false,
      ViewJuego: false,

      btnInvitado: false,
      btnIniciarSesion: false,
      btnRegistrarse: false,
      btnRecargarSaldo: false,
      mostrarNombreRegistro: false,
      //Ruleta
      isSpinning: false,
      stopNumber: null,
      // Orden de los números según la orientación correcta de la ruleta
      numbers: [0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26],

      lblMostrarError: false,
      responseData: null,

    };


  },
  methods: {

    CerrarSesion() {
      if (this.usuario.Nombre !== "Invitado") {
        this.Inicio();
      } else {
        document.getElementById("btnModalCerrarSesion").click()
      }
    },
    Inicio() {
      this.ViewInicio = true,
        this.ViewIniciarSesion = false,
        this.ViewRegistrarse = false,
        this.ViewJuego = false

      this.usuario.Monto = "";
      this.usuario.Nombre = "";

    },
    mostrarModalInvitado() {

      this.btnInvitado = true;
      this.btnRegistrarse = false,
        this.btnIniciarSesion = false
      this.btnRecargarSaldo = false;


      document.getElementById("modal-title-id").innerHTML = 'Digite el monto';
    },
    guardarModalInvitado() {
      let monto = document.getElementById("montoModal").value;
      this.usuario.Monto = monto;
      this.usuario.Nombre = 'Invitado'

      this.ViewJuego = true;
      this.ViewInicio = false;
      //Redireccionar a pantala de juego
    },

    mostrarModalIniciarSesion() {
      this.btnInvitado = false;
      this.btnIniciarSesion = true;
      this.btnRegistrarse = false
      this.btnRecargarSaldo = false;


      document.getElementById("modal-title-id").innerHTML = 'Digite su nombre';
    },
    async IniciarSesionModal() {
      let nombre = document.getElementById("nombreModal").value;

      try {
        const user = await this.LogunUser(nombre);
        if (user.data == null) {
          setTimeout(() => {
            document.getElementById("btnModalIniciarSesion").click();
            this.mostrarModalIniciarSesion();
            this.lblMostrarError = true;
          }, 1000);
          setTimeout(() => {
            document.getElementById("lblError").innerHTML = "Usuario no encontrado, intente nuevamente.";

          }, 2000);

          setTimeout(() => {
            this.lblMostrarError = false;
          }, 8000);

        } else {
          this.usuario.Nombre = user.data.nombre;
          this.usuario.Monto = user.data.monto;
          this.ViewInicio = false;
          this.ViewJuego = true;
        }
      } catch (error) {
        console.error('No se pudo iniciar sesión:', error);
      }
    },
    mostrarModalRegistrarse() {
      this.btnInvitado = false;
      this.btnIniciarSesion = false;
      this.btnRegistrarse = true;
      this.btnRecargarSaldo=false;

      document.getElementById("modal-title-id").innerHTML = 'Digite sus credenciales';

      setTimeout(() => {
        if (this.usuario.Monto) {
          document.getElementById("montoModal").value = this.usuario.Monto;          
        }
        
      }, 100);
    },
    recargarSaldoModal() {
      this.btnRecargarSaldo = true;
      this.btnInvitado = false;
      this.btnIniciarSesion = false;
      this.btnRegistrarse = false;
    },
    recargarSaldo() {
      let montoModal = document.getElementById("montoModal").value;
      if(montoModal<=0){
        alert("El saldo a recargar no puede ser 0 o menor a 0");
        return;
      }

      if (this.usuario.Nombre === "Invitado") {
        this.usuario.Monto = Number(this.usuario.Monto) + Number(montoModal);
      } else {
        const montoNuevo = this.usuario.Monto = Number(this.usuario.Monto) + Number(montoModal);
        const user = {
          Nombre: this.usuario.Nombre,
          Monto: montoNuevo
        }
        this.AddUser(user);
      }
      //Confirmar nuevo saldo
      document.getElementById("btnModalConfirmacion").click();
      document.getElementById("inpResultadoConfirmacion").value = this.usuario.Monto;
    },
    async registrarseModal() {
      let monto = document.getElementById("montoModal").value;
      let nombre = document.getElementById("nombreModal").value;
      const usuario = {
        Nombre: nombre,
        Monto: monto
      };

      try {
        // Verificar si el usuario existe:
        const ExistUser = await this.LogunUser(nombre);
        console.log(ExistUser) 
        if (ExistUser.data != null) {
          setTimeout(() => {
            document.getElementById("btnModalRegistrarse2").click();
            this.lblMostrarError = true;
          }, 500);
          setTimeout(() => {
            document.getElementById("lblError").innerHTML = "Este usuario ya existe.";

          }, 500);

          setTimeout(() => {
            this.lblMostrarError = false;
          }, 8000);

          return;
        } else {

          await this.AddUser(usuario); 

          this.ViewInicio = false;
          this.ViewJuego = true;
          this.usuario.Monto = monto;
          this.usuario.Nombre = nombre;
        }
      } catch (error) {
        console.error('No se pudo iniciar sesión:', error);
      }

    },
    iniciarApuesta() {
      // Obtener un número aleatorio del backend
      this.fetchData()
        .then(() => {
          if (this.Apuesta.Monto <= 0) {
            alert("Apueste un numero valido mayor a 0");
          } else if (this.Apuesta.Monto > this.usuario.Monto) {
            alert("No puedes apostar un monto mayor a tu saldo");
          }
          else if (this.usuario.Monto < 0) {
            alert("Tu saldo esta en negativo, debes recargar.");
          }
          else if (this.Apuesta.Numero < 0 || this.Apuesta.Numero>36) {
            alert("El numero de apuesta debe ser de 0 a 36.");
          }
          else {

            this.spinRoulette();

            this.fetchPremios();
          }
        })
        .catch(error => {
          console.error('Error al iniciar apuesta:', error);
        });
    },
    onChangeDisabled() {
      //Tipo de apuesta
      if (this.TipoApuesta == '2') {
        this.TipoNumeroDisabled = false;
        this.NumeroDisabled = true;

      } else if (this.TipoApuesta == '3') {
        this.NumeroDisabled = false;
        this.TipoNumeroDisabled = true;
      } else {
        this.NumeroDisabled = true;
        this.TipoNumeroDisabled = true;
      }
    },

    //Configuracion de ruleta
    spinRoulette() {
      if (!this.isSpinning && this.stopNumber !== null) {
        this.isSpinning = true;
        const rouletteContainer = this.$refs.rouletteContainer;
        const numIndex = this.numbers.indexOf(this.stopNumber);
        if (numIndex !== -1) {
          const degreesPerNumber = 360 / this.numbers.length;
          const offsetDegrees = (this.numbers.length - numIndex) * degreesPerNumber;
          const totalDegrees = 720 + offsetDegrees; 
          rouletteContainer.style.transition = 'transform 0s'; 
          rouletteContainer.style.transform = `rotate(0deg)`; 
          setTimeout(() => {
            rouletteContainer.style.transition = 'transform 5s ease'; 
            rouletteContainer.style.transform = `rotate(${totalDegrees}deg)`; 
          }, 50);

          setTimeout(() => {
            this.isSpinning = false;

          }, 5000);
        } else {
          this.isSpinning = false;
          alert('Número inválido.');
        }
      }
    },

    //HTTP
    fetchData() {//Obtener color y numero
      return fetch(`${apiUrl}/GetRandomNumberAndColor`)
        .then(response => {
          if (!response.ok) {
            throw new Error('Error al obtener los datos');
          }
          return response.json();
        })
        .then(data => {
          this.stopNumber = data.data.number;
        })
        .catch(error => {
          console.error('Error al obtener datos:', error);
        });
    },
    fetchPremios() {
      const TipoApuesta = this.TipoApuesta; 
      const Monto = this.Apuesta.Monto; 
      const Color = this.Apuesta.Color; 
      const TipoNumber = this.Apuesta.TipoNumero; 
      const Numero = this.Apuesta.Numero; 
      const RandomNumber = this.stopNumber; 

      const url = `${apiUrl}/GetMontoApuesta?TipoApuesta=${TipoApuesta}&Monto=${Monto}&Color=${Color}&TipoNumber=${TipoNumber}&Numero=${Numero}&RandomNumber=${RandomNumber}`;

      return fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      })
        .then(response => {
          if (!response.ok) {
            return response.json().then(error => {
              throw new Error(error.message || 'Error al obtener los datos');
            });
          }
          return response.json();
        })
        .then(data => {
          setTimeout(() => {
            this.MostarResultadosApuesta(data);
          }, 5000);
        })

        .catch(error => {
          console.error('Error al obtener datos:', error);
          alert(error.message);

        });
    },
    async LogunUser(nombreU) {
      const url = `${apiUrl}/GetUser?nombreU=${encodeURIComponent(nombreU)}`;

      try {
        const response = await fetch(url, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json'
          }
        });

        if (!response.ok) {
          const error = await response.json();
          throw new Error(error.message || 'Error al obtener el usuario');
        }

        const data = await response.json();
        return data; 

      } catch (error) {
        console.error('Error al obtener el usuario:', error);
        alert(error.message); 
      }
    },
    async AddUser(usuario) {
      const url = `${apiUrl}/AddUser`;

      try {
        const response = await fetch(url, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(usuario) // Enviar el objeto usuario en el cuerpo de la solicitud
        });

        if (!response.ok) {
          const error = await response.json();
          throw new Error(error.message || 'Error al agregar el usuario');
        }
      } catch (error) {
        console.error('Error al agregar el usuario:', error);
        alert(error.message); // Mostrar una alerta con el mensaje de error
        throw error; // Volver a lanzar el error para que pueda ser manejado fuera
      }
    },
    MostarResultadosApuesta(data) {

      document.getElementById("btnModalResultado").click();

      let updatedMonto;

      if (data.gano) {
        updatedMonto = Number(this.usuario.Monto) + Number(data.ganancia);
        document.getElementById("inpResultadoApuesta").value = data.ganancia
        document.getElementById("lblResultado").innerHTML = "Felicidades, has ganado!"
      } else {
        updatedMonto = Number(this.usuario.Monto) - Number(data.ganancia);
        document.getElementById("inpResultadoApuesta").value = data.ganancia
        document.getElementById("lblResultado").innerHTML = "Lo siento, has perdido"
      }
      this.usuario.Monto = updatedMonto;
      this.AddUser(this.usuario);
    },
    validarMonto(event) {
      let valor = event.target.value;
      if (valor < 0) {
        document.getElementById("montoModal").value = 0;
      }
    }

  },
  mounted() {
    function limpiarCampos() {
      var campos = document.getElementById('modal').querySelectorAll('input');

      campos.forEach(function (campo) {
        campo.value = '';
      });
    }
    document.getElementById('modal').addEventListener('show.bs.modal', limpiarCampos);
  }



}
</script>

<style>
.fade-in {
  animation: fadeInAnimation 1s ease-in-out forwards;
}

@keyframes fadeInAnimation {
  from {
    opacity: 0;
  }

  to {
    opacity: 1;
  }
}

.wrap {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  width: 100%;
  height: 100%;

  canvas {
    height: 300px;
    width: 300px;
  }
}

body {
  background-color: #990909;
  background: url('../assets/IMG/background.jpg') no-repeat center center fixed;
  background-size: cover;
  font-family: 'Roboto', sans-serif;
}

h1,
h2,
h3, 
h4,
h5,
h6 {
  font-family: 'Cinzel', serif;
  color: #FFD700;
}

p,
span,
a,
button,
label {
  color: #FFFFFF;
}

.modal-title,
.lblTextLabel {
  color: black
}

.nav-link.disabled {
  color: #FFD700;
  font-family: 'Cinzel', serif;
  font-size: larger;
}

.form-label {
  font-size: large;
  margin-bottom: 0;
}
</style>