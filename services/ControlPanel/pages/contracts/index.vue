<script lang="ts" setup>

useHead({
    title: 'Συμβάσεις - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <v-btn to="/contracts/create" color="purple-darken-3">Νέα Σύμβαση</v-btn>
    <ContractDataTable />
</template>
