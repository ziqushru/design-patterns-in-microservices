<script setup lang="ts">

const emits = defineEmits<{
    (e: 'closeDrawer'): void
}>();

</script>
<template>
    <v-app-bar app>
        <v-app-bar-nav-icon @click="emits('closeDrawer')" />

        <div class="d-flex justify-between align-center w-100">
            <v-toolbar-title>ContraCon</v-toolbar-title>
        </div>
    </v-app-bar>
</template>
