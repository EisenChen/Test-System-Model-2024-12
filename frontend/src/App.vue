<script setup lang="ts">
  import { ref } from 'vue'
  import { RouterLink, RouterView } from 'vue-router'
  import HelloWorld from './components/HelloWorld.vue'
  const counter = ref(0);
  const number = ref(1);
  // const server = "https://localhost:4560";
  const server = import.meta.env.VITE_API_URL;

  async function Add() {
    // counter.value += number.value;
    // return;
    try {
      console.log('send message');
      await fetch(`${server}/counter`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Id: 1, Value: number.value })
      })
      console.log('send message done');
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
    <div>
      <div>Server is:{{ server }}</div>
      <button @click="GetValue">Get Value</button>
      <button @click="Add">Add Value</button>
      <input v-model="number" type="number" />
      <div>Value is : <span :v-model="counter">{{ counter }}</span></div>
    </div>
  </header>

  <RouterView />
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
</style>
