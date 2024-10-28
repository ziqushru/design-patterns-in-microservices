<script setup lang="ts">
import 'devextreme/dist/css/dx.material.purple.dark.css';
import 'element-plus/theme-chalk/dark/css-vars.css';
import 'assets/styles.scss'
</script>
<template>
    <NuxtLoadingIndicator />
    <NuxtLayout>
        <NuxtPage />
    </NuxtLayout>
</template>
<style>
* {
    --fk-max-width-input: 100% !important;
    --fk-bg-submit: #6A1B9A;
    --fk-bg-submit-hover: #801fbc;
}

::-webkit-scrollbar {
    width: 12px;
}

::-webkit-scrollbar-track {
    background: #454545;
}

::-webkit-scrollbar-thumb {
    background-color: #888;
    border-radius: 20px;
    border: 3px solid #454545;
}

::-webkit-scrollbar-thumb:hover {
    background: #717171;
}

.formkit-asterisk {
    color: rgb(255, 123, 123);
    margin-left: .4rem;
}

.formkit-input {
    color: white !important;
    height: 43px;

}

.formkit-message {
    color: rgb(255, 123, 123) !important;
    font-size: 1rem !important;
}

.v-btn__content {
    text-transform: none !important;
}
</style>
