<template>
  <div id="App">
    <HelloWorld/>

  </div>
</template>

<script>
import HelloWorld from './components/HelloWorld.vue';


export default {
  name: 'App',
  components: {
    HelloWorld
  },
  created() {
          window.addEventListener('beforeunload', this.mostrarAlerta);
        },
        beforeUnmount() {
          window.removeEventListener('beforeunload', this.mostrarAlerta);
        },

methods:{
  mostrarAlerta(event) {
      event.preventDefault();
      event.returnValue = ''; // Esto es necesario para mostrar la alerta en algunos navegadores
      return ''; // Esto es necesario para mostrar la alerta en algunos navegadores
    },
}
}
</script>

<style>

</style>