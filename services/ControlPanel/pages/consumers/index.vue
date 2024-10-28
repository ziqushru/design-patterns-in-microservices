<script lang="ts" setup>

useHead({
    title: 'Καταναλωτές - ContractCon',
});

definePageMeta({
    layout: 'admin'
});

</script>

<template>
    <v-btn to="/consumers/create" color="purple-darken-3">Νέος Καταναλωτής</v-btn>
    <ConsumerDataTable />
</template>
