<script setup lang="ts">
  import { ref } from 'vue'
  import { RouterLink } from 'vue-router'
  import HelloWorld from './components/HelloWorld.vue'
  // Controller
  const counter = ref(0);
  const number = ref(1);
  // Redis
  const counterCache = ref(0);
  // Kafka
  const produceTopic = ref();
  const produceMessage = ref();
  const consumeTopic = ref();
  const kafkaMessage = ref([]);

  // const server = "https://localhost:4560";
  const server = import.meta.env.VITE_API_URL;
  /**************************************************/
  // Controller
  /**************************************************/
  async function AddCounter() {
    try {
      await fetch(`${server}/counter`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Id: 1, Value: number.value })
      })
    } catch (err) {
      console.log(err);
    }
    return GetValue();
  }
  async function GetValue() {
    try {
      const res = await fetch(`${server}/counter`);
      const data = await res.json();
      counter.value = Number(data.value);
    } catch (err) {
      console.log(err)
    }
  }
  /**************************************************/
  // Redis
  /**************************************************/
  async function GetCacheValue() {
    try {
      const res = await fetch(`${server}/counter/cache`);
      const data = await res.json();
      counterCache.value = Number(data.value);
    } catch (err) {
      console.log(err)
    }
  }
  /**************************************************/
  // Kafka
  /**************************************************/
  async function ProduceKafkaMessage() {
    try {
      await fetch(`${server}/kafka/message`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            Topic: produceTopic.value,
            Message: {
              Key: null,
              Value: produceMessage.value
            }
          })
        });
      consumeTopic.value = produceTopic.value;
      ConsumeKafkaMessage();
    } catch (err) {
      console.log(err)
    }
  }
  async function ConsumeKafkaMessage() {
    try {
      const params = new URLSearchParams({
        Topic: consumeTopic.value
      });
      const res = await fetch(`${server}/kafka/message?${params}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
      });
      const data = await res.json();
      kafkaMessage.value = data;
    } catch (err) {
      console.log(err)
    }
  }
</script>

<template>
  <header>
    <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />

    <div class="wrapper">
      <HelloWorld msg="You did it!" />

      <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav>
    </div>
  </header>
  <div style="width: 100%;height: 100%; display: flex;justify-content: center; margin-top: 1rem;">
    <div style="border: 1px solid; padding: 2rem;display: flex; flex-direction: column; gap:1rem;width: fit-content; ">
      <div>
        <h1 style="text-wrap: nowrap;">Controller</h1>
      </div>
      <div style="text-wrap: nowrap;display: flex; justify-content: space-between; font-size: 20px;"><span>Server is:
        </span>{{ server }}
      </div>
      <div style="display: flex; gap:1rem;justify-content: space-between;flex-grow: 1; ">
        <button @click="GetValue">Get Value</button>
        <div style="display: flex; justify-content: space-between;gap:1rem;">Value is : <span :v-model="counter">{{
          counter
            }}</span></div>
      </div>
      <div style="display: flex; justify-content: space-between;">
        <div><button @click="AddCounter">AddCounter Value</button></div>
        <div style="display: flex;gap: 1rem;">
          <div>Input Value: </div>
          <div style="width:30px;"><input v-model="number" type="number"
              style="box-sizing: border-box;width: 100%; text-align: right;" />
          </div>
        </div>
      </div>
      <div>
        <h1 style="text-wrap: nowrap;">Redis Cache</h1>
      </div>
      <div style="display: flex; gap: 1rem; text-wrap: nowrap; justify-content: space-between; ">
        <button @click="GetCacheValue">Get Cache Value</button>
        <div style="display: flex; gap:1rem;">Cache Value is:
          <span :v-model="counterCache">{{ counterCache }}</span>
        </div>
      </div>
      <div>
        <h1 style="text-wrap: nowrap;">Kafka Test</h1>
      </div>
      <div style="display: flex; gap: 1rem; text-wrap: nowrap; justify-content: space-between; ">
        <button @click="ProduceKafkaMessage">Produce Topic</button>
        <div style="display: flex; flex-direction: column; width: 30%;">
          <div style="width: 100%; display: flex;gap: 1rem; "><span>Topic:</span>
            <input style="width: 100%;" v-model="produceTopic" />
          </div>
          <div style="display: flex;gap:1rem; width: 100%; text-align: right;"><span>Message:</span>
            <input style="width: 100%;text-align: right;" v-model="produceMessage" />
          </div>
        </div>
      </div>
      <div style="display: flex; gap: 1rem; text-wrap: nowrap; justify-content: space-between; ">
        <button @click="ConsumeKafkaMessage">Consume Topic</button>
        <div style="display: flex; flex-direction: column; width: 30%;">
          <div style="width: 100%; display: flex;gap: 1rem; "><span>Topic:</span>
            <input style="width: 100%;" v-model="consumeTopic" />
          </div>
        </div>
      </div>
      <div style="display: flex; flex-direction: column;  gap: 1rem; text-wrap: nowrap; --ele-h:2rem  ">
        <h3 style="text-align: center;">Messages</h3>
        <div style="border: 1px solid; overflow-y: scroll; height: calc(var(--ele-h)*10);">
          <div v-for="(i, idx) in kafkaMessage" :key="idx" style="height: var(--ele-h);">{{ i }}</div>
        </div>
      </div>
    </div>
  </div>
  <!-- <RouterView /> -->
</template>

<style scoped>
  header {
    line-height: 1.5;
    max-height: 100vh;
  }

  .logo {
    display: block;
    margin: 0 auto 2rem;
  }

  nav {
    width: 100%;
    font-size: 12px;
    text-align: center;
    margin-top: 2rem;
  }

  nav a.router-link-exact-active {
    color: var(--color-text);
  }

  nav a.router-link-exact-active:hover {
    background-color: transparent;
  }

  nav a {
    display: inline-block;
    padding: 0 1rem;
    border-left: 1px solid var(--color-border);
  }

  nav a:first-of-type {
    border: 0;
  }

  @media (min-width: 1024px) {
    header {
      display: flex;
      place-items: center;
      padding-right: calc(var(--section-gap) / 2);
    }

    .logo {
      margin: 0 2rem 0 0;
    }

    header .wrapper {
      display: flex;
      place-items: flex-start;
      flex-wrap: wrap;
    }

    nav {
      text-align: left;
      margin-left: -1rem;
      font-size: 1rem;

      padding: 1rem 0;
      margin-top: 1rem;
    }
  }

  input[type="number"]::-webkit-inner-spin-button,
  input[type="number"]::-webkit-outer-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }
</style>
