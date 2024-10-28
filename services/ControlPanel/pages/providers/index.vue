<script lang="ts" setup>

useHead({
    title: 'Πάροχοι - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <v-btn to="/providers/create" color="purple-darken-3">Νέος Πάροχος</v-btn>
    <ProviderDataTable />
</template>
